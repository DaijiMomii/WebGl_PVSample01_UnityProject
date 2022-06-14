using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractItem_Link : InteractableItemBase
{
    [SerializeField] string url = "http://google.co.jp";

    [SerializeField] GameObject popupPrefab = null;



    void Start()
    {
        
    }


    public override void OnClick()
    {
        base.OnClick();
        Debug.Log( "リンク" );
        OpenPopup();
    }

    void OpenPopup()
    {
        AppGameManager.Instance.AppStop();
        // GameObject _prefab = ( Screen.width > Screen.height ) ? horizontalPopup : verticalPopup;
        GameObject _prefab = popupPrefab;
        AppGameManager.Instance.OpenPopup
        ( 
            _prefab,
            null,
            pop =>
            {
                AppGameManager.Instance.ClosePopup( pop );
                AppGameManager.Instance.OpenStopWindow();
                // base.ReturnPosition();
                // base.Warp();
                // currentPop = null;

                Application.OpenURL( url );
            },
            pop => 
            {
            },
            pop => // 3:キャンセル。
            {
                AppGameManager.Instance.ClosePopup( pop );
                // base.ReturnPosition();
                AppGameManager.Instance.AppRestart();

                // currentPop = null;
            }
        );

        // var _link = currentPop.gameObject.GetComponent<LinkGatePopup>();
        // var _info = "oViceを利用した説明会の会場はこちらです。\n次回の説明会の日時は未定です。";
        // _link.Init( _info );
    }

}
