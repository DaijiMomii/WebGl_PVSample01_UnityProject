using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // ---------------------------------------------------------------------
    /// <summary>
    /// カメラ回転マウスクリック処理.
    /// </summary>
    // ---------------------------------------------------------------------
    public void UpdateClickCameraRotation()
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

    // ---------------------------------------------------------------------
    /// <summary>
    /// カメラ回転タッチ処理.
    /// </summary>
    /// <param name="touch"> 回転を行うタッチ. </param>
    // ---------------------------------------------------------------------
    public void UpdateTouchCameraRotation( Touch touch )
    {
        if( rotStartPos == null )
        {
            rotStartPos = touch.position;
        }
        else
        {
            if( touch.phase != TouchPhase.Moved )
            {
                Debug.Log( touch.phase + " 一旦回転をリセットします。" );
                return;
            }

            var _rsPos = (Vector3)rotStartPos;
            var _moveX = touch.position.x - _rsPos.x;
            var _moveY = touch.position.y - _rsPos.y;

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

            rotStartPos = touch.position; 
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

    // ---------------------------------------------------------------------
    /// <summary>
    /// 回転のための値のリセット.
    /// </summary>
    // ---------------------------------------------------------------------
    public void ResetRotationValue()
    {
        rotStartPos = null;
    }

    // ---------------------------------------------------------------------
    /// <summary>
    /// 移動更新処理.
    /// </summary>
    /// <param name="horizontal"> 横入力値. </param>
    /// <param name="vertical"> 縦入力値. </param>
    // ---------------------------------------------------------------------
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

    // ---------------------------------------------------------------------
    /// <summary>
    /// 移動抵抗力.
    /// </summary>
    // ---------------------------------------------------------------------
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

    // ---------------------------------------------------------------------
    /// <summary>
    /// 移動を強制的に停止.
    /// </summary>
    // ---------------------------------------------------------------------
    public void StopPlayer()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }


    public void TestPointerClicked( UnityEngine.EventSystems.BaseEventData eventData )
    {
        Debug.Log( eventData.selectedObject.name + "@@@" );
    }




    
    
}
