using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractItem_Event : InteractableItemBase
{
    public UnityEvent OcClickedEvent = new UnityEvent();

    void Start()
    {
        
    }


    public override void OnClick()
    {
        base.OnClick();
        
        OcClickedEvent?.Invoke();
    }

}
