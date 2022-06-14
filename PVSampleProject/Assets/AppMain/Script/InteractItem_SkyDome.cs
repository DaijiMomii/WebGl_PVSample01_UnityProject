using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Video;

public class InteractItem_SkyDome : InteractableItemBase
{
    [SerializeField] Transform sphere = null;
    [SerializeField] Button closeButton = null;

    [SerializeField] Ease transitionEase = Ease.Linear;
    [SerializeField] float transitionTime = 1.5f;
    [SerializeField] int skyDomeLayerNumber = 9;

    [SerializeField] AudioSource audioSource = null;
    // [SerializeField] bool isAudioPlayOnAwake = true;
    [SerializeField] VideoPlayer video = null;
    [SerializeField] string videoFileName = "mp4";



    int sphereDefaultLayerNumber = 0;
    Vector3 sphereDefaultScale = Vector3.one;
    Sequence seq = null;

    void Start()
    {
        sphereDefaultScale = sphere.localScale;
        sphereDefaultLayerNumber = sphere.gameObject.layer;
        closeButton.gameObject.SetActive( false );

        AudioInit();
        VideoInit();
    }

    void AudioInit()
    {
        if( audioSource == null ) return;
        
        if( audioSource.isPlaying == true ) audioSource.Stop();
        audioSource.mute = true;
        AppGameManager.Instance.SoundController.AddAudioSource( audioSource, true );
    }

    void VideoInit()
    {
        if( video == null ) return;

        var _path = Application.streamingAssetsPath + "/Movie/" + videoFileName;
        video.source = VideoSource.Url;
        video.url = _path;
        video.SetDirectAudioMute( 0, true );
        AppGameManager.Instance.SoundController.AddVideoPlayer( video, true, true );
    }

    public override void OnClick()
    {
        base.OnClick();
        Debug.Log( "SkyDome表示" );
        AppGameManager.Instance.SkyDomeControl.StartSkyDomeEvent?.Invoke( true );

        AppGameManager.Instance.SetMoveUI( false );
        
        AppGameManager.Instance.CurrentLock.Move = true;
        AppGameManager.Instance.CurrentLock.Click = true;
        AppGameManager.Instance.CurrentLock.Look = true;

        seq = DOTween.Sequence();

        seq.Append
        (
            sphere.DOMove( Camera.main.transform.position, transitionTime )
        );

        seq.Join
        (
            sphere.DOScale( new Vector3( 10f, 10f, 10f ), transitionTime )
        );

        seq
        .OnComplete( () => 
        {            
            AppGameManager.Instance.SkyDomeControl.StartSkyDomeEvent?.Invoke( false );
            sphere.gameObject.layer = skyDomeLayerNumber;
            closeButton.gameObject.SetActive( true );
        } )
        .SetLink( gameObject )
        .SetEase( transitionEase );


    }

    public void OnCloseButtonClicked()
    {
        closeButton.gameObject.SetActive( false );
        AppGameManager.Instance.SetMoveUI( true );
        sphere.gameObject.layer = sphereDefaultLayerNumber;
        
        AppGameManager.Instance.SkyDomeControl.FinishSkyDomeEvent?.Invoke( true );

        seq = DOTween.Sequence();

        seq.Append
        (
            sphere.DOLocalMove( Vector3.zero, transitionTime )
        );

        seq.Join
        (
            sphere.DOScale( sphereDefaultScale, transitionTime )
        );

        seq
        .OnComplete( () => 
        {            
            AppGameManager.Instance.SkyDomeControl.FinishSkyDomeEvent?.Invoke( false );
            
            AppGameManager.Instance.CurrentLock.Move = false;
            AppGameManager.Instance.CurrentLock.Click = false;
            AppGameManager.Instance.CurrentLock.Look = false;
        } )
        .SetLink( gameObject )
        .SetEase( transitionEase );

    }
}
