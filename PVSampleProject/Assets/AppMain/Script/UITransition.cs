using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

[RequireComponent( typeof(CanvasGroup) )]
public class UITransition : MonoBehaviour
{
    [System.Serializable]
    public class Parameter
    {
        public bool IsAlpha = true;
        public Vector2 InDirection = new Vector2( 0, 0 );
        public Vector2 OutDirection = new Vector2( 0, 0 );
        public Ease Ease = Ease.Linear;
    }   

    [SerializeField] float transitionTime = 1f;

    [SerializeField] Parameter parameter = new Parameter();

    public bool IsOpen{ get; private set; } = false;

    RectTransform rect = null;
    CanvasGroup canvasGroup = null;

    public RectTransform Rect
    {
        get
        {
            if( rect == null ) rect = GetComponent<RectTransform>();
            return rect;
        }
    }

    public  CanvasGroup CanvasGroup
    {
        get
        {
            if( canvasGroup == null ) canvasGroup = GetComponent<CanvasGroup>();
            return canvasGroup;
        }
    }
    
    void Start()
    {
    }


    public void TransitionIn( UnityAction completedAction = null, bool isImmediate = false )
    {
        gameObject.SetActive( true );
        if( isImmediate == true )
        {            
            CanvasGroup.alpha = 1;
            return;
        }

        var seq = DOTween.Sequence();

        var _goal = Rect.anchoredPosition;
        var _start = _goal + parameter.InDirection;
        Rect.anchoredPosition = _start;

        seq.Append
        (
            Rect.DOLocalMoveX( _goal.x, transitionTime )
        );

        seq.Join
        (
            Rect.DOLocalMoveY( _goal.y, transitionTime )
        );

        if( parameter.IsAlpha == true )
        {
            CanvasGroup.alpha = 0;
            seq.Join
            (            
                CanvasGroup.DOFade( 1f, transitionTime )
            );
        }

        seq        
        .SetEase( parameter.Ease )
        .SetLink( gameObject )
        .OnComplete( () => 
        {
            completedAction?.Invoke();
            IsOpen = true;
        } );
    }

    public void TransitionOut( UnityAction completedAction = null, bool isImmediate = false )
    {
        if( isImmediate == true )
        {
            gameObject.SetActive( false );
            CanvasGroup.alpha = 1;
            return;
        }

        var seq = DOTween.Sequence();

        var _start = Rect.anchoredPosition;
        var _goal = Rect.anchoredPosition + parameter.OutDirection;

        seq.Append
        (
            Rect.DOLocalMoveX( _goal.x, transitionTime )
        );

        seq.Join
        (
            Rect.DOLocalMoveY( _goal.y, transitionTime )
        );
        
        if( parameter.IsAlpha == true )
        {
            seq.Join
            (            
                CanvasGroup.DOFade( 0, transitionTime )
            );
        }

        seq        
        .SetEase( parameter.Ease )
        .SetLink( gameObject )
        .OnComplete( () => 
        {
            gameObject.SetActive( false );
            CanvasGroup.alpha = 1;
            Rect.anchoredPosition = _start;
            completedAction?.Invoke();

            IsOpen = false;
        } );
    }
}
