using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnimation : MonoBehaviour
{
    [SerializeField] Vector3 speed = new Vector3();
    
    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate( speed );
    }
}
