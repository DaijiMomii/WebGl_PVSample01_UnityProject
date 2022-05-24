using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkGatePopup : PopupBase
{
    [SerializeField] Text infoTitle = null;

    protected override void Start()
    {
        base.Start();
    }

    public void Init( string title )
    {
        infoTitle.text = title;
    }

}
