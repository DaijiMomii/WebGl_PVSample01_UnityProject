using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
// -------------------------------------------------------------------------
/// <summary>
/// コライダーコールバックの受信ユーティリティクラス.
/// </summary>
// -------------------------------------------------------------------------
public class ColliderCallReceiver : MonoBehaviour
{
    // コライダーイベント定義クラス.
    [System.Serializable]
    public class CollisionEvent : UnityEvent<Collision>{}
    // コライダーエンターイベント.
    public CollisionEvent CollisionEnterEvent = new CollisionEvent();
    // コライダーステイイベント.
    public CollisionEvent CollisionStayEvent = new CollisionEvent();
    // コライダーイグジットイベント.
    public CollisionEvent CollisionExitEvent = new CollisionEvent();
    // トリガーイベント定義クラス.
    [System.Serializable]
    public class TriggerEvent : UnityEvent<Collider>{}
    // トリガーエンターイベント.
    public TriggerEvent TriggerEnterEvent = new TriggerEvent();
    // トリガーステイイベント.
    public TriggerEvent TriggerStayEvent = new TriggerEvent();
    // トリガーイグジットイベント.
    public TriggerEvent TriggerExitEvent = new TriggerEvent();
    void Start()
    {
        
    }
    // -------------------------------------------------------------------------
    /// <summary>
    /// コライダーエンターコールバック.
    /// </summary>
    /// <param name="col"> 接触したコライダー. </param>
    // -------------------------------------------------------------------------
    void OnCollisionEnter( Collision col )
    {
        CollisionEnterEvent?.Invoke( col );
    }
    // -------------------------------------------------------------------------
    /// <summary>
    /// コライダーステイコールバック.
    /// </summary>
    /// <param name="col"> 接触したコライダー. </param>
    // -------------------------------------------------------------------------
    void OnCollisionStay( Collision col )
    {
        CollisionStayEvent?.Invoke( col );
    }
    // -------------------------------------------------------------------------
    /// <summary>
    /// コライダーイグジットコールバック.
    /// </summary>
    /// <param name="col"> 接触したコライダー. </param>
    // -------------------------------------------------------------------------
    void OnCollisionExit( Collision col )
    {
        CollisionExitEvent?.Invoke( col );
    }
    // -------------------------------------------------------------------------
    /// <summary>
    /// トリガーエンターコールバック.
    /// </summary>
    /// <param name="other"> 接触したコライダー. </param>
    // -------------------------------------------------------------------------
    void OnTriggerEnter( Collider other )
    {
        TriggerEnterEvent?.Invoke( other );
    }
    // -------------------------------------------------------------------------
    /// <summary>
    /// トリガーステイコールバック.
    /// </summary>
    /// <param name="other"> 接触したコライダー. </param>
    // -------------------------------------------------------------------------
    void OnTriggerStay( Collider other )
    {
        TriggerStayEvent?.Invoke( other );
    }
    // -------------------------------------------------------------------------
    /// <summary>
    /// トリガーイグジットコールバック.
    /// </summary>
    /// <param name="other"> 接触したコライダー. </param>
    // -------------------------------------------------------------------------
    void OnTriggerExit( Collider other )
    {
        TriggerExitEvent?.Invoke( other );
    }
}


















