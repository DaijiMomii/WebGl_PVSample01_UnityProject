using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Events;
using UniRx;
using Cysharp.Threading.Tasks;

public class AppVideoController : MonoBehaviour
{
    public enum VideoResourceType
    {

    }

    [SerializeField] List<string> fieldVideoStreamingAssetsFileNames = new List<string>();
    [SerializeField] List<string> uniqueVideoStreamingAssetsFildName = new List<string>();

    [SerializeField] GameObject fieldVideoPlayerPrefab = null;

    [SerializeField] List<RawImage> testRaws = new List<RawImage>();


    public List<PvVideoPlayer> FieldVideos{ get; set; } = new List<PvVideoPlayer>();
    public List<PvVideoPlayer> UniqueVideos{ get; set; } = new List<PvVideoPlayer>();
    public class VideoInitEvent : UnityEvent<PvVideoPlayer>{}
    public VideoInitEvent InitEvent{ get; set; } = new VideoInitEvent();

    bool isInit = true;

    bool isMuteInitialize = false;


    void Start()
    {
        Init();
    }

    void Update()
    {
        
    }

    void Init()
    {
        foreach( var file in fieldVideoStreamingAssetsFileNames )
        {
            // var _go = Instantiate( fieldVideoPlayerPrefab, this.transform.position, this.transform.rotation, this.transform );
            // var _video = _go.GetComponent<FieldVideoPlayer>();

            // _video.Video.source = VideoSource.Url;
            // var _url = Application.streamingAssetsPath + "/Movie/" + file;
            // _video.Video.url = _url;
            // _video.FileName = file;
            // _video.Url = _url;
            // Videos.Add( _video );

            // _video.Video.Prepare();
            // _video.PreparedCompleted.AddListener( OnPrepareCompleted );

            _Create( file, true );
        }

        foreach( var file in uniqueVideoStreamingAssetsFildName )
        {
            _Create( file, false );
        }


        void _Create( string _fileName, bool isField )
        {
            var _go = Instantiate( fieldVideoPlayerPrefab, this.transform.position, this.transform.rotation, this.transform );
            var _video = _go.GetComponent<PvVideoPlayer>();

            _video.Video.source = VideoSource.Url;
            var _url = Application.streamingAssetsPath + "/Movie/" + _fileName;
            _video.Video.url = _url;
            _video.FileName = _fileName;
            _video.Url = _url;
            _video.Type = ( isField == true ) ? PvVideoPlayer.VideoType.Field : PvVideoPlayer.VideoType.Unique;

            if( isField == true ) FieldVideos.Add( _video );
            else UniqueVideos.Add( _video );

            _video.Video.Prepare();
            _video.PreparedCompleted.AddListener( OnPrepareCompleted );
        }
    }

    void OnPrepareCompleted( PvVideoPlayer fVideo )
    {        
        // AppGameManager.Instance.AddLog( "@ Prepared Comp. " + fVideo.name ); 
        // Debug.Log( fVideo.Video.texture.width + "/" + fVideo.Video.texture.height );
        var _rt = new RenderTexture( fVideo.Video.texture.width, fVideo.Video.texture.width, 16, RenderTextureFormat.Default );
        fVideo.Video.targetTexture = _rt;
        fVideo.RenderTex = _rt;

        isInit = true;

        fVideo.Video.Play();
        if( fVideo.Type == PvVideoPlayer.VideoType.Unique )
        {
            fVideo.Video.Pause();
            fVideo.Video.frame = 2;
        }

        InitEvent?.Invoke( fVideo );
    }


    // FieldVideoPlayer video1 = null;
    public void OnHtmlInit( bool isMute = false )
    {
        if( isMuteInitialize == true ) return;

        Debug.Log( "@@@@@@... HTMLのInit" );      

        foreach( var video in FieldVideos )
        {
            video.Video.SetDirectAudioMute( 0, true ); 
        }

        foreach( var video in UniqueVideos )
        {
            video.Video.SetDirectAudioMute( 0, false );  
            float _value = ( isMute == true ) ? 0 : 1f;
            video.Video.SetDirectAudioVolume( 0, _value );      
            if( video.Video.isPlaying == true && video.Type == PvVideoPlayer.VideoType.Unique )
            {
                video.Video.Pause();
                video.Video.frame = 2;
            }
        }

        isMuteInitialize = true;
    }

    public void OnHtmlMuteOn()
    {
        foreach( var video in FieldVideos )
        {
            video.Video.SetDirectAudioMute( 0, true ); 
        }

        foreach( var video in UniqueVideos )
        {
            video.Video.SetDirectAudioMute( 0, false );
            video.Video.SetDirectAudioVolume( 0, 0 );      
            if( video.Video.isPlaying == true && video.Type == PvVideoPlayer.VideoType.Unique )
            {
                video.Video.Pause();
                video.Video.frame = 2;
            }
        }
    }

    public void OnHtmlMuteOff()
    {
        foreach( var video in FieldVideos )
        {
            video.Video.SetDirectAudioMute( 0, true ); 
        }

        foreach( var video in UniqueVideos )
        {
            video.Video.SetDirectAudioMute( 0, false );  
            video.Video.SetDirectAudioVolume( 0, 1 );      
            if( video.Video.isPlaying == true && video.Type == PvVideoPlayer.VideoType.Unique )
            {
                video.Video.Pause();
                video.Video.frame = 2;
            }
        }
    }

    public PvVideoPlayer GetFieldVideo( string fileName )
    {
        foreach( var param in FieldVideos )
        {
            if( fileName == param.FileName ) return param;
        }
        return null;
    }

    public PvVideoPlayer GetUniqueVideo( string fileName )
    {
        foreach( var param in UniqueVideos )
        {
            if( fileName == param.FileName ) return param;
        }
        return null;
    }

    public PvVideoPlayer PlayFieldVideo( string fileName )
    {
        foreach( var video in FieldVideos )
        {
            if( video.FileName == fileName )
            {
                video.Video.Play();
                return video;
            }
        }
        return null;
    }

    public PvVideoPlayer PlayUniqueVideo( string fileName )
    {
        foreach( var video in UniqueVideos )
        {
            if( video.FileName == fileName )
            {
                video.Video.Play();
                return video;
            }
        }
        return null;
    }

    public PvVideoPlayer PauseFieldVideo( string fileName )
    {
        foreach( var video in FieldVideos )
        {
            if( video.FileName == fileName )
            {
                video.Video.Pause();
                return video;
            }
        }
        return null;
    }

    public PvVideoPlayer PauseUniqueVideo( string fileName )
    {
        foreach( var video in UniqueVideos )
        {
            if( video.FileName == fileName )
            {
                video.Video.Pause();
                return video;
            }
        }
        return null;
    }

    
    public PvVideoPlayer StopFieldVideo( string fileName )
    {
        foreach( var video in FieldVideos )
        {
            if( video.FileName == fileName )
            {
                video.Video.Stop();
                return video;
            }
        }
        return null;
    }

    public PvVideoPlayer StopUniqueVideo( string fileName )
    {
        foreach( var video in UniqueVideos )
        {
            if( video.FileName == fileName )
            {
                // <<Tips>>
                // WebGlのIOSでは一旦停止してしまうと再度ユーザーアクションがないと音を出せなくになってしまうので、
                // StopするときはそれでOKな時のみにする.                
                video.Video.Stop();
                return video;
            }
        }
        return null;
    }

    public PvVideoPlayer SetReadyFieldVideo( string fileName, int frame = 1 )
    {
        var _video = PlayFieldVideo( fileName );
        if( _video == null ) 
        {
            Debug.LogWarning( fileName + "というファイルが見つかりません." );
            return null;
        }
        _video.Video.Pause();
        _video.Video.frame = frame;

        return _video;
    }

    public PvVideoPlayer SetReadyUniqueVideo( string fileName, int frame = 1 )
    {
        var _video = PlayUniqueVideo( fileName );
        if( _video == null ) 
        {
            Debug.LogWarning( fileName + "というファイルが見つかりません." );
            return null;
        }
        _video.Video.Pause();
        _video.Video.frame = frame;

        return _video;
    }
    
}
