using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

// -----------------------------------------------------------------
/// <summary>
/// プラグイン、HTMLからのコールなどのクラス.
/// </summary>
// -----------------------------------------------------------------
public class AppHtmlReceiver : MonoBehaviour
{

    // プラグインインポート端末情報の取得.
    [DllImport("__Internal")]
    private static extern void DeviceInformation();
    // プラグインインポート端末情報の取得.
    [DllImport("__Internal")]
    private static extern void OSInformation();
    // プラグインインポートURLを開く.
    [DllImport("__Internal")]
    private static extern void OpenUrl( string url );
    // プラグインインポートURLを開く.
    [DllImport("__Internal")]
    private static extern void OpenUrlNewWindow( string url );
    // プラグインインポートヘッダー全体を開く.
    [DllImport("__Internal")]
    private static extern void OpenHeader();
    // プラグインインポートヘッダー全体を閉じる.
    [DllImport("__Internal")]
    private static extern void CloseHeader();

    // プラグインインポートMenuアイコンの表示.
    [DllImport("__Internal")]
    private static extern void SetMenuIconDisplay( bool isDisplay );
    // プラグインインポートSoundアイコンの表示.
    [DllImport("__Internal")]
    private static extern void SetSoundIconDisplay( bool isDisplay );
    // プラグインインポートMuteアイコンの表示.
    [DllImport("__Internal")]
    private static extern void SetMuteIconDisplay( bool isDisplay );
    // プラグインインポートMute設定.
    [DllImport("__Internal")]
    private static extern void SetMute( bool isMute );

    // Muteフラグ.
    public bool IsHtmlMute{ get; set; } = true;
    // ヘッダーオープンフラグ.
    public bool IsHtmlHeaderOpened{ get; set; } = true;
    // メニューアイコン表示フラグ.
    public bool HtmlMenu_MenuIconDisplay{ get; set; } = true;
    // サウンドアイコン表示フラグ.
    public bool HtmlMenu_SoundIconDisplay{ get; set; } = true;
    // ミュートアイコン表示フラグ.
    public bool HtmlMenu_MuteIconDisplay{ get; set; } = true;
    // 現在の画面サイズ.
    public Vector2 CurrentScreenSize{ get; set; } = Vector2.zero;

    
    public float SoundButtonHtmlPosition_Left{ get; private set; } = 0;
    public float SoundButtonHtmlPosition_Top{ get; private set; } = 0;

    // プラットフォーム情報.
    public AppGameManager.DeviceParam Platform{ get; set; } = new AppGameManager.DeviceParam();





    void Start()
    {
        if( Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor )
        {
            AppGameManager.Instance.Debug_SetPlatformText( "Editor" );
            Platform.Device = AppGameManager.Device.Editor;
            Platform.OS = AppGameManager.OS.Editor;
        }
        else
        {            
            DeviceInformation();
            OSInformation();
            AppGameManager.Instance.Debug_SetPlatformText( Platform.Device + "(" + Platform.OS + ")" );
        }

        AppGameManager.Instance.InitMoveUi( Platform.OS );

        var currentSS = CurrentScreenSize;
        if( CurrentScreenSize.x == 0 ) currentSS.x = Screen.width;
        if( CurrentScreenSize.y == 0 ) currentSS.y = Screen.height;
        CurrentScreenSize = currentSS;
    }


    // ----------------------------------------------------------------------
    /// SendMessageでWebのHTML,JSもしくはプラグインのJSLibから実行される関数.
    // ----------------------------------------------------------------------
    /// <summary>
    /// アプリ開始時コールバック.
    /// </summary>
    // ----------------------------------------------------------------------
    public void OnHTML_AppStarted()
    {
        Debug.Log( "<<  WebGl App 開始.  >>" );
    }

    // ----------------------------------------------------------------------
    /// <summary>
    /// HTMLからのリサイズコールバック(Width).
    /// </summary>
    /// <param name="width"></param>
    // ----------------------------------------------------------------------
    public void OnHTML_ResizeScreenWidth( int width )
    {
        Debug.Log( "HTMLからのコール Width : " + width );
        var _current = CurrentScreenSize;
        _current.x = width;
        CurrentScreenSize = _current;

        AppGameManager.Instance.ScreenSizeChanged?.Invoke( CurrentScreenSize );

        AppGameManager.Instance.Debug_SetPlatformText( Platform + " / " + CurrentScreenSize );
    }

    // ----------------------------------------------------------------------
    /// <summary>
    /// HTMLからのリサイズコールバック(height).
    /// </summary>
    /// <param name="width"></param>
    // ----------------------------------------------------------------------
    public void OnHTML_ResizeScreenHeight( int height )
    {
        Debug.Log( "HTMLからのコール Hight : " + height );
        var _current = CurrentScreenSize;
        _current.y = height;
        CurrentScreenSize = _current;

        AppGameManager.Instance.ScreenSizeChanged.Invoke( CurrentScreenSize );

        AppGameManager.Instance.Debug_SetPlatformText( Platform + " / " + CurrentScreenSize );
    }


    public void OnHTML_SoundButtonRectX( int x )
    {
        SoundButtonHtmlPosition_Left = (float)x;
    }

    public void OnHTML_SoundButtonRectY( int y )
    {
        SoundButtonHtmlPosition_Top = (float)y;
    }


    // -------------------------------------------------------------------------------------
    /// <summary>
    /// プラットフォーム判定（デバイス）.
    /// </summary>
    /// <param name="platformKey"></param>
    // -------------------------------------------------------------------------------------
    public void OnHTML_SetDevice( string deviceKey )
    {
        Debug.Log( "Device : " + deviceKey );

        switch( deviceKey )
        {
            case "iPhone":
            {
                Platform.Device = AppGameManager.Device.iPhone;
            }
            break;
            case "android":
            {
                Platform.Device = AppGameManager.Device.android;
            }
            break;
            case "iPad":
            {
                Platform.Device = AppGameManager.Device.iPad;
            }
            break;
            case "androidTab":
            {
                Platform.Device = AppGameManager.Device.androidTab;
            }
            break;
            case "iOSOther":
            {
                Platform.Device = AppGameManager.Device.iOSOther;
            }
            break;
            default:
            {
                Platform.Device = AppGameManager.Device.Unknown;
            }
            break;
        }
    }

    // -------------------------------------------------------------------------------------
    /// <summary>
    /// プラットフォーム判定（OS）.
    /// </summary>
    /// <param name="platformKey"></param>
    // -------------------------------------------------------------------------------------
    public void OnHTML_SetOS( string osKey )
    {
        Debug.Log( "OS : " + osKey );

        switch( osKey )
        {
            case "Windows":
            {
                Platform.OS = AppGameManager.OS.Windows;
            }
            break;
            case "android":
            {
                Platform.OS = AppGameManager.OS.Android;
            }
            break;
            case "iOS":
            {
                Platform.OS = AppGameManager.OS.iOS;
            }
            break;
            case "OSX":
            {
                Platform.OS = AppGameManager.OS.OSX;
            }
            break;
            default:
            {
                Platform.OS = AppGameManager.OS.Unknown;
            }
            break;
        }

        // platformText.text = Platform.OS + "(" + Platform.OS + ")";
    }

    // -------------------------------------------------------------------------------------
    /// <summary>
    /// ミュート判定.
    /// </summary>
    /// <param name="isMute"></param>
    // -------------------------------------------------------------------------------------
    public void OnHTML_SetMute( bool isMute )
    {
        Debug.LogWarning( "OnHTML_SetMute" );
        // var gm = AppGameManager.Instance;
        // if( isMute == true )
        // {
        //     SetSoundIconDisplay( false );
        //     HtmlMenu_SoundIconDisplay = false;
        // }
        // else
        // {
        //     if( Platform.OS == AppGameManager.OS.iOS || Platform.OS == AppGameManager.OS.Android )
        //     {
        //         SetSoundIconDisplay( false );
        //         HtmlMenu_SoundIconDisplay = false;
        //     }
        //     else
        //     {
        //         SetSoundIconDisplay( true );
        //         HtmlMenu_SoundIconDisplay = true;
        //     }
        // }
    }

    // -------------------------------------------------------------------------------------
    /// <summary>
    /// HTMLメニューボタンクリックコールバック.
    /// </summary>
    // -------------------------------------------------------------------------------------
    public void OnHTML_MenuButtonClicked()
    {
        var gm = AppGameManager.Instance;
        if( gm.SideMenu.IsOpen == false )
        {
            gm.SideMenu.Open();
            gm.SetMoveUI( false );

            gm.HideHeader();
        }
        else
        {
            gm.SideMenu.Close();
            gm.SetMoveUI( true );

            gm.ShowHeader();
        }
    }

    // -------------------------------------------------------------------------------------
    /// <summary>
    /// HTMLサウンドボタンクリックコールバック.
    /// </summary>
    // -------------------------------------------------------------------------------------
    public void OnHTML_SoundButtonClicked()
    {
        var gm = AppGameManager.Instance;

        if( gm.SideMenu.IsVolumeWindowOpened == false )
        {
            AppGameManager.Instance.SideMenu.SetVolumeWindowPosition();
            gm.SideMenu.OpenVolumeWindow();
        }
        else
        {
            gm.SideMenu.CloseVolumeWindow();
        }
    }

    // -------------------------------------------------------------------------------------
    /// <summary>
    /// HTMLミュートボタンクリックコールバック.
    /// </summary>
    // -------------------------------------------------------------------------------------
    public void OnHTML_MuteButtonClicked()
    {        
        Debug.LogWarning( "OnHTML_MuteButtonClicked" );
        var gm = AppGameManager.Instance;
        if( IsHtmlMute == true )
        {            
            // Mute解除.
            gm.SoundController.OnHtmlInit( false );
            gm.SoundController.OnHtmlMuteOff();

            IsHtmlMute = false;

            if( Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor ) return;
            
            SetMute( false );
            if( Platform.OS == AppGameManager.OS.iOS || Platform.OS == AppGameManager.OS.Android )
            {
                SetSoundIconDisplay( false );
                HtmlMenu_SoundIconDisplay = false;
            }
            else
            {
                SetSoundIconDisplay( true );
                HtmlMenu_SoundIconDisplay = true;
            }
        }
        else
        {
            // Mute.
            gm.SoundController.OnHtmlInit( true );
            gm.SoundController.OnHtmlMuteOn();
            
            IsHtmlMute = true;

            if( Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor ) return;
            
            SetMute( true );
            SetSoundIconDisplay( false );
            HtmlMenu_SoundIconDisplay = false;
        }        
    }




    // -------------------------------------------------------------------------------------
    // その他インポート関数の実行など.
    // -------------------------------------------------------------------------------------
    /// <summary>
    /// Webページのオープン.
    /// </summary>
    /// <param name="url"> URL </param>
    /// <param name="isNewWindow"> 新しいウインドウで開くフラグ. </param>
    // -------------------------------------------------------------------------------------
    public void OpenWebPage( string url, bool isNewWindow = false )
    {
        Debug.Log( url + " にアクセスします" );

        // player.StopPlayer();
        AppGameManager.Instance.OpenWebPageEvent?.Invoke();

        if( Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor )
        {
            Debug.Log( "Editorでは通常のApplication.OpenURL()を使用します" );
            Application.OpenURL( url );
        }
        else if( isNewWindow == false )
        {
            OpenUrl( url );
        }
        else
        {
            OpenUrlNewWindow( url );
        }
    }

    // -------------------------------------------------------------------------------------
    /// <summary>
    /// ヘッダー全体の表示.
    /// </summary>
    /// <param name="isOpen"> 開く閉じる. </param>
    // -------------------------------------------------------------------------------------
    public void SetHeaderDisplay( bool isOpen )
    {
        if( Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor ) return;
        if( isOpen == true ) OpenHeader();
        else CloseHeader();
    }

    // -------------------------------------------------------------------------------------
    /// <summary>
    /// ヘッダーアイコンの表示.
    /// </summary>
    /// <param name="icon"> アイコンのキー文字列. </param>
    /// <param name="isShow"> 開く閉じる. </param>
    // -------------------------------------------------------------------------------------
    public void SetIconDisplay( string icon, bool isShow )
    {
        if( Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor ) return;
        switch( icon )
        {
            case "Menu": case "menu": SetMenuIconDisplay( isShow ); break;
            case "Sound": case "sound": SetSoundIconDisplay( isShow ); break;
            case "Mute": case "mute": SetMuteIconDisplay( isShow ); break;
        }
    }

}
