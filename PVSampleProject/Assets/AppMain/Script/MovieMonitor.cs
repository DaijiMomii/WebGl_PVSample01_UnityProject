using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Events;

// ---------------------------------------------------------------------------
/// <summary>
/// RawImageに動画表示.
/// </summary>
// ---------------------------------------------------------------------------
public class MovieMonitor : MonoBehaviour
{
    // 動画ファイル名.
    [SerializeField] string fileName = "";
    // RawImage.
    [SerializeField] RawImage raw = null;

    // ビデオイベント定義クラス.
    [System.Serializable]
    public class VideoPlayEvent : UnityEvent<bool>{}
    // 再生一時停止時イベント.
    public VideoPlayEvent OnPlayPause = new VideoPlayEvent();

    // ファイル名.
    public string FileName{ get{ return fileName; } }
    
    // ビデオプレイヤー.
    PvDistributedVideoPlayer video = null;

    void Start()
    {
        AppGameManager.Instance.AppSoundController.InitEvent.AddListener( OnVideoInitCompleted );
    }

    void Update()
    {
    }


    void OnVideoInitCompleted( PvDistributedVideoPlayer videoPlayer )
    {
        if( fileName != videoPlayer.FileName ) return;

        if( video == null ) 
        {
            video = videoPlayer;//AppGameManager.Instance.FieldVideoController.GetVideo( fileName );
            video.AddRawImage( raw );
        }
        SetSize( true );
    }

    public void OnPresentationStart( InteractItem_Presentation item )
    {                
    }

    public void OnPresentationEnd()
    {
    }

    // ---------------------------------------------------------------------------
    /// <summary>
    /// 動画のサイズ設定.
    /// </summary>
    /// <param name="isInit"></param>
    // ---------------------------------------------------------------------------
    public void SetSize( bool isInit )
    {
        if( video == null || video.Video == null || video.Video.texture == null ) return;

        if( isInit == false )
        {            
            UiUtility.SetAnchorPreset( UiUtility.Anchor.AllStretch,  raw.rectTransform );
            UiUtility.SetRectTransformStretch( raw.rectTransform, 10f, 10f, 10f, 10f );
        }

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

    // ---------------------------------------------------------------------------
    /// <summary>
    /// 再生ボタンクリックコールバック.
    /// </summary>
    // ---------------------------------------------------------------------------
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
