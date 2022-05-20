using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBillboard : MonoBehaviour
{
    public enum Type
    {
        Normal,
        Back,
        NormalFixedY,
        BackFixedY,
    }

    [SerializeField] Type BillboardType = Type.Normal;

    void Start()
    {
        
    }

    void Update()
    {
        switch( BillboardType )
        {
            case Type.Normal: Billboard(); break;
            case Type.NormalFixedY: FixedYBillboard(); break;
            case Type.Back: BackBillboard(); break;
            case Type.BackFixedY: FixedYBackBillboard(); break;
        }
    }

    void Billboard()
    {
        // transform.LookAt( Camera.main.transform.position );
        transform.forward = ( Camera.main.transform.position - transform.position ).normalized;
    }

    void FixedYBillboard()
    {
        var _dir = ( Camera.main.transform.position - transform.position ).normalized;
        _dir.y = 0;
        transform.forward = _dir;        
    }

    void BackBillboard()
    {
        var _dir = ( transform.position - Camera.main.transform.position ).normalized;
        transform.forward = _dir;      
    }

    void FixedYBackBillboard()
    {
        var _dir = ( transform.position - Camera.main.transform.position ).normalized;
        _dir.y = 0;
        transform.forward = _dir;      
    }
}
