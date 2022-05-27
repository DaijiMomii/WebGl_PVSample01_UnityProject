using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Presentation_CenterInformation : MonoBehaviour
{
    [SerializeField] UITransition startMenuRootTransition = null;
    [SerializeField] MovieMonitor movie = null;
    [SerializeField] UIAutoHide autoHide = null;

    [SerializeField] Image playButtonImage = null;

    [SerializeField] Sprite playButtonSprite = null;
    [SerializeField] Sprite pauseButtonSprite = null;


    FieldVideoPlayer video = null;

    void Start()
    {
        startMenuRootTransition.gameObject.SetActive( false );
        autoHide.IsActiveAutoHide = false;
        autoHide.gameObject.SetActive( false );

        AppGameManager.Instance.PresentationStartEvent.AddListener( OnPresentationStarted );
        AppGameManager.Instance.PresentationEndEvent.AddListener( OnPresentationEnd );

        Init();
    }

    void Init()
    {
        if( video == null ) video = AppGameManager.Instance.FieldVideoController.GetUniqueVideo( movie.FileName );
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
        autoHide.gameObject.SetActive( true );
        autoHide.IsActiveAutoHide = true;
        CloseStartMenu();
        playButtonImage.sprite = pauseButtonSprite;
    }

    void OnPresentationStarted( PresentationInteractItem item )
    {
        AppGameManager.Instance.FieldVideoController.SetReadyUniqueVideo( movie.FileName, 2 );
        OpenStartMenu();
    }

    public void OnPresentationEnd()
    {
        video.Pause();
    }

    public void OpenStartMenu()
    {
        startMenuRootTransition.TransitionIn();
    }    

    public void CloseStartMenu()
    {
        startMenuRootTransition.TransitionOut();
    }

    public void OnChangedPlayPause( bool isPlay )
    {
        var _sp = ( isPlay == true ) ? pauseButtonSprite : playButtonSprite;
        playButtonImage.sprite = _sp;

    }
}


