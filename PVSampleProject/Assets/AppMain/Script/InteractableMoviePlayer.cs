using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableMoviePlayer : AppMoviePlayer
{
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void Play( float time = -1f )
    {
        if( time > 0 )
        {
            Video.time = time;
        }

        Video.Play();
    }

    public void Pause()
    {
        Video.Pause();
    }

    public void  Stop()
    {
        Video.Stop();
    }

    public void SeekTime( float time )
    {
        Video.time = time;
    }

    public void SeekFrame( long frame )
    {
        Video.frame = frame;
    }

    
    public void OnOpened()
    {
        // Debug.Log( "On Opened" );
        StartCoroutine( base.Init() );
    }

    public void SetMute( bool isMute )
    {
        Video.SetDirectAudioMute( 0, isMute );
    }

}
