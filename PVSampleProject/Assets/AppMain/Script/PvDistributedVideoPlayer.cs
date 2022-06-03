using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Events;
using UniRx;
using Cysharp.Threading.Tasks;

public class PvDistributedVideoPlayer : MonoBehaviour
{
    public enum VideoType
    {
        Field, Unique,
    }

    // public int UniqueNum = 0;
    public string FileName = "";
    public string Url = "";
    public RenderTexture RenderTex = null;
    public VideoPlayer Video
    {
        get
        {
            if( video == null ) video = GetComponent<VideoPlayer>();
            return video;
        }
    }

    // public VideoType Type = VideoType.Field;

    public bool IsMuteVideo = true;
    public bool IsPlayOnAwake = false;





    public class PreparedCompletedEvent : UnityEvent<PvDistributedVideoPlayer>{}
    public PreparedCompletedEvent PreparedCompleted = new PreparedCompletedEvent();

    public List<RawImage> rawList = new List<RawImage>();

    VideoPlayer video = null;

    void Start()
    {
        if( video == null ) video = GetComponent<VideoPlayer>();
        video.prepareCompleted += OnPrepareCompleted;
    }

    void OnPrepareCompleted( VideoPlayer video )
    {
        PreparedCompleted?.Invoke( this );        

        foreach( var raw in rawList )
        {
            if( raw.texture != video.texture ) raw.texture = video.texture;
        }
    }

    void OnDestroy()
    {
        Destroy( RenderTex );
        video.prepareCompleted -= OnPrepareCompleted;
    }

    public void AddRawImage( RawImage raw )
    {
        rawList.Add( raw );
        raw.texture = video.texture;
    }

    
    public void Play( float seekTime = -1f )
    {
        if( seekTime >= 0f )
        {
            Video.time = seekTime;
        }
        Video.Play();
    }

    public void Play( int seekFrame )
    {
        if( seekFrame >= 0 )
        {
            Video.frame = seekFrame;
        }
        Video.Play();
    }

    public void Pause( float seekTime = -1f )
    {
        if( seekTime >= 0f )
        {
            Video.time = seekTime;
        }
        Video.Pause();
    }

    public void Pause( int seekFrame )
    {
        if( seekFrame >= 0 )
        {
            Video.frame = seekFrame;
        }
        Video.Pause();
    }

    public void Stop()
    {
        Video.Stop();
    }

}
