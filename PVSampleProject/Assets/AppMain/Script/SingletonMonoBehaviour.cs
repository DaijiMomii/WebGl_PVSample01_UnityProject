using UnityEngine;
using System.Collections.Generic;
using System.Collections;
// using Cysharp.Threading.Tasks;

// ----------------------------------------------------------------------
/// <summary>
/// MonoBehaviourを継承し、初期化メソッドを備えたシングルトンなクラス.
/// </summary>
// ----------------------------------------------------------------------
public class SingletonMonoBehaviour<T> : MonoBehaviourWithInit where T : MonoBehaviourWithInit
{
    //! インスタンス
    static T _instance;   

    //! インスタンスを外部から参照する用(getter)
    public static T Instance
    {
        get
        {
            // インスタンスがまだ作られていない
            if ( _instance == null )
            {
                try
                {
                    // シーン内からインスタンスを取得
                    _instance = (T)FindObjectOfType (typeof(T));

                    // シーン内に存在しない場合はエラー
                    if ( _instance == null )
                    {
                        // Debug.LogError ( typeof(T) + " is nothing" );
                        Debug.LogWarning( typeof(T) + " is nothing yet. Return null." );
                        return null;
                    }
                    //発見した場合は初期化
                    else 
                    {
                        _instance.InitIfNeeded ();
                    }
                }
                catch
                {
                    Debug.Log( "<color=red>Singleton Error. SingletonMonoBehaviour GetInstance. シングルトンが取得できません。</color>" );
                }
            }

            return _instance;
        }
        private set{}
    }

    // -------------------------------------------------------------------
    /// <summary>
    /// 初期化.
    /// </summary>
    // -------------------------------------------------------------------
    protected sealed override void Awake()
    {
        //存在しているインスタンスが自分であれば問題なし
        if( this == Instance )
        {
            DontDestroyOnLoad( this );
            InitIfNeeded();
            return;
        }

        // 自分じゃない場合は重複して存在しているので、エラー
        // Debug.LogError (typeof(T) + " is duplicated");
        Destroy( gameObject );
    }


    // -----------------------------------------------------------------
    /// <summary>
    /// 破壊時コールバック.
    /// </summary>
    // -----------------------------------------------------------------
    static void OnDestroy()
    {
        Instance = null;
    }
}




// -------------------------------------------------------------------
/// <summary>
/// 初期化メソッドを備えたMonoBehaviour
/// </summary>
// -------------------------------------------------------------------
public class MonoBehaviourWithInit : MonoBehaviour
{
    //初期化したかどうかのフラグ(一度しか初期化が走らないようにするため)
    bool _isInitialized = false;

    public bool IsInit{ get { return _isInitialized; } }

    // --------------------------------------------------------
    /// <summary>
    /// 必要なら初期化する
    /// </summary>
    // --------------------------------------------------------
    public void InitIfNeeded()
    {
        if( _isInitialized == true )
        {
            return;
        }

        Init();
        _isInitialized = true;
    }

    // ----------------------------------------------------------
    /// <summary>
    /// 初期化(Awake時かその前の初アクセス、どちらかの一度しか行われない)
    /// </summary>
    // ----------------------------------------------------------
    protected virtual void Init(){}

    //sealed overrideするためにvirtualで作成
    protected virtual void Awake (){}


}
