using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiledMoviePlayer : AppMoviePlayer
{
    bool isPlayingOnPresentationStarted = true;

    protected override void Start()
    {
        base.Start();
        AppGameManager.Instance.PresentationStartEvent.AddListener( OnPresentationStart );
        AppGameManager.Instance.PresentationEndEvent.AddListener( OnPresentationEnd );
    }

    protected override void Update()
    {
        base.Update();

    }

    public void OnPresentationStart( InteractableItemBase interactableItem )
    {
        if( Video.isPlaying == true )
        {
            Video.Stop();
            isPlayingOnPresentationStarted = true;
        }
        else
        {
            isPlayingOnPresentationStarted = false;
        }

        Video.gameObject.SetActive( false );
        // Debug.Log( isPlayingOnPresentationStarted + "@@@@" );
    }

    public void OnPresentationEnd()
    {
        Video.gameObject.SetActive( true );

        Video.Prepare();
        StartCoroutine( Restart( isPlayingOnPresentationStarted ) );
    }

    protected virtual IEnumerator Restart( bool isPlay )
    {
        yield return new WaitUntil( () => Video.texture != null );
        Raw.texture = Video.texture;
        if( isPlay == true ) Video.Play();
    }
}
