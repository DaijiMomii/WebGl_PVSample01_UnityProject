using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractItem_ShowImage : InteractableItemBase
{
    [SerializeField] UITransition imageTransition = null;

    void Start()
    {
        
    }


    public override void OnClick()
    {
        base.OnClick();
        Debug.Log( "画像表示" );

        imageTransition.TransitionIn();

        AppGameManager.Instance.CurrentLock.Move = true;
        AppGameManager.Instance.CurrentLock.Rotation = true;
        AppGameManager.Instance.CurrentLock.Click = true;
        AppGameManager.Instance.CurrentLock.Look = true;
    }

    public void OnCloseButtonClicked()
    {
        imageTransition.TransitionOut();

        AppGameManager.Instance.CurrentLock.Move = false;
        AppGameManager.Instance.CurrentLock.Rotation = false;
        AppGameManager.Instance.CurrentLock.Click = false;
        AppGameManager.Instance.CurrentLock.Look = false;
    }

}
