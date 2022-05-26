using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class InteractableItemBase : MonoBehaviour //, IPointerClickHandler
{
    [System.Serializable]
    public class InformationParam
    {        
        public bool IsShowLookInformation = true;
        public Transform Point = null;
        public GameObject Prefab = null;
        public string Information = "";
        public InformationPlateBase Plate{ get; set; } = null;
    }

    [SerializeField] InformationParam information = new InformationParam();


 
    protected bool canClick = true;

    Coroutine autoHideCor = null;

    bool isDropping = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // public void OnPointerClick( PointerEventData eventData )
    // {
    //     Debug.Log( "Click----------------" );
    // }

    public virtual void OnClickPointerExit()
    {
        // Debug.Log( "外にでたーーーー" );
        isDropping = false;
        // AppGameManager.Instance.IsClickRotationLock = false;
    }

    public virtual void OnClickPointerDown()
    {
        // Debug.Log( gameObject.name + "@ Base Down" );
        isDropping = true;
        // AppGameManager.Instance.IsClickRotationLock = true;
    }

    public virtual void OnClickPointerUp()
    {
        // Debug.Log( gameObject.name + "@ Base Up" );
        if( isDropping == true ) OnClick();
        isDropping = false;
        // AppGameManager.Instance.IsClickRotationLock = false;
    }

    public virtual void OnClickPointerHold()
    {
        // Debug.Log( gameObject.name + "@ Base Hold" );
    } 

    public virtual void OnClick()
    {
        Debug.Log( "Click--" + gameObject.name );
    }

    public virtual void OnCenterPointer()
    {
        // Debug.Log( gameObject.name + "@ Center Pointer" );
        SetInformation();
    }

    public virtual void OnCenterPointerExit()
    {

        // Debug.Log( gameObject.name + "@ Exit Center Pointer" );

        if( autoHideCor != null )
        {
            StopCoroutine( AutoHide() );
            autoHideCor = null;

        }
        autoHideCor = StartCoroutine( AutoHide() );
    }

    void SetInformation()
    {
        if( information.IsShowLookInformation == false ) return;

        if( information.Point == null || information.Prefab == null )
        {
            // Debug.LogWarning( gameObject.name + "のインフォメーション設定をしてください." );
            return;
        }

        if( information.Plate == null )
        {            
            var _go = Instantiate( information.Prefab, information.Point.position, information.Point.rotation, information.Point.transform );
            information.Plate = _go.GetComponent<InformationPlateBase>();
            information.Plate.SetInformation( information );
        }
        else if( information.Plate.gameObject.activeSelf == false )
        {
            information.Plate.gameObject.SetActive( true );
            information.Plate.SetInformation( information );
        }
    }

    IEnumerator AutoHide()
    {
        if( information.Plate != null )
        {
            yield return new WaitForSeconds( 1f );
            information.Plate.gameObject.SetActive( false );
        }
    }

    IEnumerator WaitCanClick()
    {
        yield return new WaitForSeconds( 0.5f );
        canClick = true;
    }
}
