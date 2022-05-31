using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OViceWarpGate : WarpGate
{
    [SerializeField] GameObject horizontalPopup = null;
    [SerializeField] GameObject verticalPopup = null;
    [SerializeField] ColliderCallReceiver colliderCall = null;
    protected override void Start()
    {
        if( colliderCall != null )
        {
            colliderCall.TriggerEnterEvent.AddListener( OnGateTriggerEnter );
        }

        AppGameManager.Instance.ScreenSizeChanged.AddListener( OnScreenSizeChanged );
    }

    PopupBase currentPop = null;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter( other );
    }

    protected override void OnGateTriggerEnter( Collider other ) 
    {
        base.OnGateTriggerEnter( other );
    }
     
    public void OnEnterWarpGate()
    {        
        AppGameManager.Instance.AppStop();
        GameObject _prefab = ( Screen.width > Screen.height ) ? horizontalPopup : verticalPopup;
        currentPop = AppGameManager.Instance.OpenPopup
        ( 
            _prefab,
            null,
            pop => // 1:同じタブで開く.
            {
                AppGameManager.Instance.ClosePopup( pop );
                AppGameManager.Instance.OpenStopWindow();
                base.ReturnPosition();
                base.Warp();

                currentPop = null;
            },
            pop => // 2:新しいタブで開く.
            {
                AppGameManager.Instance.ClosePopup( pop );
                AppGameManager.Instance.OpenStopWindow();
                base.ReturnPosition();
                base.Warp( true );

                currentPop = null;
            },
            pop => // 3:キャンセル。
            {
                AppGameManager.Instance.ClosePopup( pop );
                base.ReturnPosition();
                AppGameManager.Instance.AppRestart();

                currentPop = null;
            }
        );

        var _link = currentPop.gameObject.GetComponent<LinkGatePopup>();
        var _info = "oViceを利用した説明会の会場はこちらです。\n次回の説明会の日時は未定です。";
        _link.Init( _info );
    }

    void OnScreenSizeChanged( Vector2 currentScreen )
    {
        if( currentPop != null )
        {
            AppGameManager.Instance.ClosePopup( currentPop );
            base.ReturnPosition();
            AppGameManager.Instance.AppRestart();
        }

        currentPop = null;
    }



}
