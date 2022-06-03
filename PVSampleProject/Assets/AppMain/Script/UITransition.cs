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
    public bool IsTransition{ get; private set; } = false;

    RectTransform rect = null;
    CanvasGroup canvasGroup = null;

    Sequence currentSeq = null;

    bool isInit = false;

    Vector2 defaultAnchoredPosition = Vector3.zero;
    float defaultAlpha = 1f;

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
        if( IsTransition == false )
        {
            IsOpen = ( CanvasGroup.alpha != 0 );
        }
    }

    void Init()
    {
        if( isInit == false )
        {
            defaultAnchoredPosition = Rect.anchoredPosition;
            defaultAlpha = CanvasGroup.alpha;

            isInit = true;
        }
    }


    public void TransitionIn( UnityAction completedAction = null, bool isImmediate = false, bool? blockRayCasts = null )
    {
        gameObject.SetActive( true );
        if( isImmediate == true )
        {            
            CanvasGroup.alpha = 1;
            return;
        }

        IsTransition = true;

        Init();

        if( currentSeq != null )
        {
            DOTween.Kill( currentSeq );
            currentSeq = null;
        }

        currentSeq = DOTween.Sequence();

        var _goal = defaultAnchoredPosition;//Rect.anchoredPosition;
        var _start = ( Rect.anchoredPosition == defaultAnchoredPosition ) ? _goal + parameter.InDirection : Rect.anchoredPosition;
        Rect.anchoredPosition = _start;

        currentSeq.Append
        (
            // Rect.DOLocalMoveX( _goal.x, transitionTime )
            Rect.DOAnchorPosX( _goal.x, transitionTime )
        );

        currentSeq.Join
        (
            // Rect.DOLocalMoveY( _goal.y, transitionTime )
            Rect.DOAnchorPosY( _goal.y, transitionTime )
        );

        if( parameter.IsAlpha == true )
        {
            CanvasGroup.alpha = 0;
            currentSeq.Join
            (            
                CanvasGroup.DOFade( 1f, transitionTime )
            );
        }

        currentSeq        
        .SetEase( parameter.Ease )
        .SetLink( gameObject )
        .OnComplete( () => 
        {
            completedAction?.Invoke();

            Rect.anchoredPosition = defaultAnchoredPosition;
            IsOpen = true;
            IsTransition = false;
            SetBlockRayCast( blockRayCasts );
        } );
    }

    public void TransitionOut( UnityAction completedAction = null, bool isSetInactive = true, bool isImmediate = false, bool? blockRayCasts = null )
    {
        if( isImmediate == true )
        {
            if( isSetInactive == true ) gameObject.SetActive( false );
            CanvasGroup.alpha = 0;
            return;
        }

        IsTransition = true;

        Init();

        if( currentSeq != null )
        {
            DOTween.Kill( currentSeq );
            currentSeq = null;
        }

        currentSeq = DOTween.Sequence();

        var _start = Rect.anchoredPosition;
        var _goal = defaultAnchoredPosition + parameter.OutDirection;

        currentSeq.Append
        (
            // Rect.DOLocalMoveX( _goal.x, transitionTime )
            Rect.DOAnchorPosX( _goal.x, transitionTime )
        );

        currentSeq.Join
        (
            // Rect.DOLocalMoveY( _goal.y, transitionTime )
            Rect.DOAnchorPosY( _goal.y, transitionTime )
        );
        
        if( parameter.IsAlpha == true )
        {
            currentSeq.Join
            (            
                CanvasGroup.DOFade( 0, transitionTime )
            );
        }

        currentSeq        
        .SetEase( parameter.Ease )
        .SetLink( gameObject )
        .OnComplete( () => 
        {
            CanvasGroup.alpha = 0;            
            Rect.anchoredPosition = defaultAnchoredPosition;
            if( isSetInactive == true )
            {
                gameObject.SetActive( false );
                CanvasGroup.alpha = 1;
            }

            completedAction?.Invoke();

            IsOpen = false;
            IsTransition = false;
            SetBlockRayCast( blockRayCasts );
        } );

        
    }

    void SetBlockRayCast( bool? isBlockRaycasts )
    {
        if( isBlockRaycasts != null )
        {
            CanvasGroup.blocksRaycasts = (bool)isBlockRaycasts;
        }
    }
}
