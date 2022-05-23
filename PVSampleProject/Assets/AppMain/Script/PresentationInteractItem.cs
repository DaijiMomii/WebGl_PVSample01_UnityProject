using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentationInteractItem : InteractableItemBase  
{
    [SerializeField] Transform presentationCameraTransform = null;
    [SerializeField] UITransition presentationTransition = null;

    public Transform PresentationCameraTransform{ get{ return presentationCameraTransform; } }

    
    List<( int, Transform )> childrenTransformWithLayer = new List<( int, Transform )>();


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void OnClick()
    {
        base.OnClick();
        Debug.Log( "プレゼン開始" );

        childrenTransformWithLayer = GetAllChildrenWithLayer( gameObject );
        foreach( ( int layerNum, Transform childTransform ) tpl in childrenTransformWithLayer )
        {
            // SUbUIレイヤーは変更しない.
            if( tpl.layerNum == 6 ) continue;

            tpl.childTransform.gameObject.layer = AppGameManager.Instance.AttentionLayerNum;
        }

        AppGameManager.Instance.SetPresentation( this );
        // presentation.gameObject.SetActive( true );
        presentationTransition.TransitionIn();

        AppGameManager.Instance.CurrentLock.Move = true;
        AppGameManager.Instance.CurrentLock.Rotation = true;
        AppGameManager.Instance.CurrentLock.Click = true;
        AppGameManager.Instance.CurrentLock.Look = true;
    }

    

    public void OnPresentationEndButtonClicked()
    {
        foreach( ( int layerNum, Transform childTransform ) tpl in childrenTransformWithLayer )
        {
            tpl.childTransform.gameObject.layer = tpl.layerNum;
        }
        
        AppGameManager.Instance.EndPresentation();
        // presentation.gameObject.SetActive( false );
        presentationTransition.TransitionOut();

        AppGameManager.Instance.CurrentLock.Move = false;
        AppGameManager.Instance.CurrentLock.Rotation = false;
        AppGameManager.Instance.CurrentLock.Click = false;
        AppGameManager.Instance.CurrentLock.Look = false;
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
