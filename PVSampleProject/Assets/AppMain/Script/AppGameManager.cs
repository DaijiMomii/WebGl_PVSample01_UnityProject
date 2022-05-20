using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class AppGameManager : SingletonMonoBehaviour<AppGameManager>
{
    [DllImport("__Internal")]
    private static extern void DeviceInformation();

    [DllImport("__Internal")]
    private static extern void OpenUrl( string url );



    public enum MouseAction
    {
        Down, Hold, Up,
    }

    [System.Serializable]
    public class Lock
    {
        public bool Move = false;
        public bool Rotation = false;
    }

    [SerializeField] LayerMask mask = default(LayerMask);

    [SerializeField] AppPlayerController player = null;

    [SerializeField] RectTransform stickBgRect = null;
    [SerializeField] RectTransform stickImageRect = null;


    [SerializeField] Text platformText = null;

    
    public Text DebugText = null;


    public Lock CurrentLock = new Lock();

    
    Vector3? startMoveMousePosition = null;

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
            UpdateRaycast( MouseAction.Down );
        }
        else if( Input.GetMouseButton( 0 ) == true ) 
        {
            UpdateRaycast( MouseAction.Hold );
        }
        else if( Input.GetMouseButtonUp( 0 ) == true ) 
        {
            UpdateRaycast( MouseAction.Up );
        }

        UpdateCameraRotation();
    }

    void UpdateRaycast( MouseAction mouseAction ) 
    {    
        // Rayの作成
        // Ray ray = new Ray( transform.position, transform.forward );
        Ray mouseRay = Camera.main.ScreenPointToRay( Input.mousePosition );
    	
        // Rayが衝突したコライダーの情報を得る
        RaycastHit hit;
        // Rayが衝突したかどうか
        if( Physics.Raycast( mouseRay, out hit, 5.0f, mask )) 
        {
            Debug.Log( hit.collider.gameObject.name );
            DebugText.text = hit.collider.gameObject.name;
            if( hit.collider.gameObject.tag == "Interactable" )
            {
                var _interact = hit.collider.gameObject.GetComponent<InteractableItemBase>();
                
                switch( mouseAction )
                {
                    case MouseAction.Down: _interact.OnPointerDown(); break;
                    case MouseAction.Hold: _interact.OnPointerHold(); break;
                    case MouseAction.Up  : _interact.OnPointerUp();   break;
                }
            }
        } 
        else
        {
            DebugText.text = "none";
        }
    
        // // Rayが衝突した全てのコライダーの情報を得る。＊順序は保証されない
        // RaycastHit[] hits = Physics.RaycastAll( mouseRay, Mathf.Infinity );
        // foreach( var obj in hits ) 
        // {
            
        // }
    }

    
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




    public void OnPhoneUiStickPointerDown()
    {
        if( CurrentLock.Move == true ) return;

        if( Input.touchCount > 0 )
        {
            var _touches = Input.touches;
            var _currentTouch = _touches[ _touches.Length - 1 ];
            player.CurrentMoveFingerId = _currentTouch.fingerId;
        }
    }

    public void OnPhoneUiStickPointerUp()
    {
        stickImageRect.anchoredPosition = Vector3.zero;
        startMoveMousePosition = null;
        player.PhoneUiInput = null;
    }
    
    public void OnPhoneUiStickDrag()
    {
        if( CurrentLock.Move == true ) return;

        if( startMoveMousePosition == null )
        {
            if( Input.touchCount > 0 )
            { 
                var _touches = Input.touches;
                if( player.CurrentMoveFingerId <= 0 ) 
                {                   
                    var _currentTouch = _touches[ _touches.Length - 1 ];
                    player.CurrentMoveFingerId = _currentTouch.fingerId;
                }

                Touch? _selectedTouch = null;
                foreach( var t in _touches )
                {
                    if( t.fingerId == player.CurrentMoveFingerId ) _selectedTouch = t;
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
                    if( t.fingerId == player.CurrentMoveFingerId ) _selectedTouch = t;
                }
               
                if( _selectedTouch == null )
                {
                    Debug.LogWarning( "IDに該当するタッチを検出できませんでした、FingerId = " + player.CurrentMoveFingerId );
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



    public void OnPhoneUiBgPointerDown()
    {
        if( CurrentLock.Rotation == true ) return;

        if( Input.touchCount > 0 )
        {
            var _touches = Input.touches;
            var _currentTouch = _touches[ _touches.Length - 1 ];
            if( player.CurrentRotationFingerId != -1 && player.CurrentRotationFingerId != _currentTouch.fingerId )
            {
                player.ResetRotationValue();
            }

            player.CurrentRotationFingerId = _currentTouch.fingerId;
            player.IsRotationLock = false;
        }
        else
        {            
            player.IsRotationLock = false;
        }
    }

    public void OnPhoneUiBgDrag()
    {
        // if( CurrentLock.Rotation == true ) return;

        // if( Input.touchCount > 0 )
        // {
        //     var _touches = Input.touches;
        //     var _currentTouch = _touches[ _touches.Length - 1 ];
        //     if( player.CurrentRotationFingerId != -1 && player.CurrentRotationFingerId != _currentTouch.fingerId )
        //     {
        //         player.ResetRotationValue();
        //         player.CurrentRotationFingerId = _currentTouch.fingerId;
        //     }

        //     player.UpdateCameraRotation( true );
        // }
        // else
        // {            
        //     player.UpdateCameraRotation( false );
        // }        

        // UpdateCameraRotation();
    }

    void UpdateCameraRotation()
    {
        if( CurrentLock.Rotation == true ) return;

        if( Input.touchCount > 0 )
        {
            var _touches = Input.touches;
            var _currentTouch = _touches[ _touches.Length - 1 ];
            if( player.CurrentRotationFingerId != -1 && player.CurrentRotationFingerId != _currentTouch.fingerId )
            {
                player.ResetRotationValue();
                player.CurrentRotationFingerId = _currentTouch.fingerId;
            }

            player.UpdateCameraRotation( true );
        }
        else if( Input.GetMouseButton( 0 ) == true )
        {            
            player.UpdateCameraRotation( false );
        }        
    }

    public void OnPhoneUiBgPointerUp()
    {
        player.ResetRotationValue();
    }


    public void AccessUrl( string url )
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
        else
        {
            OpenUrl( url );
        }
    }
}
