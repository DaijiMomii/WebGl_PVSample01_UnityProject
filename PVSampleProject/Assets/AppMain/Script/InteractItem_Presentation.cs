using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractItem_Presentation : InteractableItemBase  
{
    // 
    [SerializeField] string audioFileName = "";
    // プレゼン時画面横長の時にカメラがくる位置.
    [SerializeField] Transform presentationCameraTransformHorizontal = null;
    // プレゼン時画面縦長の時にカメラがくる位置.
    [SerializeField] Transform presentationCameraTransformVertical = null;
    // プレゼントランジション.
    [SerializeField] UITransition presentationTransition = null;
    // プレゼン画面レクト.
    [SerializeField] RectTransform presentationRect = null;
    [SerializeField] AudioSource audioSource = null;

    public UnityEvent ItemClickedEvent = new UnityEvent();
    public UnityEvent PresentationClosedEvent = new UnityEvent();

    
    List<( int, Transform )> childrenTransformWithLayer = new List<( int, Transform )>();


    void Start()
    {
        presentationTransition.gameObject.SetActive( true );
        presentationTransition.TransitionOut( null, false, true, false );

        if( audioSource != null )
        {
            AppGameManager.Instance.AppSoundController.AddAudioSourceParam( audioSource, false );
        }
    }

    void Update()
    {
        
    }

    public Transform GetCameraTransformInCurrentOrientation()
    {
        if( Screen.width > Screen.height ) return presentationCameraTransformHorizontal;
        else return presentationCameraTransformVertical;
    }

    public override void OnClick()
    {
        base.OnClick();
        Debug.Log( "プレゼン開始" );

        childrenTransformWithLayer = GetAllChildrenWithLayer( gameObject );
        AppGameManager.Instance.SetMoveUI( false );
        foreach( ( int layerNum, Transform childTransform ) tpl in childrenTransformWithLayer )
        {
            // SUbUIレイヤーは変更しない.
            if( tpl.layerNum == 6 ) continue;

            tpl.childTransform.gameObject.layer = AppGameManager.Instance.AttentionLayerNum;
        }

        AppGameManager.Instance.SetPresentation( this );
        
        if( Screen.width > Screen.height )
        {
            UiUtility.SetRectTransformStretch( presentationRect, 20f, 20f, Screen.width / 6f, 20f );
        }
        else 
        {
            UiUtility.SetRectTransformStretch( presentationRect, 20f, Screen.height / 5f, 20f, 20f );
        }
        presentationTransition.TransitionIn( null, false, true );

        AppGameManager.Instance.CurrentLock.Move = true;
        AppGameManager.Instance.CurrentLock.Rotation = true;
        AppGameManager.Instance.CurrentLock.Click = true;
        AppGameManager.Instance.CurrentLock.Look = true;

        ItemClickedEvent?.Invoke();
    }

    

    public void OnPresentationEndButtonClicked()
    {
        foreach( ( int layerNum, Transform childTransform ) tpl in childrenTransformWithLayer )
        {
            tpl.childTransform.gameObject.layer = tpl.layerNum;
        }
        
        AppGameManager.Instance.EndPresentation();
        presentationTransition.TransitionOut( null, false, false, false );

        AppGameManager.Instance.CurrentLock.Move = false;
        AppGameManager.Instance.CurrentLock.Rotation = false;
        AppGameManager.Instance.CurrentLock.Click = false;
        AppGameManager.Instance.CurrentLock.Look = false;

        var _clip = AppGameManager.Instance.AppSoundController.GetAudioClip( audioFileName );
        audioSource.PlayOneShot( _clip );

        AppGameManager.Instance.SetMoveUI( true );

        PresentationClosedEvent?.Invoke();
    }

    
    List<( int layerNum, Transform childTransform )> GetAllChildrenWithLayer( GameObject parent )
    {
        var _list = new List<( int, Transform )>();

        foreach( Transform child in parent.transform )
        {
            _AddList( child );
        }

        return _list;


        void _AddList( Transform trans )
        {
            _list.Add( ( trans.gameObject.layer, trans ) );
            if( trans.childCount > 0 )
            {
                foreach( Transform _child in trans )
                {
                    _list.Add( ( _child.gameObject.layer, _child ) );
                    _AddList( _child );
                }
            }
        }
    }
}
