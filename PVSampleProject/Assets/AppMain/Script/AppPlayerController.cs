using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    [SerializeField] float presentationTransitionTime = 0.5f;
    // 移動パラメータ.
    [SerializeField] MoveParam Move = new MoveParam();

    // カメラ水平回転トランスフォーム.
    [SerializeField] Transform cameraRootH = null;
    // カメラ垂直回転トランスフォーム.
    [SerializeField] Transform cameraRootV = null;

    
    [SerializeField] Transform cameraLocalRoot = null;
    [SerializeField] Camera skyDomeCamera = null;
    [SerializeField] Camera attentionCamera = null;

    [SerializeField, Range( 0f, 30f )] float addCameraAnglesRatioInPresentation = 21f;


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
    Camera mainCamera = null;

    // List<( int, Transform )> childrenTransformWithLayer = new List<( int, Transform )>();

    



    void Start()
    {
        // 角度は取得すべきではないので、回転量を独自に保管.
        currentEulerRotationV = cameraRootV.localEulerAngles.x;
        AppGameManager.Instance.PresentationStartEvent.AddListener( SetPresentation );
        AppGameManager.Instance.PresentationEndEvent.AddListener( EndPresentation );
        AppGameManager.Instance.SkyDomeControl.StartSkyDomeEvent.AddListener( OnSkyDomeStarted );
        AppGameManager.Instance.SkyDomeControl.FinishSkyDomeEvent.AddListener( OnSkyDomeFinished );

        mainCamera = Camera.main;
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


    public void SetPresentation( InteractItem_Presentation presentationItem, bool isOpen )
    {
        attentionCamera.gameObject.SetActive( true );

        var seq = DOTween.Sequence();

        var _cameraTransform = presentationItem.GetCameraTransformInCurrentOrientation();
        var _pos = _cameraTransform.position;
        // 一旦リセット.
        _cameraTransform.localRotation = Quaternion.identity;
        var _rot = _cameraTransform.rotation.eulerAngles;
        var _ratio =  (float)Screen.width / (float)Screen.height;
        var _add = _ratio * addCameraAnglesRatioInPresentation;
        _rot.y = 180f + _add;
        // 先にマーカーの角度をローカルで設定.
        _cameraTransform.localRotation = Quaternion.Euler( _rot );

        // マーカーを元にワールド角度を取得.
        var _camQua = _cameraTransform.rotation;

        seq.Append
        (
            cameraLocalRoot.transform.DOMove( _pos, presentationTransitionTime )
        );

        seq.Join
        (  
            cameraLocalRoot.transform.DORotateQuaternion( _camQua, presentationTransitionTime )
        );

        seq
        .SetEase( Ease.InOutQuad )
        .SetLink( gameObject )
        .OnComplete( () => 
        {
            
            // Debug.Log( _cameraTransform.position + "/" + _cameraTransform.rotation.eulerAngles + " @@@ " + cameraLocalRoot.position + "/" + cameraLocalRoot.rotation.eulerAngles );
        } );

        
    }

    

    public void EndPresentation()
    {
        var seq = DOTween.Sequence();
        attentionCamera.gameObject.SetActive( false );

        seq.Append
        (
            cameraLocalRoot.transform.DOLocalMove( Vector3.zero, presentationTransitionTime )
        );

        seq.Join
        (  
            cameraLocalRoot.transform.DOLocalRotateQuaternion( Quaternion.identity, presentationTransitionTime )
        );       

        seq
        .SetEase( Ease.InOutQuad )
        .SetLink( gameObject );
    }



    void OnSkyDomeStarted( bool isStarted )
    {
        if( isStarted == true )
        {

        }
        else
        {            
            skyDomeCamera.gameObject.SetActive( true );
            mainCamera.gameObject.SetActive( false );
        }
    }

    void OnSkyDomeFinished( bool isStarted )
    {
        if( isStarted == true )
        {            
            mainCamera.gameObject.SetActive( true );        
            skyDomeCamera.gameObject.SetActive( false );
        }
        else
        {            
        }
    }





    public void TestPointerClicked( UnityEngine.EventSystems.BaseEventData eventData )
    {
        Debug.Log( eventData.selectedObject.name + "@@@" );
    }



    
    
}
