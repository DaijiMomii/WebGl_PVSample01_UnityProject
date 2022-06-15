using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---------------------------------------------------------------------------
/// <summary>
/// 選択可能オブジェクト、画像をPOPUP表示.
/// </summary>
// ---------------------------------------------------------------------------
public class InteractItem_ShowImage : InteractableItemBase
{
    // ポップアッププレハブ.
    
    [SerializeField] GameObject horizontalPopup = null;
    [SerializeField] GameObject verticalPopup = null;
 
    // WebサイトのURL.
    [SerializeField] string webSiteUrl = "";
    // ポップアップに表示する文字情報.
    [SerializeField, Multiline] string popupInformation = "";
    // ポップアップに表示する画像.
    [SerializeField] Sprite showSprite = null;

    // ポップアップ.
    PopupBase currentPop = null;

    void Start()
    {
        
    }

    // ---------------------------------------------------------------------------
    /// <summary>
    /// クリックコールバック.
    /// </summary>
    // ---------------------------------------------------------------------------
    public override void OnClick()
    {
        base.OnClick();
        Debug.Log( "画像表示" );

        AppGameManager.Instance.CurrentLock.Move = true;
        AppGameManager.Instance.CurrentLock.Rotation = true;
        AppGameManager.Instance.CurrentLock.Click = true;
        AppGameManager.Instance.CurrentLock.Look = true;

        AppGameManager.Instance.SetMoveUI( false );

        OpenPopup();
    }

    // ---------------------------------------------------------------------------
    /// <summary>
    /// ポップアップを開く.
    /// </summary>
    // ---------------------------------------------------------------------------
    void OpenPopup()
    {
        AppGameManager.Instance.AppStop();
        GameObject _prefab = ( Screen.width > Screen.height ) ? horizontalPopup : verticalPopup;
        // GameObject _prefab = popupPrefab;
        currentPop = AppGameManager.Instance.OpenPopup
        ( 
            _prefab,
            null,
            pop => // Webサイトを開く.
            {
                AppGameManager.Instance.ClosePopup( pop );
                AppGameManager.Instance.OpenStopWindow();
                currentPop = null;

                Application.OpenURL( webSiteUrl );
            },
            pop => 
            {
            },
            pop => // 閉じる.
            {
                AppGameManager.Instance.ClosePopup( pop );
                AppGameManager.Instance.AppRestart();

                AppGameManager.Instance.CurrentLock.Move = false;
                AppGameManager.Instance.CurrentLock.Rotation = false;
                AppGameManager.Instance.CurrentLock.Click = false;
                AppGameManager.Instance.CurrentLock.Look = false;

                AppGameManager.Instance.SetMoveUI( true );

                currentPop = null;
            }
        );

        var _link = currentPop.gameObject.GetComponent<ShowImagePopup>();

        _link.Init( popupInformation, showSprite );
    }
}
