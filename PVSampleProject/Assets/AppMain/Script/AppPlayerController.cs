using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Video;

public class AppPlayerController : MonoBehaviour
{
    // -------------------------------------------------------------------------
    /// <summary>
    /// 移動パラメーター
    /// </summary>
    // -------------------------------------------------------------------------
    [System.Serializable]
    public class MoveParam
    {
        // 速度制限値.
        public float Limit = 15f;
        // 移動力.
        public float Power = 100f;
        // 即停止の境界値.
        public float StopLimit = 10f;
    }

    // カメラ回転速度.
    [SerializeField] float rotationSpeed = 3f;
    // 移動パラメータ.
    [SerializeField] MoveParam Move = new MoveParam();

    // カメラ水平回転トランスフォーム.
    [SerializeField] Transform cameraRootH = null;
    // カメラ垂直回転トランスフォーム.
    [SerializeField] Transform cameraRootV = null;


    // リジッドボディ.
    public Rigidbody Rigid
    {
        get
        {
            if( rigid == null ) rigid = GetComponent<Rigidbody>();
            return rigid;
        }
    }
    // スマホ入力の値.
    public Vector2? PhoneUiInput{ get; set; } = null;
    // 回転ロック.
    public bool IsRotationLock{ get; set; } = true;
    // 回転用タッチ操作の現在の指ID.
    public int CurrentRotationFingerId{ get; set; } = -1;
    // 移動用タッチ操作の現在の指ID.
    public int CurrentMoveFingerId{ get; set; } = -1;

    // 回転入力開始位置.
    Vector3? rotStartPos = null;
    // リジッドボディ.
    Rigidbody rigid = null;
    // 垂直回転値.
    float currentEulerRotationV = 0;

    



    void Start()
    {
        // 角度は取得すべきではないので、回転量を独自に保管.
        currentEulerRotationV = cameraRootV.localEulerAngles.x;
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        float _horizontal = 0;
        float _vertical = 0;

        if( PhoneUiInput != null )
        {
            var _input = (Vector2)PhoneUiInput;
            _horizontal = _input.x;
            _vertical = _input.y;
        }
        else
        {            
            _horizontal = Input.GetAxis( "Horizontal" );
            _vertical = Input.GetAxis( "Vertical" ); 
        }
        
        FixedUpdateMove( _horizontal, _vertical );   
    }


    // ----------------------------------------------------------------------------------------------
    /// <summary>
    /// カメラの回転処理.
    /// </summary>
    // ----------------------------------------------------------------------------------------------
    public void UpdateCameraRotation( bool isTouch )
    {        
        if( IsRotationLock == true ) return;

        if( isTouch == false )
        {
            var _pos = Input.mousePosition;

            if( rotStartPos == null )
            {
                rotStartPos = Input.mousePosition;
            }   
            else
            {
                var _rsPos = (Vector3)rotStartPos;
                var _moveX = _pos.x - _rsPos.x;
                var _moveY = _pos.y - _rsPos.y;

                // Y軸周りの回転.
                cameraRootH.Rotate( 0, -_moveX * rotationSpeed * 0.01f, 0 );
                // X軸周りの回転（90°制限）.
                var _addValue = _moveY * rotationSpeed * 0.01f;                
                if( currentEulerRotationV < 90f && currentEulerRotationV > -90f )
                {
                    _Add( _addValue );
                }               
                else if( currentEulerRotationV >= 90f )
                {
                    if( _addValue > 0 ) _Set( 90f );                  
                    else _Add( _addValue );
                }
                else if( currentEulerRotationV <= -90f )
                {
                    if( _addValue < 0 ) _Set( -90f );                  
                    else _Add( _addValue );
                }
                else 
                {
                    Debug.LogWarning( "CameraRotation Warning @@ " + currentEulerRotationV );
                }                

                rotStartPos = _pos;  
            }

            // AppGameManager.Instance.DebugText.text = _pos.ToString();
        }
        else
        {
            if( CurrentRotationFingerId < 0 )
            {
                Debug.LogWarning( "FingerIdが見つかりません" );
                return;
            }

            Touch? _selectedTouch = null;
            var _touches = Input.touches;
            foreach( var t in _touches )
            {
                if( t.fingerId == CurrentRotationFingerId ) _selectedTouch = t;
            }

            if( _selectedTouch == null ) 
            {
                Debug.LogWarning( "タッチしている指の検出に失敗しました。FingerId=" + CurrentRotationFingerId );
                return;
            }
            
            // 
            var _touch = (Touch)_selectedTouch;

            if( rotStartPos == null )
            {
                rotStartPos = _touch.position;
                // AppGameManager.Instance.DebugText.text = _touch.position.ToString();
            }
            else
            {
                if( _touch.phase != TouchPhase.Moved )
                {
                    Debug.Log( _touch.phase + " 一旦回転をリセットします。" );
                    return;
                }

                var _rsPos = (Vector3)rotStartPos;
                var _moveX = _touch.position.x - _rsPos.x;
                var _moveY = _touch.position.y - _rsPos.y;

                // Y軸周りの回転.
                cameraRootH.Rotate( 0, -_moveX * rotationSpeed * 0.01f, 0 );
                // X軸周りの回転（90°制限）.
                var _addValue = _moveY * rotationSpeed * 0.01f;                
                if( currentEulerRotationV < 90f && currentEulerRotationV > -90f )
                {
                    _Add( _addValue );
                }               
                else if( currentEulerRotationV >= 90f )
                {
                    if( _addValue > 0 ) _Set( 90f );                  
                    else _Add( _addValue );
                }
                else if( currentEulerRotationV <= -90f )
                {
                    if( _addValue < 0 ) _Set( -90f );                  
                    else _Add( _addValue );
                }
                else 
                {
                    Debug.LogWarning( "CameraRotation Warning @@ " + currentEulerRotationV );
                }                

                rotStartPos = _touch.position; 
                
                // AppGameManager.Instance.DebugText.text = _rsPos.ToString();
            }           

            
        }


        void _Add( float value )
        {
            currentEulerRotationV += value;
            var _currentV = cameraRootV.localEulerAngles;
            _currentV.x = currentEulerRotationV;
            cameraRootV.localEulerAngles = _currentV;
        }

        void _Set( float value )
        {
            currentEulerRotationV = value;
            var _currentV = cameraRootV.localEulerAngles;
            _currentV.x = currentEulerRotationV;
            cameraRootV.localEulerAngles = _currentV;
        }
    }

    public void ResetRotationValue()
    {
        rotStartPos = null;
        IsRotationLock = true;
        CurrentRotationFingerId = -1;
    }



    void FixedUpdateMove( float horizontal, float vertical )
    {
        if( AppGameManager.Instance.CurrentLock.Move == true ) return;

        var _current = Rigid.velocity;

        if( ( horizontal != 0 || vertical != 0 ) && _current.sqrMagnitude < Move.Limit )
        {
            var _force = new Vector3( horizontal, 0, vertical ) * Move.Power;
            Rigid.AddForce( cameraRootH.rotation * _force );
        }
        else
        {
            MoveResistance();
        }

    }


    void MoveResistance()
    {
        var _current = Rigid.velocity;
        _current.y = 0;

        if( _current.sqrMagnitude > Move.StopLimit ) _Resist();
        else Rigid.velocity = Vector3.zero;     


        void _Resist()
        {
            _current.x *= 0.5f;
            _current.z *= 0.5f;
            Rigid.velocity = _current;
        }
    }

    public void TestPointerClicked( UnityEngine.EventSystems.BaseEventData eventData )
    {
        Debug.Log( eventData.selectedObject.name + "@@@" );
    }

    public void StopPlayer()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }



    
    
}
