using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;

[RequireComponent( typeof(VideoPlayer) )]
[RequireComponent( typeof(RawImage) )]
public class AppMoviePlayer : MonoBehaviour
{
    [SerializeField] protected string movieFolderPath = "Movie";
    [SerializeField] protected string streamingAssetsFileName = "";

    [SerializeField] protected bool playOnAwake = true;

    [SerializeField] protected bool isMute = true;

    protected VideoPlayer video = null;
    protected RawImage raw = null;
    // [SerializeField] Canvas canvas = null;

    bool isInit = false;

    public VideoPlayer Video
    {
        get
        {
            if( video == null ) video = GetComponent<VideoPlayer>();
            return video;
        }
    }

    public RawImage Raw
    {
        get
        {
            if( raw == null ) raw = GetComponent<RawImage>();
            return raw;
        }
    }


    protected virtual void Start()
    {
        // Video.source = VideoSource.Url;
        // if( string.IsNullOrEmpty( movieFolderPath ) == true ) Video.url =  Application.streamingAssetsPath + "/" + streamingAssetsFileName;
        // Video.url = Application.streamingAssetsPath + "/" + movieFolderPath + "/" + streamingAssetsFileName;

        StartCoroutine( Init() );
        ChangeVideoTexture( gameObject, SetVideo );
    }

    protected virtual void Update()
    {
    }

    void SetVideo()
    {
        if( Video.texture == null ) return;


    }

    void SetUrl()
    {
        if( string.IsNullOrEmpty( Video.url ) == true )
        {
            Video.source = VideoSource.Url;
            if( string.IsNullOrEmpty( movieFolderPath ) == true ) Video.url =  Application.streamingAssetsPath + "/" + streamingAssetsFileName;
            Video.url = Application.streamingAssetsPath + "/" + movieFolderPath + "/" + streamingAssetsFileName;
        }
    }

    protected virtual IEnumerator Init()
    {
        AppGameManager.Instance.AddLog( "Init1" );
        SetUrl();
        Video.Prepare(); 
        yield return new WaitUntil( () => Video.texture != null );
        Raw.texture = Video.texture;   

        // if( isInit == false ) SetSize();
        SetSize();

        Video.Play();        
        if( playOnAwake == false )
        {
            Video.Pause();
            Video.frame = 2;
        }

        isInit = true;
        AppGameManager.Instance.AddLog( "Init2" );
    }


    public void HtmlInit()
    {
        Debug.Log( "HTMLのInit" );
        AppGameManager.Instance.AddLog( "HTMLの動画Init開始" );
      
        AppGameManager.Instance.AddLog( "IsInit = " + isInit );

        Video.SetDirectAudioMute( 0, false );  
        float _vol = ( isMute == true ) ? 0 : 1f;
        Video.SetDirectAudioVolume( 0, _vol );      
        Video.Play();  
        Video.Pause();
        Video.frame = 2;
    }
    // IEnumerator HtmlInitCor()
    // {
    //     SetUrl();
    //     Video.Prepare(); 

    //     yield return new WaitUntil( () => Video.texture != null );

    //     Raw.texture = Video.texture;   
    //     SetSize();
        
    //     Video.SetDirectAudioMute( 0, false );  
    //     float _vol = ( isMute == true ) ? 0 : 1f;
    //     Video.SetDirectAudioVolume( 0, _vol );      

    //     Video.Play();  
    //     Video.Pause();
    //     Video.frame = 2;

    //     AppGameManager.Instance.AddLog( "HTMLの動画Init完了" );
    // }

    void SetSize()
    {
        var _rawSize = new Vector2( Raw.rectTransform.rect.width, Raw.rectTransform.rect.height );
  
        UiUtility.SetAnchorPreset( UiUtility.Anchor.Middle_Center,  Raw.rectTransform );
        var _current = _rawSize;
        var _rawRatio = _rawSize.x / _rawSize.y;
        var _videoRatio = (float)Video.texture.width / (float)Video.texture.height;

        if( _videoRatio > _rawRatio )
        {
            _current.y = _current.x * ( (float)Video.texture.height / (float)Video.texture.width );
        }
        else 
        {
            _current.x = _current.y * ( (float)Video.texture.width / (float)Video.texture.height );
        }

        Raw.rectTransform.sizeDelta = _current;
    }

    void ChangeVideoTexture( GameObject go, UnityAction action )
    {
        this.ObserveEveryValueChanged( x => Video.texture )
        .Subscribe( x => action?.Invoke() )
        .AddTo( go );
    }
}
