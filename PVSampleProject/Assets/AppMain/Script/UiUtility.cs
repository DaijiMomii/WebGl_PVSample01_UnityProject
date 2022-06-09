using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UniRx;
// using Cysharp.Threading.Tasks;
// using TMPro;

// ------------------------------------------------------------------------------------
/// <summary>
/// UI関連のユーティリティクラス.
/// </summary>
// ------------------------------------------------------------------------------------
public class UiUtility : MonoBehaviour
{
#region Definition.
    // ---------------------------------------------------------------------
    /// <summary>
    /// アンカー位置.
    /// </summary>
    // ---------------------------------------------------------------------
    public enum Anchor
    {
        Top_Left,       Top_Center,     Top_Right,      TopStretch,
        Middle_Left,    Middle_Center,  Middle_Right,   MiddleStretch,
        Bottom_Left,    Bottom_Center,  Bottom_Right,   BottomStretch,
        LeftStretch,    CenterStretch,  RightStretch,   AllStretch,
    }

    public enum PivotType
    {
        Top_Left,       Top_Center,     Top_Right, 
        Middle_Left,    Middle_Center,  Middle_Right,
        Bottom_Left,    Bottom_Center,  Bottom_Right,
    }

#endregion

    // static CancellationTokenSourceEx cts = new CancellationTokenSourceEx();


    void Start()
    {
        // cts.Init( gameObject );
    }


#region Public Method.
    // -----------------------------------------------------------------------------------
    /// <summary>
    /// スマホセーフエリアに対応してサイズを調整.
    /// </summary>
    /// <param name="rect"> 調整するレクトトランスフォーム. </param>
    /// <param name="isOnlyDevice"> スマホデバイス実行時のみ適用するフラグ. </param>
    // -----------------------------------------------------------------------------------
    // public static async UniTask SetSafeAreaAnchor( RectTransform rect, bool isOnlyDevice = true )
    // {
    //     if( rect == null )
    //     {
    //         return;
    //     }

    //     if( isOnlyDevice == true )
    //     {
    //         if( Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer )
    //         {
    //             Debug.Log( "safeAreaへの対応はスマホ実行時のみ行われます。" );
    //             return;
    //         }
    //     }

    //     Debug.Log( "<color=blue>Platform = " + Application.platform + " safeAreaの設定により画面を調整します</color>" );
        


    //     // 回転が終わるまで待つ.
    //     // Input.deviceOrientationは端末の現在の向き、Screen.orientationは端末が横を向いていたとしても縦固定の画面にしていた場合縦になる画面の向き.
    //     // if( Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown )
    //     if( Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown )
    //     {
    //         await cts.WaitUntil( () => Screen.width < Screen.height );
    //     }
    //     else if( Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight )
    //     {
    //         await cts.WaitUntil( () => Screen.width > Screen.height );
    //     }
        


    //     var area = Screen.safeArea;
    //     var resolution = Screen.currentResolution;

    //     // Debug.Log( "Orientation : " + Input.deviceOrientation + " === " + area.xMax + ", " + area.xMin + "/" + area.yMax + "," + area.yMin + " // " + resolution.width + "," + resolution.height );
    //     // Debug.Log( Screen.safeArea.x + " . " + Screen.safeArea.y );

    //     // ビューのアンカーをセーフエリアに合わせて調整.
    //     rect.sizeDelta = Vector2.zero;
    //     rect.anchorMax = new Vector2(area.xMax / resolution.width, area.yMax / resolution.height);
    //     rect.anchorMin = new Vector2(area.xMin / resolution.width, area.yMin / resolution.height);

    //     SetRectTransformStretch( rect : rect, top : 0f, bottom : 0f, left : 0f, right : 0f );
    // }


    // --------------------------------------------------------------------------------
    /// <summary>
    /// アンカーの設定.
    /// </summary>
    /// <param name="anchor"> アンカー位置. </param>
    /// <param name="rect"> 設定するレクトトランスフォーム. </param>
    // ---------------------------------------------------------------------------------
    public static void SetAnchorPreset( Anchor anchor, RectTransform rect, bool isSetZero = false )
    {
        switch( anchor )
        {
            case Anchor.Top_Left:
                rect.anchorMin =    new Vector2( 0, 1f );
                rect.anchorMax =    new Vector2( 0, 1f );
                break;
            case Anchor.Top_Center:
                rect.anchorMin =    new Vector2( 0.5f, 1f );
                rect.anchorMax =    new Vector2( 0.5f, 1f );
                break;
            case Anchor.Top_Right:
                rect.anchorMin =    new Vector2( 1f, 1f );
                rect.anchorMax =    new Vector2( 1f, 1f );
                break;
            case Anchor.TopStretch:
                rect.anchorMin =    new Vector2( 0, 1f );
                rect.anchorMax =    new Vector2( 1, 1f );
                break;
            case Anchor.Middle_Left:
                rect.anchorMin =    new Vector2( 0, 0.5f );
                rect.anchorMax =    new Vector2( 0, 0.5f );
                break;
            case Anchor.Middle_Center:
                rect.anchorMin =    new Vector2( 0.5f, 0.5f );
                rect.anchorMax =    new Vector2( 0.5f, 0.5f );
                break;
            case Anchor.Middle_Right:
                rect.anchorMin =    new Vector2( 1f, 0.5f );
                rect.anchorMax =    new Vector2( 1f, 0.5f );
                break;
            case Anchor.MiddleStretch:
                rect.anchorMin =    new Vector2( 0, 0.5f );
                rect.anchorMax =    new Vector2( 1, 0.5f );
                break;
            case Anchor.Bottom_Left:
                rect.anchorMin =    new Vector2( 0, 0 );
                rect.anchorMax =    new Vector2( 0, 0 );
                break;
            case Anchor.Bottom_Center:
                rect.anchorMin =    new Vector2( 0.5f, 0 );
                rect.anchorMax =    new Vector2( 0.5f, 0 );
                break;
            case Anchor.Bottom_Right:
                rect.anchorMin =    new Vector2( 1f, 0 );
                rect.anchorMax =    new Vector2( 1f, 0 );
                break;
            case Anchor.BottomStretch:
                rect.anchorMin =    new Vector2( 0, 0 );
                rect.anchorMax =    new Vector2( 1f, 0 );
                break;
            case Anchor.LeftStretch:
                rect.anchorMin =    new Vector2( 0, 0 );
                rect.anchorMax =    new Vector2( 0, 1f );
                break;
            case Anchor.CenterStretch:
                rect.anchorMin =    new Vector2( 0.5f, 0 );
                rect.anchorMax =    new Vector2( 0.5f, 1 );
                break;
            case Anchor.RightStretch:
                rect.anchorMin =    new Vector2( 1f, 0 );
                rect.anchorMax =    new Vector2( 1f, 1f );
                break;
            case Anchor.AllStretch:
                rect.anchorMin =    new Vector2( 0, 0 );
                rect.anchorMax =    new Vector2( 1f, 1f );
                break;
        }

        if( isSetZero == true )
        {
            rect.transform.localPosition = Vector3.zero;
            rect.anchoredPosition = Vector3.zero;
        }
    }

    public static void SetAnchor( RectTransform rect, float? minX, float? minY, float? maxX, float? maxY )
    {
        if( rect == null ) return;
        
        var _currentMin = rect.anchorMin;
        var _currentMax = rect.anchorMax;

        if( minX != null ) _currentMin.x = (float)minX;
        if( minY != null ) _currentMin.y = (float)minY;
        if( maxX != null ) _currentMax.x = (float)maxX;
        if( maxY != null ) _currentMax.y = (float)maxY;

        rect.anchorMin = _currentMin;
        rect.anchorMax = _currentMax;
    }

    // ---------------------------------------------------------------------------------
    /// <summary>
    /// レクトトランスフォームのストレッチの値を一括設定.
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="top"></param>
    /// <param name="bottom"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    // ---------------------------------------------------------------------------------
    public static void SetRectTransformStretch( RectTransform rect , float top, float bottom, float left, float right )
    {
        if( rect == null )
        {
            return;
        }
        // Debug.Log( top + "/" + bottom + "/" + left + "/" + right );
        rect.offsetMin = new Vector2 ( left, bottom );
        rect.offsetMax = new Vector2 ( -right, -top );
    }

    public static void SetRectTransformStretch( RectTransform rect, string set, float value )
    {
        switch( set )
        {
            case "Top": case "top":
            {
                var _current = rect.offsetMax;
                _current.y = -value;
                rect.offsetMax = _current;
            }
            break;
            case "Right": case "right":
            {
                var _current = rect.offsetMax;
                _current.x = -value;
                rect.offsetMax = _current;
            }
            break;
            case "Bottom": case "bottom":
            {
                var _current = rect.offsetMin;
                _current.y = value;
                rect.offsetMin = _current;
            }
            break;
            case "Left": case "left":
            {
                var _current = rect.offsetMin;
                _current.x = value;
                rect.offsetMin = _current;
            }
            break;
        }
    }

    public static void SetPivot( PivotType pivot, RectTransform rect, bool isSetZero = false )
    {
        switch( pivot )
        {
            case PivotType.Top_Left:        rect.pivot = new Vector2( 0, 1f ); break;
            case PivotType.Top_Center:      rect.pivot = new Vector2( 0.5f, 1f ); break;
            case PivotType.Top_Right:       rect.pivot = new Vector2( 1f, 1f ); break;
            case PivotType.Middle_Left:     rect.pivot = new Vector2( 0, 0.5f ); break;
            case PivotType.Middle_Center:   rect.pivot = new Vector2( 0.5f, 0.5f ); break;
            case PivotType.Middle_Right:    rect.pivot = new Vector2( 1f, 0.5f ); break;
            case PivotType.Bottom_Left:     rect.pivot = new Vector2( 0, 0 ); break;
            case PivotType.Bottom_Center:   rect.pivot = new Vector2( 0.5f, 0 ); break;
            case PivotType.Bottom_Right:    rect.pivot = new Vector2( 1f, 0 ); break;
        }

        if( isSetZero == true ) rect.transform.localPosition = Vector3.zero;
    }

    // ------------------------------------------------------------------------------------------
    /// <summary>
    /// アンカータイプの取得.
    /// </summary>
    /// <param name="anchorKey"> アンカー文字列. </param>
    /// <returns> アンカータイプ. </returns>
    // ------------------------------------------------------------------------------------------
    public static Anchor GetAnchorType( string anchorKey )
    {
        switch( anchorKey )
        {
            case "Top_Left": case "TopLeft": case "top_left": case "topleft": case "TOP_LEFT": case "TOPLEFT": case "tl": case "TL":                                        { return Anchor.Top_Left; }
            case "Top_Center": case "TopCenter": case "top_center": case "topcenter": case "TOP_CENTER": case "TOPCENTER": case "tc": case "TC":                            { return Anchor.Top_Center; }
            case "Top_Right": case "TopRight": case "top_right": case "topright": case "TOP_RIGHT": case "TOPRIGHT": case "tr": case "TR":                                  { return Anchor.Top_Right; }
            case "Top_Stretch": case "TopStretch": case "top_stretch": case "topstretch": case "TOP_STRETCH": case "TOPSTRETCH": case "ts": case "TS":                      { return Anchor.TopStretch; }

            case "Middle_Left": case "MiddleLeft": case "middle_left": case "middleleft": case "MIDDLE_LEFT": case "MIDDLELEFT": case "ml": case "ML":                      { return Anchor.Middle_Left; }
            case "Middle_Center": case "MiddleCenter": case "middle_center": case "middlecenter": case "MIDDLE_CENTER": case "MIDDLECENTER": case "mc": case "MC":          { return Anchor.Middle_Center; }
            case "Middle_Right": case "MiddleRight": case "middle_right": case "middleright": case "MIDDLE_RIGHT": case "MIDDLERIGHT": case "mr": case "MR":                { return Anchor.Middle_Right; }
            case "Middle_Stretch": case "MiddleStretch": case "middle_stretch": case "middlestretch": case "MIDDLE_STRETCH": case "MIDDLESTRETCH": case "ms": case "MS":    { return Anchor.MiddleStretch; }

            case "Bottom_Left": case "BottomLeft": case "bottom_left": case "bottomleft": case "BOTTOM_LEFT": case "BOTTOMLEFT": case "bl": case "BL":                      { return Anchor.Bottom_Left; }
            case "Bottom_Center": case "BottomCenter": case "bottom_center": case "bottomcenter": case "BOTTOM_CENTER": case "BOTTOMCENTER": case "bc": case "BC":          { return Anchor.Bottom_Center; }
            case "Bottom_Right": case "BottomRight": case "bottom_right": case "bottomright": case "BOTTOM_RIGHT": case "BOTTOMRIGHT": case "br": case "BR":                { return Anchor.Bottom_Right; }
            case "Bottom_Stretch": case "BottomStretch": case "bottom_stretch": case "bottomstretch": case "BOTTOM_STRETCH": case "BOTTOMSTRETCH": case "bs": case "BS":    { return Anchor.BottomStretch; }

            case "Left_Stretch": case "LeftStretch": case "left_stretch": case "leftstretch": case "LEFT_STRETCH": case "LEFTSTRETCH": case "ls": case "LS":                { return Anchor.LeftStretch; }
            case "Center_Stretch": case "CenterStretch": case "center_stretch": case "centerstretch": case "CENTER_STRETCH": case "CENTERSTRETCH": case "cs": case "CS":    { return Anchor.CenterStretch; }
            case "Right_Stretch": case "RightStretch": case "right_stretch": case "rightstretch": case "RIGHT_STRETCH": case "RIGHTSTRETCH": case "rs": case "RS":          { return Anchor.RightStretch; }
            case "All_Stretch": case "AllStretch": case "all_stretch": case "allstretch": case "ALL_STRETCH": case "ALLSTRETCH": case "all": case "ALL": case "All":        { return Anchor.AllStretch; }

            default: { return Anchor.Middle_Center; }
        }
    }

    // --------------------------------------------------------------------------------
    /// <summary>
    /// キャンバスのレンダーモードを取得.
    /// </summary>
    /// <param name="renderModeKey"> レンダーモードを表すキー文字列. </param>
    /// <returns> レンダーモード. </returns>
    // --------------------------------------------------------------------------------
    public static RenderMode GetCanvasRenderMode( string renderModeKey )
    {
        switch( renderModeKey )
        {
            case "ScreenSpaceOverlay": case "overlay": case "Overlay":                          { return RenderMode.ScreenSpaceOverlay; }
            case "ScreenSpaceCamera": case "camera": case "Camera":                             { return RenderMode.ScreenSpaceCamera; }
            case "WorldSpace": case "Worldspace": case "worldspace": case "World": case "world":{ return RenderMode.WorldSpace; }
            default :                                                                           { return RenderMode.ScreenSpaceOverlay; }
        }
    }

    public static Canvas GetCanvas( GameObject go )
    {
        if( go == null ) return null;
        
        // Canvasが見つかるまで親に向かって検索、見つからなかったらnull.
        var _canvas = go.GetComponentInParent<Canvas>();
        if (_canvas == null)
        {
            _canvas = go.AddComponent<Canvas>();
        }
        return _canvas;
    }

    // ----------------------------------------------------------------------------------
    /// <summary>
    /// カラー文字列からカラーの取得.
    /// </summary>
    /// <param name="key"> カラー文字列. </param>
    /// <returns> カラー. </returns>
    // ----------------------------------------------------------------------------------
    public static Color GetColorPreset( string key )
    {
        switch( key )
        {
            case "red": case "Red": case "RED" :            return Color.red;
            case "blue": case "Blue": case "BLUE" :         return Color.blue;
            case "green": case "Green": case "GREEN" :      return Color.green;
            case "yellow": case "Yellow": case "YELLOW" :   return Color.yellow;
            case "cyan": case "Cyan": case "CYAN" :         return Color.cyan;
            case "magenta": case "Magenta": case "MAGENTA" :return Color.magenta;
            case "grey": case "Grey": case "GREY" :         return Color.grey;
            case "clear": case "Clear": case "CLEAR" :      return Color.clear;
            case "black": case "Black": case "BLACK":       return Color.black;
            case "white": case "White": case "WHITE" :      return Color.white;
            default :                                       return Color.black;
        }
    }

    // -----------------------------------------------------------------------------------
    /// <summary>
    /// 現在のレクトトランスフォームのサイズを最大値として、テクスチャの縦横比を変えずにセットする.
    /// </summary>
    /// <param name="rectTransform"></param>
    /// <param name="texture"></param>
    // -----------------------------------------------------------------------------------
    public static void SetRectSizeFromTexture( RectTransform rectTransform, Texture2D texture )
    {
        var _texWHRatio = (float)texture.width / (float)texture.height;
        var _recWHtRatio = (float)rectTransform.rect.width / (float)rectTransform.rect.height;

        float _resultW = 0;
        float _resultH = 0;

        if( _recWHtRatio > _texWHRatio )
        {
            //縦を合わせて横を縮める.
            _resultW = rectTransform.rect.height * ( (float)texture.width / (float)texture.height );
            _resultH = rectTransform.rect.height;
        }
        else
        {
            //横を合わせて縦を縮める.
            _resultW = rectTransform.rect.width;
            _resultH = rectTransform.rect.width * ( (float)texture.height / (float)texture.width );
        }

        var _current = rectTransform.sizeDelta;
        var _currentPosition = rectTransform.anchoredPosition;
        _current.x = _resultW;
        _current.y = _resultH;
        SetAnchorPreset( Anchor.Middle_Center, rectTransform, true );
        rectTransform.sizeDelta = _current;
        rectTransform.anchoredPosition = _currentPosition;

    }

    // -----------------------------------------------------------------------------
    /// <summary>
    /// テクスチャをReadableに変換.
    /// </summary>
    /// <param name="texture2d"></param>
    /// <returns></returns>
    // -----------------------------------------------------------------------------
    public static Texture2D CreateReadabeTexture2D( Texture2D texture2d )
    {
        RenderTexture renderTexture = RenderTexture.GetTemporary(
                    texture2d.width,
                    texture2d.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(texture2d, renderTexture);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTexture;
        Texture2D readableTextur2D = new Texture2D(texture2d.width, texture2d.height);
        readableTextur2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        readableTextur2D.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTexture);
        return readableTextur2D;
    }
#endregion
}
