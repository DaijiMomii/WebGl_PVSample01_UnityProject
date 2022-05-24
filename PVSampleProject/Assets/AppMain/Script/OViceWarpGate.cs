using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OViceWarpGate : WarpGate
{
    [SerializeField] GameObject popup = null;
    [SerializeField] ColliderCallReceiver colliderCall = null;
    protected override void Start()
    {
        if( colliderCall != null )
        {
            colliderCall.TriggerEnterEvent.AddListener( OnGateTriggerEnter );
        }
    }

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
        var _pop = AppGameManager.Instance.OpenPopup
        ( 
            popup,
            null,
            pop => // 1:同じタブで開く.
            {
                AppGameManager.Instance.ClosePopup( pop );
                AppGameManager.Instance.OpenStopWindow();
                base.ReturnPosition();
                base.Warp();
            },
            pop => // 2:新しいタブで開く.
            {
                AppGameManager.Instance.ClosePopup( pop );
                AppGameManager.Instance.OpenStopWindow();
                base.ReturnPosition();
                base.Warp( true );
            },
            pop => // 3:キャンセル。
            {
                AppGameManager.Instance.ClosePopup( pop );
                base.ReturnPosition();
                AppGameManager.Instance.AppRestart();
            }
        );

        var _link = _pop.gameObject.GetComponent<LinkGatePopup>();
        _link.Init( "oVice" );
    }



}
