using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Events;
using UniRx;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

public class AppSoundController : MonoBehaviour
{
    [System.Serializable]
    public class VideoResourceParam
    {
        public string FileName = ".mp4";
        public bool IsMute = true;
        public bool IsPlayOnAwake = false;
    }

    [System.Serializable]
    public class VideoPlayerParam
    {
        public VideoPlayer Video = null;
        public bool IsMute = true;
        public bool IsPlayOnAwake = false;
    }

    [System.Serializable]
    public class AudioSourceParam
    {
        // public string FileName = "";
        public AudioSource Audio = null;
        public bool IsPlayOnAwake = false;

        public bool IsInit{ get; set; } = false;
    }

    [System.Serializable]
    public class AudioClipParam
    {
        public string FileName = "";
        public AudioClip Clip = null;
    }

    [SerializeField] List<VideoResourceParam> videoDistributeResourceParams = new List<VideoResourceParam>();
    [SerializeField] List<VideoPlayerParam> videoPlayers = new List<VideoPlayerParam>();
    [SerializeField] List<string> audioResourceNames = new List<string>();
    [SerializeField] List<AudioSourceParam> audioSources = new List<AudioSourceParam>();
    List<AudioClipParam> audioClips = new List<AudioClipParam>();

    [SerializeField] GameObject fieldVideoPlayerPrefab = null;

    public List<PvDistributedVideoPlayer> Videos{ get; set; } = new List<PvDistributedVideoPlayer>();
    public class VideoInitEvent : UnityEvent<PvDistributedVideoPlayer>{}
    public VideoInitEvent InitEvent{ get; set; } = new VideoInitEvent();

    public bool IsMute{ get; private set; } = true;

    bool isInit = true;

    // bool isMuteInitialize = false;


    void Start()
    {
        Init();
    }

    void Update()
    {
        
    }

    async void Init()
    {
        // ビデオリソースからRawImage投射用のビデオプレイヤーを作成.
        foreach( var param in videoDistributeResourceParams )
        {
            _CreateVideo( param.FileName, param.IsMute, param.IsPlayOnAwake );
        }

        // 個別のビデオプレイヤーリストを一旦全ミュート.
        foreach( var video in videoPlayers )
        {
            video.Video.playOnAwake = false;
            video.Video.SetDirectAudioMute( 0, true );
        }

        // オーディオリソース名からオーディオクリップを読み込んで保管.
        foreach( var fileName in audioResourceNames )
        {
            if( string.IsNullOrEmpty( fileName ) == false )
            {
                var _path = Application.streamingAssetsPath + "/Audio/" + fileName;

                var _req = UnityWebRequestMultimedia.GetAudioClip( _path, AudioType.MPEG );
                await _req.SendWebRequest() ;
                var _clip = DownloadHandlerAudioClip.GetContent( _req );

                var _param = new AudioClipParam();
                _param.FileName = fileName;
                _param.Clip = _clip;
                audioClips.Add( _param );
            }
        }

        // 個別のオーディオプレイヤーリストを一旦全ミュート.
        foreach( var audio in audioSources )
        {
            audio.Audio.playOnAwake = false;
            audio.Audio.mute = true;
        }


        void _CreateVideo( string _fileName, bool isMute, bool isPlayOnAwake )
        {
            var _go = Instantiate( fieldVideoPlayerPrefab, this.transform.position, this.transform.rotation, this.transform );
            var _video = _go.GetComponent<PvDistributedVideoPlayer>();

            _video.Video.source = VideoSource.Url;
            var _url = Application.streamingAssetsPath + "/Movie/" + _fileName;
            _video.Video.url = _url;
            _video.FileName = _fileName;
            _video.Url = _url;
            // _video.UniqueNum = Random.Range( 0, 99999 );

            _video.IsMuteVideo = isMute;
            _video.IsPlayOnAwake = isPlayOnAwake;

            Videos.Add( _video );

            _video.Video.Prepare();
            _video.PreparedCompleted.AddListener( OnPrepareCompleted );
        }
    }

    void OnPrepareCompleted( PvDistributedVideoPlayer video )
    {        
        var _rt = new RenderTexture( video.Video.texture.width, video.Video.texture.width, 16, RenderTextureFormat.Default );
        video.Video.targetTexture = _rt;
        video.RenderTex = _rt;

        isInit = true;

        video.Video.Play();
        if( video.IsPlayOnAwake == false )
        {
            video.Video.Pause();
            video.Video.frame = 2;
        }

        InitEvent?.Invoke( video );
    }


    public void OnHtmlInit( bool isInitMute = false )
    {
        if( isInit == true ) return;

        foreach( var video in Videos )
        {
            if( video.IsMuteVideo == true ) video.Video.SetDirectAudioMute( 0, true );  
            else video.Video.SetDirectAudioMute( 0, false );  

            float _value = ( isInitMute == true ) ? 0 : 1f;

            video.Video.SetDirectAudioVolume( 0, _value );  
        }

        foreach( var video in videoPlayers )
        {
            if( video.IsMute == true ) video.Video.SetDirectAudioMute( 0, true );  
            else video.Video.SetDirectAudioMute( 0, false );  

            float _value = ( isInitMute == true ) ? 0 : 1f;

            video.Video.SetDirectAudioVolume( 0, _value );  
        }

        foreach( var audio in audioSources )
        {
            float _value = ( isInitMute == true ) ? 0 : 1f;
            audio.Audio.volume = _value;            
            audio.Audio.mute = false;

            if( audio.IsPlayOnAwake == true && audio.IsInit == false ) 
            {
                audio.Audio.Play();
                audio.IsInit = true;
            }
        }

        IsMute = isInitMute;
    }

    public void OnHtmlMuteOn()
    {
        var _os = AppGameManager.Instance.Platform.OS;
        foreach( var video in Videos )
        {
            if( video.IsMuteVideo == true ) video.Video.SetDirectAudioMute( 0, true );
            else
            {
                if( _os == AppGameManager.OS.iOS ) video.Video.SetDirectAudioMute( 0, true );
                else video.Video.SetDirectAudioMute( 0, false );

                // video.Video.SetDirectAudioMute( 0, true );
            }

            video.Video.SetDirectAudioVolume( 0, 0 );  
        }

        foreach( var video in videoPlayers )
        {
            if( video.IsMute == true ) video.Video.SetDirectAudioMute( 0, true );
            else
            {
                if( _os == AppGameManager.OS.iOS ) video.Video.SetDirectAudioMute( 0, true );
                else video.Video.SetDirectAudioMute( 0, false );

                // video.Video.SetDirectAudioMute( 0, true );
            }

            video.Video.SetDirectAudioVolume( 0, 0 );  
        }

        foreach( var audio in audioSources )
        {
            if( audio.Audio.mute == true ) audio.Audio.mute = false;
            audio.Audio.volume = 0;
        }


        IsMute = true;
    }

    public void SetVolume( float value )
    {
        var _os = AppGameManager.Instance.Platform.OS;

        if( value > 1 ) value = 1;
        else if( value < 0 ) value = 0;

        foreach( var video in Videos )
        {
            if( video.IsMuteVideo == true ) video.Video.SetDirectAudioMute( 0, true );
            else video.Video.SetDirectAudioMute( 0, false );

            video.Video.SetDirectAudioVolume( 0, value );  
        }

        foreach( var video in videoPlayers )
        {
            if( video.IsMute == true ) video.Video.SetDirectAudioMute( 0, true );
            else video.Video.SetDirectAudioMute( 0, false );
          
            video.Video.SetDirectAudioVolume( 0, value );  
        }

        foreach( var audio in audioSources )
        {
            if( audio.Audio.mute == true ) audio.Audio.mute = false;
            audio.Audio.volume = value;
        }
    }

    public void OnHtmlMuteOff()
    {
        foreach( var video in Videos )
        {
            if( video.IsMuteVideo == true ) video.Video.SetDirectAudioMute( 0, true );
            else video.Video.SetDirectAudioMute( 0, false );  

            video.Video.SetDirectAudioVolume( 0, 1 );
        }

        foreach( var video in videoPlayers )
        {
            if( video.IsMute == true ) video.Video.SetDirectAudioMute( 0, true );
            else video.Video.SetDirectAudioMute( 0, false );

            video.Video.SetDirectAudioVolume( 0, 1 );  
        }

        foreach( var audio in audioSources )
        {
            if( audio.Audio.mute == true ) audio.Audio.mute = false;
            audio.Audio.volume = 1;
        }

        IsMute = false;
    }

    public void AddAudioSource( AudioSource audio, bool playOnAwake )
    {
        var param = new AudioSourceParam();
        param.Audio = audio;
        param.IsPlayOnAwake = playOnAwake;
        param.IsInit = true;

        audioSources.Add( param );

        if( playOnAwake == true )
        {
            audio.mute = IsMute;
            audio.volume = ( IsMute == true ) ? 0 : 1;
            audio.Play();
        }
    } 

    public void AddVideoPlayer( VideoPlayer video, bool playOnAwake, bool mute )
    {
        var _param = new VideoPlayerParam();
        _param.Video = video;
        _param.IsPlayOnAwake = playOnAwake;
        _param.IsMute = mute;
        videoPlayers.Add( _param );

        // 追加時にMuteじゃなくても次のユーザーアクションまではMuteじゃないとエラーになるかもなので.
        if( mute == true ) video.SetDirectAudioMute( 0, true );  
        else if ( IsMute == true ) video.SetDirectAudioMute( 0, true );  
        // else video.SetDirectAudioMute( 0, false );  
        video.SetDirectAudioVolume( 0, 1 );  

        video.Play();
        if( playOnAwake == false )
        {
            video.Pause();
            video.frame = 2;
        }
    }

    public void AddAudioSourceParam( AudioSource audio, bool isPlayOnAwake )
    {
        var _param = new AudioSourceParam();
        _param.Audio = audio;
        _param.IsPlayOnAwake = isPlayOnAwake;

        audioSources.Add( _param );
    }

    public AudioClip GetAudioClip( string fileName )
    {
        foreach( var param in audioClips )
        {
            if( param.FileName == fileName ) return param.Clip;
        }
        return null;
    }

    public PvDistributedVideoPlayer GetVideo( string fileName )
    {
        foreach( var param in Videos )
        {
            if( fileName == param.FileName ) return param;
        }
        return null;
    }

    // public AudioSourceParam GetAudioSource( string fileName )
    // {
    //     foreach( var audio in audioSources )
    //     {
    //         if( audio.FileName == fileName ) return audio;
    //     }
    //     return null;
    // }  

    public PvDistributedVideoPlayer PlayVideo( string fileName )
    {
        foreach( var video in Videos )
        {
            if( video.FileName == fileName )
            {
                video.Video.Play();
                return video;
            }
        }
        return null;
    }

    public PvDistributedVideoPlayer PauseVideo( string fileName )
    {
        foreach( var video in Videos )
        {
            if( video.FileName == fileName )
            {
                video.Video.Pause();
                return video;
            }
        }
        return null;
    }   

    public PvDistributedVideoPlayer StopVideo( string fileName )
    {
        foreach( var video in Videos )
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


    public PvDistributedVideoPlayer SetReadyVideo( string fileName, int frame = 1 )
    {
        var _video = PlayVideo( fileName );
        if( _video == null ) 
        {
            Debug.LogWarning( fileName + "というファイルが見つかりません." );
            return null;
        }
        _video.Video.Pause();
        _video.Video.frame = frame;

        return _video;
    }

    bool isTest = false;
    public void TestVolumeSetting()
    {
        
        var _value = ( isTest == true ) ? 1 : 0.3f;
        foreach( var video in Videos )
        {
            if( video.IsMuteVideo == true ) video.Video.SetDirectAudioMute( 0, true );
            else video.Video.SetDirectAudioMute( 0, false );

            video.Video.SetDirectAudioVolume( 0, _value );  
        }

        foreach( var video in videoPlayers )
        {
            if( video.IsMute == true ) video.Video.SetDirectAudioMute( 0, true );
            else video.Video.SetDirectAudioMute( 0, false );

            video.Video.SetDirectAudioVolume( 0, _value );  
        }

        foreach( var audio in audioSources )
        {
            if( audio.Audio.mute == true ) audio.Audio.mute = false;
            audio.Audio.volume = _value;
        }

        isTest = !isTest;
    }
    
}
