using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

[RequireComponent( typeof(VideoPlayer) )]
[RequireComponent( typeof(RawImage) )]
public class AppMoviePlayer : MonoBehaviour
{
    [SerializeField] string movieFolderPath = "Movie";
    [SerializeField] string streamingAssetsFileName = "";

    VideoPlayer video = null;
    RawImage raw = null;
    [SerializeField] Canvas canvas = null;

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


    void Start()
    {
        Video.source = VideoSource.Url;
        if( string.IsNullOrEmpty( movieFolderPath ) == true ) Video.url =  Application.streamingAssetsPath + "/" + streamingAssetsFileName;
        Video.url = Application.streamingAssetsPath + "/" + movieFolderPath + "/" + streamingAssetsFileName;

        Video.Prepare();
        Video.Play();

        StartCoroutine( Init() );
    }

    void Update()
    {
        // Raw.texture = Video.texture;   
        // UiUtility.SetAnchorPreset( UiUtility.Anchor.Middle_Center,  Raw.rectTransform );
        // Raw.rectTransform.position = Vector2.zero;
    }

    IEnumerator Init()
    {
        yield return new WaitUntil( () => Video.texture != null );

        Raw.texture = Video.texture;   
        UiUtility.SetAnchorPreset( UiUtility.Anchor.Middle_Center,  Raw.rectTransform );
        // Raw.transform.localPosition = Vector3.zero;
        var _curret = Raw.rectTransform.sizeDelta;
        var _rawRatio = Raw.rectTransform.rect.width / Raw.rectTransform.rect.height;
        var _videoRatio = (float)Video.texture.width / (float)Video.texture.height;

        if( _videoRatio > _rawRatio )
        {
            _curret.y = _curret.x * ( (float)Video.texture.height / (float)Video.texture.width );
        }
        else 
        {
            _curret.x = _curret.y * ( (float)Video.texture.width / (float)Video.texture.height );
        }

        Raw.rectTransform.sizeDelta = _curret;
    }
}
