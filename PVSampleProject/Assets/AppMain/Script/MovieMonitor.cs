using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Events;

public class MovieMonitor : MonoBehaviour
{
    [SerializeField] string fileName = "";
    [SerializeField] RawImage raw = null;

    [System.Serializable]
    public class VideoPlayEvent : UnityEvent<bool>{}
    public VideoPlayEvent OnPlayPause = new VideoPlayEvent();

    public string FileName{ get{ return fileName; } }

    bool isPlayingOnPresentationStarted = true;

    AppVideoPlayer video = null;

    void Start()
    {
        // AppGameManager.Instance.PresentationStartEvent.AddListener( OnPresentationStart );
        // AppGameManager.Instance.PresentationEndEvent.AddListener( OnPresentationEnd );

        AppGameManager.Instance.AppVideoController.InitEvent.AddListener( OnVideoInitCompleted );

    }

    void Update()
    {
    }

    void OnVideoInitCompleted( AppVideoPlayer videoPlayer )
    {
        if( fileName != videoPlayer.FileName ) return;

        if( video == null ) 
        {
            video = videoPlayer;//AppGameManager.Instance.FieldVideoController.GetVideo( fileName );
            video.AddRawImage( raw );
        }
        SetSize();
    }

    public void OnPresentationStart( PresentationInteractItem item )
    {                
    }

    public void OnPresentationEnd()
    {
    }

    // protected virtual IEnumerator Restart( bool isPlay )
    // {
    //     yield return new WaitUntil( () => Video.texture != null );
    //     Raw.texture = Video.texture;
    //     if( isPlay == true ) Video.Play();
    // }


    void SetSize()
    {
        var _rawSize = new Vector2( raw.rectTransform.rect.width, raw.rectTransform.rect.height );
  
        UiUtility.SetAnchorPreset( UiUtility.Anchor.Middle_Center,  raw.rectTransform );
        var _current = _rawSize;
        var _rawRatio = _rawSize.x / _rawSize.y;
        var _videoRatio = (float)video.Video.texture.width / (float)video.Video.texture.height;

        if( _videoRatio > _rawRatio )
        {
            _current.y = _current.x * ( (float)video.Video.texture.height / (float)video.Video.texture.width );
        }
        else 
        {
            _current.x = _current.y * ( (float)video.Video.texture.width / (float)video.Video.texture.height );
        }

        raw.rectTransform.sizeDelta = _current;
    }


    public void OnPlayButtonClicked()
    {
        if( video != null && video.Video != null )
        {
            if( video.Video.isPlaying == true )
            {
                video.Video.Pause();
                OnPlayPause?.Invoke( false );
            }
            else
            {
                video.Video.Play();
                OnPlayPause?.Invoke( true );
            }
        }
    }

    // public void OnStopButtonClicked()
    // {
    //     if( video != null && video.Video != null )
    //     {
    //         // video.Video.Stop();
    //     }
    // }

}
