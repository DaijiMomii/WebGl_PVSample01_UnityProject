using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent( typeof( UITransition ) )]
public class UIAutoHide : MonoBehaviour
{
    [SerializeField] float hideTime = 3f;
    public bool IsActiveAutoHide{ get; set; } = true;
    public bool IsShow{ get; private set; } = true;

    public UITransition Transition
    {
        get
        {
            if( transition == null ) transition = GetComponent<UITransition>();
            return transition;
        }
    }

    UITransition transition = null;
    float timer = 0;

    void Start()
    {
        IsShow = ( Transition.CanvasGroup.alpha != 0 );
    }

    void Update()
    {
        if( Input.GetMouseButton( 0 ) == true )
        {
            timer = 0;
            if( IsShow == false ) Show();
        }

        if( IsShow == true && IsActiveAutoHide == true )
        {
            timer += Time.deltaTime;

            if( timer > hideTime )
            {
                Hide();
                timer = 0;
            }
        }

    }

    public void Hide( UnityAction completedAction = null )
    {
        Transition.TransitionOut( () => 
        {
            IsShow = false;
            completedAction?.Invoke();
        },
        false );
    }

    public void Show( UnityAction completedAction = null )
    {
        Transition.TransitionIn( () => 
        {
            IsShow = true;
            completedAction?.Invoke();
        } );
    }

}
