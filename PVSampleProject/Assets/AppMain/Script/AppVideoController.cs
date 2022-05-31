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


    public List<AppVideoPlayer> FieldVideos{ get; set; } = new List<AppVideoPlayer>();
    public List<AppVideoPlayer> UniqueVideos{ get; set; } = new List<AppVideoPlayer>();
    public class VideoInitEvent : UnityEvent<AppVideoPlayer>{}
    public VideoInitEvent InitEvent{ get; set; } = new VideoInitEvent();

    bool isInit = true;


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
            var _video = _go.GetComponent<AppVideoPlayer>();

            _video.Video.source = VideoSource.Url;
            var _url = Application.streamingAssetsPath + "/Movie/" + _fileName;
            _video.Video.url = _url;
            _video.FileName = _fileName;
            _video.Url = _url;
            _video.Type = ( isField == true ) ? AppVideoPlayer.VideoType.Field : AppVideoPlayer.VideoType.Unique;

            if( isField == true ) FieldVideos.Add( _video );
            else UniqueVideos.Add( _video );

            _video.Video.Prepare();
            _video.PreparedCompleted.AddListener( OnPrepareCompleted );
        }
    }

    void OnPrepareCompleted( AppVideoPlayer fVideo )
    {        
        AppGameManager.Instance.AddLog( "@ Prepared Comp. " + fVideo.name ); 
        // Debug.Log( fVideo.Video.texture.width + "/" + fVideo.Video.texture.height );
        var _rt = new RenderTexture( fVideo.Video.texture.width, fVideo.Video.texture.width, 16, RenderTextureFormat.Default );
        fVideo.Video.targetTexture = _rt;
        fVideo.RenderTex = _rt;

        isInit = true;

        fVideo.Video.Play();
        if( fVideo.Type == AppVideoPlayer.VideoType.Unique )
        {
            fVideo.Video.Pause();
            fVideo.Video.frame = 2;
        }

        InitEvent?.Invoke( fVideo );
    }


    // FieldVideoPlayer video1 = null;
    public void OnHtmlInit()
    {
        Debug.Log( "@@@@@@... HTMLのInit" );      
        AppGameManager.Instance.AddLog( "@@@@@@@@" ); 

        foreach( var video in FieldVideos )
        {
            video.Video.SetDirectAudioMute( 0, true ); 
            // video.Video.SetDirectAudioVolume( 0, 0 );  
            // video.Video.Prepare();    
            // video.Video.Play();  
            // video.Video.Pause();
            // video.Video.frame = 2;
        }

        foreach( var video in UniqueVideos )
        {
            video.Video.SetDirectAudioMute( 0, false );  
            video.Video.SetDirectAudioVolume( 0, 1 );      
            if( video.Video.isPlaying == true && video.Type == AppVideoPlayer.VideoType.Unique )
            {
                video.Video.Pause();
                video.Video.frame = 2;
            }
            // video.Video.Play();  
            // video.Video.Pause();
            // video.Video.frame = 2;

            // video1 = video;
        }
    }

    // public void PPPP()
    // {
    //     if( video1 != null )
    //     {
    //         if( video1.Video.isPlaying == true ) video1.Video.Pause();
    //         else video1.Video.Play();
    //     }
    // }

    // public void SetVideo( string fileName, RawImage raw )
    // {
    //     var _video = GetVideo( fileName );
    //     if( _video != null )
    //     {
    //         _video.rawList.Add( raw );
    //     }
    // }


    public AppVideoPlayer GetFieldVideo( string fileName )
    {
        foreach( var param in FieldVideos )
        {
            if( fileName == param.FileName ) return param;
        }
        return null;
    }

    public AppVideoPlayer GetUniqueVideo( string fileName )
    {
        foreach( var param in UniqueVideos )
        {
            if( fileName == param.FileName ) return param;
        }
        return null;
    }

    public AppVideoPlayer PlayFieldVideo( string fileName )
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

    public AppVideoPlayer PlayUniqueVideo( string fileName )
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

    public AppVideoPlayer PauseFieldVideo( string fileName )
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

    public AppVideoPlayer PauseUniqueVideo( string fileName )
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

    
    public AppVideoPlayer StopFieldVideo( string fileName )
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

    public AppVideoPlayer StopUniqueVideo( string fileName )
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

    public AppVideoPlayer SetReadyFieldVideo( string fileName, int frame = 1 )
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

    public AppVideoPlayer SetReadyUniqueVideo( string fileName, int frame = 1 )
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
