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
    AppVideoPlayer video = null;

    void Start()
    {
        startMenuRootTransition.gameObject.SetActive( false );
        autoHide.IsActiveAutoHide = false;
        // autoHide.gameObject.SetActive( false );
        playPauseButton.gameObject.SetActive( false );

        AppGameManager.Instance.PresentationStartEvent.AddListener( OnPresentationStarted );
        AppGameManager.Instance.PresentationEndEvent.AddListener( OnPresentationEnd );

        Init();
    }

    void Init()
    {
        if( video == null ) video = AppGameManager.Instance.AppVideoController.GetUniqueVideo( movie.FileName );
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

    void OnPresentationStarted( PresentationInteractItem item )
    {
        AppGameManager.Instance.AppVideoController.SetReadyUniqueVideo( movie.FileName, 2 );
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


