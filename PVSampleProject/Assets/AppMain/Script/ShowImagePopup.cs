using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ---------------------------------------------------------------------------
/// <summary>
/// 画像情報表示のポップアップ.
/// </summary>
// ---------------------------------------------------------------------------
public class ShowImagePopup : PopupBase
{
    // テキスト.
    [SerializeField] Text infoText = null;
    // 画像表示用イメージ.
    [SerializeField] Image image = null;

    protected override void Start()
    {
        base.Start();
    }

    // ---------------------------------------------------------------------------
    /// <summary>
    /// 初期設定.
    /// </summary>
    /// <param name="info"> 情報. </param>
    /// <param name="sprite"> 画像. </param>
    // ---------------------------------------------------------------------------
    public void Init( string info, Sprite sprite )
    {
        if( sprite != null )
        {
            image.sprite = sprite;
        }

        infoText.text = info;
    }

}
