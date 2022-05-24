using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent( typeof(UITransition) )]
public class PopupBase : MonoBehaviour
{
    public class CallbackParam
    {
        public bool IsClose = true;
        public UnityAction Action = null;
    }


    [SerializeField] Button button1 = null;
    [SerializeField] Button button2 = null;
    [SerializeField] Button button3 = null;

    UITransition transition = null;

    public int Answer{ get; private set; } = -1;

    public UITransition Transition
    {
        get
        {
            if( transition == null ) transition = GetComponent<UITransition>();
            return transition;
        }
    }


    protected virtual void Start()
    {
    }


    public virtual void Open( UnityAction openedAction = null, UnityAction buttonAction1 = null, UnityAction buttonAction2 = null, UnityAction buttonAction3 = null )
    {
        Transition.TransitionIn( openedAction );
        
        if( button1 != null && buttonAction1 != null ) button1.onClick.AddListener( buttonAction1 );
        if( button2 != null && buttonAction2 != null ) button2.onClick.AddListener( buttonAction2 );
        if( button3 != null && buttonAction3 != null ) button3.onClick.AddListener( buttonAction3 );
    }

    public virtual void Close( UnityAction closedAction = null )
    {
        Transition.TransitionOut( () => 
        {
            closedAction?.Invoke();
            Destroy( gameObject );
        });
    }

    public void OnButton1Clicked()
    {
        Answer = 1;
    }

    public void OnButton2Clicked()
    {
        Answer = 2;
    }

    public void OnButton3Clicked()
    {
        Answer = 3;
    }

}
