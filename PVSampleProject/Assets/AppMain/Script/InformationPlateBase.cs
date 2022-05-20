using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationPlateBase : MonoBehaviour
{
    [SerializeField] Text infoText = null;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetInformation( InteractableItemBase.InformationParam param )
    {
        infoText.text = param.Information;
    }
}
