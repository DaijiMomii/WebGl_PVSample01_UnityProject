using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Runtime.InteropServices;

public class WarpGate : MonoBehaviour
{

    [SerializeField] protected string linkUrl = "";
    // "https://daijimomii.github.io/WebGl_PVSample_Info/"
    [SerializeField] protected bool isAutoWarp = false;
    [SerializeField] protected bool isOpenStop = true;

    // [SerializeField] protected GameObject colliderCall = null;

    [SerializeField] protected Transform returnPoint = null;

    // [SerializeField] bool isNewWindow = false;





    public UnityEvent WarpGateEvent = new UnityEvent();

    GameObject player = null;



    

    protected virtual void Start()
    {
        // Debug.Log( "Base Start" );
    }

    protected virtual void OnGateTriggerEnter( Collider other ) 
    {
        if( other.tag == "Player" )
        {
            Debug.Log( "ワープ" );
            WarpGateEvent?.Invoke();
            player = other.gameObject;

            if( isAutoWarp == true ) Warp();
        }
    }

    protected virtual void OnTriggerEnter( Collider other ) 
    {
        if( other.tag == "Player" )
        {
            Debug.Log( "ワープ" );
            WarpGateEvent?.Invoke();
            player = other.gameObject;

            if( isAutoWarp == true ) Warp();
        }
    }

    public void Warp( bool isNewWindow = false )
    {
        if( string.IsNullOrEmpty( linkUrl ) == false )
        {
            AppGameManager.Instance.OpenWebPage( linkUrl, isNewWindow );
        }
        else
        {
            Debug.LogWarning( "URLを正しく設定してください。" );
        }
    }

    public virtual void ReturnPosition()
    {
        if( returnPoint != null )
        {
            player.transform.position = returnPoint.position;
        }

        // player = null;
    }


}
