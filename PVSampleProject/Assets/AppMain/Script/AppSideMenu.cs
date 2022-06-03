using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppSideMenu : MonoBehaviour
{
    [SerializeField] UITransition bgTransition = null;
    [SerializeField] UITransition menuTransition = null;

    public bool IsOpen{ get; private set; } = false;

    void Start()
    {
        bgTransition.gameObject.SetActive( false );
        menuTransition.gameObject.SetActive( false );
        IsOpen = false;
    }


    public void Open()
    {
        // bgTransition.gameObject.SetActive( true );
        // menuTransition.gameObject.SetActive( true );
        bgTransition.TransitionIn( () => { IsOpen = true; });
        menuTransition.TransitionIn();        
    }

    public void Close()
    {
        bgTransition.TransitionOut( () => { IsOpen = false; });
        menuTransition.TransitionOut();
    }

    public void OnCloseButtonClicked()
    {
        Close();
    }
}
