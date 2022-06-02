using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AppSkyDomeControl : MonoBehaviour
{
    public class SkyDomeEvent : UnityEvent<bool>{}
    public SkyDomeEvent StartSkyDomeEvent = new SkyDomeEvent(); 
    public SkyDomeEvent FinishSkyDomeEvent = new SkyDomeEvent(); 
    
    void Start()
    {
        
    }

}
