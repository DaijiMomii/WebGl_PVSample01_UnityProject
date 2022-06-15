using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ---------------------------------------------------------------------------
/// <summary>
/// 外部リンクのゲート用ポップアップ.
/// </summary>
// ---------------------------------------------------------------------------
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
