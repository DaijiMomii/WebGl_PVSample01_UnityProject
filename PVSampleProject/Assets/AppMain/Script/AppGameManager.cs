using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Runtime.InteropServices;
using DG.Tweening;

public class AppGameManager : SingletonMonoBehaviour<AppGameManager>
{
    
    // プラグインインポート端末情報の取得.
    [DllImport("__Internal")]
    private static extern void DeviceInformation();
    // プラグインインポートURLを開く.
    [DllImport("__Internal")]
    private static extern void OpenUrl( string url );
    [DllImport("__Internal")]
    private static extern void OpenUrlNewWindow( string url );


    // -------------------------------------------------------------------------------------
    /// <summary>
    /// マウスアクション.
    /// </summary>
    // -------------------------------------------------------------------------------------
    public enum MouseAction
    {
        None, Down, Hold, Up,
    }

    // -------------------------------------------------------------------------------------
    /// <summary>
    /// 固定（ロック）パラメータ定義.
    /// </summary>
    // -------------------------------------------------------------------------------------
    [System.Serializable]
    public class Lock
    {
        public bool Move = false;
        public bool Rotation = false;

        public bool Click = false;
        public bool Look = false;
    }

    // クリック、タップRayのマスク.
    [SerializeField] LayerMask clickRayMask = default(LayerMask);
    // 画面中心Rayのマスク.
    [SerializeField] LayerMask centerRayMask = default(LayerMask);
    // プレイヤー.
    [SerializeField] AppPlayerController player = null;
    // 移動用UIの背景レクトトランスフォーム.
    [SerializeField] RectTransform stickBgRect = null;
    // 移動用UIのスティックレクトトランスフォーム.
    [SerializeField] RectTransform stickImageRect = null;

    [SerializeField] Image bgImage = null;

    [SerializeField] Camera subUiCamera = null;

    [SerializeField] Transform popupParent = null;
    [SerializeField] UITransition popupBgTransition = null;

    [SerializeField] UITransition stopWindowTransition = null;




    // 現在のロック状態.
    public Lock CurrentLock = new Lock();
    // マウスクリックでの回転をロック.
    public bool IsClickRotationLock{ get; set; } = false;

    // タッチの際UIなどに触れて画面回転をしない指のフィンガーID.
    public List<int> CurrentNonRotationFingerId = new List<int>();
    // 現在回転に使用しているフィンガーID.
    public int CurrentRotationFingerId{ get; set; } = -1;
    // 現在移動に使用しているフィンガーID.
    public int CurrentMoveFingerId{ get; set; } = -1;

    public Camera SubUiCamera{ get{ return subUiCamera; } }

    
    public int DefaultLayerNum{ get; } = 0;
    public int AttentionLayerNum{ get; } = 7;


    public class PresentationEvent : UnityEvent<PresentationInteractItem>{}
    public PresentationEvent PresentationStartEvent = new PresentationEvent();
    public UnityEvent PresentationEndEvent = new UnityEvent();

    // 移動用のクリック開始位置.
    Vector3? startMoveMousePosition = null;


    InteractableItemBase currentCursorRayHit = null;
    InteractableItemBase currentCenterRayHit = null;


    
    // DEBUG.
    [SerializeField] Text platformText = null;
    // [SerializeField] Text touchDebugText = null;
    // public Text DebugText = null;
    // [SerializeField] GameObject testWindow = null;

    // [SerializeField] GameObject popupTest = null;



    void Start()
    {
        if( Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor )
        {
            platformText.text = "Editor";
        }
        else
        {            
            DeviceInformation();
            // platformText.text = "---";
        }

    }



    void Update()
    {
        if( Input.GetMouseButtonDown( 0 ) == true ) 
        {
            UpdateClickRaycast( MouseAction.Down );
        }
        else if( Input.GetMouseButton( 0 ) == true ) 
        {
            UpdateClickRaycast( MouseAction.Hold );
        }
        else if( Input.GetMouseButtonUp( 0 ) == true ) 
        {
            UpdateClickRaycast( MouseAction.Up );
        }
        else
        {
            UpdateClickRaycast( MouseAction.None );
        }

        UpdateCenterRaycast();


        // var _tCount = Input.touchCount;
        // var _isMouse = Input.GetMouseButton( 0 );
        // touchDebugText.text = "Touch : " + _tCount + "/ Mouse : " + _isMouse;
        // touchDebugText.text = "W : " + Screen.width + " H : " + Screen.height;

        UpdateRotation();

    }

    // -------------------------------------------------------------------------------------
    /// <summary>
    /// クリック、タップした位置にRayを発射.
    /// </summary>
    /// <param name="mouseAction"> マウス(タップ)のアクション状態. </param>
    // -------------------------------------------------------------------------------------
    void UpdateClickRaycast( MouseAction mouseAction ) 
    {    
        if( CurrentLock.Click == true )
        {
            if( currentCursorRayHit != null )
            {
                currentCursorRayHit.OnClickPointerExit();
                currentCursorRayHit = null;
            }
            return;
        }

        // Rayの作成
        Ray mouseRay = Camera.main.ScreenPointToRay( Input.mousePosition );
    	
        // Rayが衝突したコライダーの情報を得る
        RaycastHit hit;
        // Rayが衝突したかどうか
        if( Physics.Raycast( mouseRay, out hit, 5.0f, clickRayMask )) 
        {
            if( hit.collider.gameObject.tag == "Interactable" )
            {
                var _interact = hit.collider.gameObject.GetComponent<InteractableItemBase>();
                // DebugText.text = hit.collider.gameObject.name;

                if( currentCenterRayHit != null && currentCursorRayHit != _interact )
                {
                    _interact.OnClickPointerExit();                    
                }

                currentCursorRayHit = _interact;
                
                switch( mouseAction )
                {
                    case MouseAction.Down: _interact.OnClickPointerDown(); break;
                    case MouseAction.Hold: _interact.OnClickPointerHold(); break;
                    case MouseAction.Up  : _interact.OnClickPointerUp();   break;
                }
            }
            else
            {
                if( currentCursorRayHit != null )
                {
                    currentCursorRayHit.OnClickPointerExit();
                    currentCursorRayHit = null;
                }
            }
        } 
        else
        {
            // DebugText.text = "none";
            if( currentCursorRayHit != null )
            {
                currentCursorRayHit.OnClickPointerExit();
                currentCursorRayHit = null;
            }
        }
    
        // // Rayが衝突した全てのコライダーの情報を得る。＊順序は保証されない
        // RaycastHit[] hits = Physics.RaycastAll( mouseRay, Mathf.Infinity );
        // foreach( var obj in hits ) 
        // {
            
        // }
    }

    void UpdateCenterRaycast()
    {
        if( CurrentLock.Look == true )
        {
            if( currentCenterRayHit != null )
            {
                currentCenterRayHit.OnCenterPointerExit();
                currentCenterRayHit = null;
            }
            return;
        }

        Ray centerRay = Camera.main.ScreenPointToRay( new Vector3( Screen.width / 2f, Screen.height / 2f, 0 ) );
    	
        // Rayが衝突したコライダーの情報を得る
        RaycastHit hit;
        // Rayが衝突したかどうか
        if( Physics.Raycast( centerRay, out hit, 5.0f, centerRayMask )) 
        {
            // Debug.Log( "Center : " + hit.collider.gameObject.name );
            if( hit.collider.gameObject.tag == "Interactable" )
            {
                var _interact = hit.collider.gameObject.GetComponent<InteractableItemBase>();
                
                if( currentCenterRayHit == null )
                {
                    currentCenterRayHit = _interact;
                    _interact.OnCenterPointer();
                }
                else if( currentCenterRayHit == _interact ) 
                {
                    _interact.OnCenterPointer();
                }
                else if( currentCenterRayHit != _interact )
                {
                    currentCenterRayHit.OnCenterPointerExit();
                    currentCenterRayHit = _interact;
                    _interact.OnCenterPointer();
                }
                else 
                {
                    Debug.LogWarning( "currentCenterRayHit が不正な値です." );
                }
            }
            else
            {
                if( currentCenterRayHit != null )
                {
                    currentCenterRayHit.OnCenterPointerExit();
                    currentCenterRayHit = null;
                }
            }
        } 
        else
        {
            if( currentCenterRayHit != null )
            {
                currentCenterRayHit.OnCenterPointerExit();
                currentCenterRayHit = null;
            }
        }
    }

    // -------------------------------------------------------------------------------------
    /// <summary>
    /// 回転処理.
    /// </summary>
    // -------------------------------------------------------------------------------------
    void UpdateRotation()
    {

        if( CurrentLock.Rotation == true ) return;

        // マウスクリック.
        if( Input.GetMouseButton( 0 ) == true && Input.touchCount == 0 )
        {
            if( IsClickRotationLock == false ) player.UpdateClickCameraRotation();
        }
        else if( Input.touchCount > 0 )
        {
            List<Touch> _touchList = new List<Touch>();

            foreach( var t in Input.touches )
            {
                Debug.Log( t.fingerId + " : @1" );
                bool _isNonRot = false;
                foreach( var id in CurrentNonRotationFingerId )
                {
                    Debug.Log( id + " : @2" );
                    if( id == t.fingerId )
                    { 
                        _isNonRot = true; 
                        break; 
                    }
                }

                if( _isNonRot == false ) _touchList.Add( t );
            }

            if( _touchList.Count > 0 )
            {
                var _currentFinger = _touchList[ _touchList.Count - 1 ];
                Debug.Log( _currentFinger.fingerId + " : @3" );

                if( CurrentRotationFingerId != _currentFinger.fingerId )
                {
                    player.ResetRotationValue();
                }
                player.UpdateTouchCameraRotation( _currentFinger );
                CurrentRotationFingerId = _currentFinger.fingerId;
            }
            else
            {                
                Debug.Log( _touchList.Count + " : @4" );
                player.ResetRotationValue();
                CurrentRotationFingerId = -1;
            }
        }
        else if( Input.touchCount == 0 )
        {
            CurrentNonRotationFingerId.Clear();
            player.ResetRotationValue();
            CurrentRotationFingerId = -1;
        }
    }



    // -------------------------------------------------------------------------------------
    /// <summary>
    ///  スマホ用移動UIのポインターダウンコール.
    /// </summary>
    // -------------------------------------------------------------------------------------
    public void OnPhoneUiStickPointerDown()
    {
        if( CurrentLock.Move == true ) return;

        if( Input.touchCount > 0 )
        {
            var _touches = Input.touches;
            var _currentTouch = _touches[ _touches.Length - 1 ];
            CurrentMoveFingerId = _currentTouch.fingerId;

            //
            CurrentNonRotationFingerId.Add( _currentTouch.fingerId );
        }
        else 
        {
            IsClickRotationLock = true;
        }
        
    }

    // -------------------------------------------------------------------------------------
    /// <summary>
    ///  スマホ用移動UIのポインターアップコール.
    /// </summary>
    // -------------------------------------------------------------------------------------
    public void OnPhoneUiStickPointerUp()
    {
        stickImageRect.anchoredPosition = Vector3.zero;
        startMoveMousePosition = null;
        player.PhoneUiInput = null;
        IsClickRotationLock = false;


        if( Input.touchCount == 0 )
        {
            CurrentNonRotationFingerId.Clear();
        }
        else
        {
            int _releaseFinger = -1;
            foreach( var t in Input.touches )
            {
                foreach( var id in CurrentNonRotationFingerId )
                {
                    if( id == t.fingerId ) continue;
                    _releaseFinger = id;
                }
            } 

            if( _releaseFinger != -1 && CurrentNonRotationFingerId.Contains( _releaseFinger ) == true )
            {
                // var _rIndex = CurrentNonRotationFingerId.IndexOf( _releaseFinger );
                CurrentNonRotationFingerId.Remove( _releaseFinger );
            } 
        }
    }


    // -------------------------------------------------------------------------------------
    /// <summary>
    ///  スマホ用移動UIのポインタードラッグコール.
    /// </summary>
    // -------------------------------------------------------------------------------------
    public void OnPhoneUiStickDrag()
    {
        if( CurrentLock.Move == true ) return;

        if( startMoveMousePosition == null )
        {
            if( Input.touchCount > 0 )
            { 
                var _touches = Input.touches;
                if( CurrentMoveFingerId <= 0 ) 
                {                   
                    var _currentTouch = _touches[ _touches.Length - 1 ];
                    CurrentMoveFingerId = _currentTouch.fingerId;
                }

                Touch? _selectedTouch = null;
                foreach( var t in _touches )
                {
                    if( t.fingerId == CurrentMoveFingerId ) _selectedTouch = t;
                }

                if( _selectedTouch == null )
                {
                    Debug.Log( "タッチを検出できませんでした、" );
                    return;
                }

                var _touch = (Touch)_selectedTouch;
                startMoveMousePosition = _touch.position;
            }
            else
            {
                startMoveMousePosition = Input.mousePosition;
            }
        }
        else
        {
            Vector2 _diff = Vector2.zero;
            if( Input.touchCount > 0 )
            { 
                Touch? _selectedTouch = null;
                var _touches = Input.touches;
                foreach( var t in _touches )
                {
                    if( t.fingerId == CurrentMoveFingerId ) _selectedTouch = t;
                }
               
                if( _selectedTouch == null )
                {
                    Debug.LogWarning( "IDに該当するタッチを検出できませんでした、FingerId = " + CurrentMoveFingerId );
                    return;
                }

                var _touch = (Touch)_selectedTouch;

                _diff = ( _touch.position - (Vector2)startMoveMousePosition );
            }
            else
            {
                _diff = ( (Vector2)Input.mousePosition - (Vector2)startMoveMousePosition );
            }   
            
            var _dir = _diff.normalized;
            var _mag = _diff.sqrMagnitude;

            var _max = ( stickBgRect.sizeDelta.x / 2f ) - ( stickImageRect.sizeDelta.x / 2f );
            
            var _current = stickImageRect.anchoredPosition;
            _current.x = _diff.x * 0.2f;
            _current.y = _diff.y * 0.2f;

            if( _current.x > _max ) _current.x = _max;
            if( _current.y > _max ) _current.y = _max;

            stickImageRect.anchoredPosition = _current;

            var _playerInput = player.PhoneUiInput;
            if( _playerInput == null ) 
            {
                _playerInput = new Vector2( _dir.x, _dir.y );
            }
            else
            {
                var _in = (Vector2)_playerInput;
                _in.x = _dir.x;
                _in.y = _dir.y;
                _playerInput = _in;
            }

            player.PhoneUiInput = _playerInput;
        }        
    }
    
    // -------------------------------------------------------------------------------------
    /// <summary>
    /// URLにアクセス.
    /// </summary>
    /// <param name="url"></param>
    // -------------------------------------------------------------------------------------
    public void AccessUrl( string url, bool isNewWindow = false )
    {
        Debug.Log( url + " にアクセスします" );

        // CurrentLock.Move = true;
        // CurrentLock.Rotation = true;
        player.StopPlayer();

        if( Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor )
        {
            Debug.Log( "Editorでは通常のApplication.OpenURL()を使用します" );
            Application.OpenURL( url );
        }
        else if( isNewWindow == false )
        {
            OpenUrl( url );
        }
        else
        {
            OpenUrlNewWindow( url );
        }
    }


    // public void Open_Test()
    // {
    //     CurrentLock.Move = true;
    //     CurrentLock.Rotation = true;
    //     CurrentLock.Click = true;
    //     CurrentLock.Look = true;

    //     bgImage.gameObject.SetActive( true );
    //     var _current = bgImage.color;
    //     _current.a = 0;
    //     bgImage.color = _current;
    //     bgImage.DOFade( 0.5f, 1f )
    //     .OnComplete( () => 
    //     {  
    //         testWindow.SetActive( true );
    //     } );
    // }

    // public void Close_Test()
    // {
    //     testWindow.SetActive( false );
    //     bgImage.DOFade( 0f, 1f )
    //     .OnComplete( () => 
    //     {
    //         bgImage.gameObject.SetActive( false );

    //         CurrentLock.Move = false;
    //         CurrentLock.Rotation = false;
    //         CurrentLock.Click = false;
    //         CurrentLock.Look = false;

    //     } );
    // }



    public void SetPresentation( PresentationInteractItem presentationItem )
    {
        PresentationStartEvent?.Invoke( presentationItem );
    }

    public void EndPresentation()
    {
        PresentationEndEvent?.Invoke();
    }


    // public void TestPopup()
    // {
    //     OpenPopup( popupTest );
    // }

    public void AppStop()
    {
        CurrentLock.Click = true;
        CurrentLock.Look = true;
        CurrentLock.Move = true;
        CurrentLock.Rotation = true;

        
    }

    public void OpenStopWindow()
    {
        stopWindowTransition.TransitionIn();
    }

    public void AppRestart()
    {
        CurrentLock.Click = false;
        CurrentLock.Look = false;
        CurrentLock.Move = false;
        CurrentLock.Rotation = false;
    }
    public void CloseStopWindow()
    {
        stopWindowTransition.TransitionOut();
    }

    public void OnStopWindowRestartButtonClicked()
    {
        if( stopWindowTransition.IsOpen == true ) stopWindowTransition.TransitionOut();
        AppRestart();
    }


    public PopupBase OpenPopup( GameObject prefab, UnityAction openedAction = null, UnityAction<PopupBase> buttonAction1 = null, UnityAction<PopupBase> buttonAction2 = null, UnityAction<PopupBase> buttonAction3 = null, bool isBg = true )
    {
        if( isBg == true ) popupBgTransition.TransitionIn();

        var _go = Instantiate( prefab, popupParent.position, popupParent.rotation, popupParent );
        var _popup = _go.GetComponent<PopupBase>();
        _popup.Open
        ( 
            openedAction,
            () => // button1.
            {
                buttonAction1?.Invoke( _popup );
                // _StandardCloseAction();
            },
            () => // button2.
            {
                buttonAction2?.Invoke( _popup );
                // _StandardCloseAction();
            },
            () => // button3.
            {
                buttonAction3?.Invoke( _popup );
                // _StandardCloseAction();
            }
        );

        return _popup;

        // void _StandardCloseAction()
        // {
        //     if( isBg == true ) popupBgTransition.TransitionOut();
        //     _popup.Close();
        // }
    }

    public void ClosePopup( PopupBase popup )
    {
        popup.Close();
        if( popupBgTransition.IsOpen == true ) popupBgTransition.TransitionOut();
    }


    
    // -------------------------------------------------------------------------------------
    /// <summary>
    /// プラグインから実行するためのプラットフォーム判定.
    /// </summary>
    /// <param name="platformKey"></param>
    // -------------------------------------------------------------------------------------
    public void SetPlatform( string platformKey )
    {
        Debug.Log( "Platform : " + platformKey );
        platformText.text = platformKey;
    }
    

    public void TestLink()
    {
        Debug.Log( "https://daijimomii.github.io/WebGl_PVSample_Info/  にアクセスします" );
        // Application.OpenURL( "https://daijimomii.github.io/WebGl_PVSample_Info/" );
        OpenUrl( "https://daijimomii.github.io/WebGl_PVSample_Info/" );
    }

    public void ChangeScreenWidth( int width )
    {
        Debug.Log( "HTMLからのコール Width : " + width );
    }

    public void ChangeScreenHeight( int height )
    {
        Debug.Log( "HTMLからのコール Hight : " + height );
    }

}
