using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItemBase : MonoBehaviour
{
    protected bool canClick = true;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public virtual void OnPointerDown()
    {
        Debug.Log( "Base Down" );
    }

    public virtual void OnPointerUp()
    {
        Debug.Log( "Base Up" );
    }

    public virtual void OnPointerHold()
    {
        Debug.Log( "Base Hold" );
    } 

    IEnumerator WaitCanClick()
    {
        yield return new WaitForSeconds( 0.5f );
        canClick = true;
    }
}
