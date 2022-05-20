using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Runtime.InteropServices;

public class WarpGate : MonoBehaviour
{

    [SerializeField] string linkUrl = "";
    // "https://daijimomii.github.io/WebGl_PVSample_Info/"

    public UnityEvent WarpGateEvent = new UnityEvent();



    

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    void OnTriggerEnter( Collider other ) 
    {
        if( other.tag == "Player" )
        {
            Debug.Log( "ワープ" );
            WarpGateEvent?.Invoke();

            if( string.IsNullOrEmpty( linkUrl ) == false )
            {
                AppGameManager.Instance.AccessUrl( linkUrl );
            }
        }
    }

}
