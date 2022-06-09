using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Presentation_CenterInformation : MonoBehaviour
{
    public enum AnimationState
    {
        Wait, 
        Ojigi,
        Tewohuru,
        Pose1,
        Pose2,
    }

    // 開始時に表示するメニューのトランジション.
    [SerializeField] UITransition startMenuRootTransition = null;
    //
    [SerializeField] RectTransform startMenuRootRect = null;
    [SerializeField] VerticalLayoutGroup startMenuLayout = null;
    // 動画.
    [SerializeField] MovieMonitor movie = null;
    // UIの自動Hide.
    [SerializeField] UIAutoHide autoHide = null;
    [SerializeField] Button playPauseButton = null;
    // 再生ボタンのImage.
    [SerializeField] Image playButtonImage = null;
    // 再生ボタンのSprite.
    [SerializeField] Sprite playButtonSprite = null;
    // 一時停止ボタンSprite.
    [SerializeField] Sprite pauseButtonSprite = null;
    // アニメーター.
    [SerializeField] Animator anim = null;

    // 動画再生管理クラス.
    PvDistributedVideoPlayer video = null;
    InteractItem_Presentation interact = null;

    void Start()
    {
        startMenuRootTransition.gameObject.SetActive( false );
        autoHide.IsActiveAutoHide = false;
        // autoHide.gameObject.SetActive( false );
        playPauseButton.gameObject.SetActive( false );
        interact = GetComponent<InteractItem_Presentation>();

        AppGameManager.Instance.PresentationStartEvent.AddListener( OnPresentationStarted );
        AppGameManager.Instance.PresentationEndEvent.AddListener( OnPresentationEnd );
        AppGameManager.Instance.BindScreenSize( gameObject, OnScreenSizeChanged );

        Init();
    }

    void Update()
    {
        UpdateSize();
    }

    void Init()
    {
        // if( video == null ) video = AppGameManager.Instance.AppVideoController.GetUniqueVideo( movie.FileName );
        if( video == null ) video = AppGameManager.Instance.AppSoundController.GetVideo( movie.FileName );
    }
    
    void UpdateSize()
    {
        if( interact.IsOpen == false ) return;

        // バインドするとScreenサイズ変化と取得のタイミングが合わないので、Updateでフラグをつけて設定する.
        movie.SetSize( false );
        var _width = (float)Screen.width;
        var _height = (float)Screen.height;
        UiUtility.SetRectTransformStretch( startMenuRootRect, 0, 0, _width * 0.1f, _width * 0.1f );
        // def : 0 0 80 80
        if( Screen.width > Screen.height )
        {
            var _left = _width * 0.1f;
            startMenuLayout.padding.left = (int)_left;
            startMenuLayout.padding.bottom = 80;
        }
        else
        {
            var _bottom = _height * 0.2f;
            startMenuLayout.padding.left = 0;
            startMenuLayout.padding.bottom = (int)_bottom;
        }        
    }


    void OnScreenSizeChanged( Vector2 size )
    {
    }

    public void Test()
    {
        // OnScreenSizeChanged( Vector2.one );
    }

    public void OnWhatPVButtonClicked() 
    {
        Init();
        video.Play( (int)1 );
        OnVideoStarted();
    }

    public void OnMeritButtonClicked() 
    {
        Init();
        video.Play( 18.2f );
        OnVideoStarted();
    }

    public void OnIntroductionButtonClicked()
    {
        Init();
        video.Play( 33.2f );
        OnVideoStarted();
    }

    public void OnContactButtonClicked()
    {
        Init();
        video.Play( 55f );
        OnVideoStarted();
    }

    void OnVideoStarted()
    {
        Debug.Log( "プレゼンビデオスタート" );

        CloseStartMenu();
    }

    void OnPresentationStarted( InteractItem_Presentation item, bool isOpen )
    {
        if( isOpen == false ) return;

        // AppGameManager.Instance.AppVideoController.SetReadyUniqueVideo( movie.FileName, 2 );
        AppGameManager.Instance.AppSoundController.SetReadyVideo( movie.FileName, 2 );
        OpenStartMenu();
    }

    public void OnPresentationEnd()
    {
        video.Pause();        

        startMenuRootTransition.gameObject.SetActive( false );
        autoHide.IsActiveAutoHide = false;
        playPauseButton.gameObject.SetActive( false );

        SetAnim( AnimationState.Tewohuru );
    }

    public void OpenStartMenu()
    {
        startMenuRootTransition.TransitionIn();   
        autoHide.IsActiveAutoHide = false;
        playPauseButton.gameObject.SetActive( false );
    }    

    public void CloseStartMenu()
    {
        // Debug.Log( "プレゼンを閉じます" );

        startMenuRootTransition.TransitionOut();
        
        playPauseButton.gameObject.SetActive( true );
        autoHide.IsActiveAutoHide = true;
        playButtonImage.sprite = pauseButtonSprite;
    }

    public void OnChangedPlayPause( bool isPlay )
    {
        var _sp = ( isPlay == true ) ? pauseButtonSprite : playButtonSprite;
        playButtonImage.sprite = _sp;

    }

    public void OnItemClicked()
    {
        var _width = (float)Screen.width;
        var _height = (float)Screen.height;
        UiUtility.SetRectTransformStretch( startMenuRootRect, 0, 0, _width * 0.1f, _width * 0.1f );
        // def : 0 0 80 80
        if( Screen.width > Screen.height )
        {
            var _left = _width * 0.1f;
            startMenuLayout.padding.left = (int)_left;
            startMenuLayout.padding.bottom = 80;
        }
        else
        {
            var _bottom = _height * 0.2f;
            startMenuLayout.padding.left = 0;
            startMenuLayout.padding.bottom = (int)_bottom;
        }
        // キャラクリックじInspector登録用.
        SetAnim( AnimationState.Ojigi );
    }

    public void SetAnim( AnimationState animState )
    {
        switch( animState )
        {
            case AnimationState.Ojigi:
            {
                anim.SetTrigger( "Ojigi" );
            }
            break;
            case AnimationState.Tewohuru:
            {
                anim.SetTrigger( "Tewohuru" );
            }
            break;
            case AnimationState.Pose1:
            {
                if( anim.GetBool( "Pose1" ) == false ) anim.SetBool( "Pose1", true );
            }
            break;
            case AnimationState.Pose2:
            {
                anim.SetBool( "Pose2", true );
            }
            break;
        }
    }
}


