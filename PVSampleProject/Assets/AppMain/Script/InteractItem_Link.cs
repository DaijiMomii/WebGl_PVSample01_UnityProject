using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractItem_Link : InteractableItemBase
{
    [SerializeField] string pageName = "";
    [SerializeField] string url = "";

    [SerializeField] GameObject horizontalPopup = null;
    [SerializeField] GameObject verticalPopup = null;
 
    PopupBase currentPop = null;

    void Start()
    {
        
    }


    public override void OnClick()
    {
        base.OnClick();
        Debug.Log( "リンク" );

        if( string.IsNullOrEmpty( url ) == false )
        {
            OpenPopup();
        }
    }

    void OpenPopup()
    {
        AppGameManager.Instance.AppStop();
        GameObject _prefab = ( Screen.width > Screen.height ) ? horizontalPopup : verticalPopup;
        // GameObject _prefab = popupPrefab;
        currentPop = AppGameManager.Instance.OpenPopup
        ( 
            _prefab,
            null,
            pop =>
            {
                AppGameManager.Instance.ClosePopup( pop );
                AppGameManager.Instance.OpenStopWindow();
                currentPop = null;

                Application.OpenURL( url );
            },
            pop => 
            {
            },
            pop => // 3:キャンセル。
            {
                AppGameManager.Instance.ClosePopup( pop );
                AppGameManager.Instance.AppRestart();

                currentPop = null;
            }
        );

        var _link = currentPop.gameObject.GetComponent<LinkGatePopup>();
        var _info = "";
        if( string.IsNullOrEmpty( pageName ) == false ) _info = pageName + "\nをブラウザで開きます。";
        else _info = url + "\nをブラウザで開きます。";
        _link.Init( _info );
    }

}
