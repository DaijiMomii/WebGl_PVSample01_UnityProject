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
        if( Camera.main == null ) return;
        // transform.LookAt( Camera.main.transform.position );
        transform.forward = ( Camera.main.transform.position - transform.position ).normalized;
    }

    void FixedYBillboard()
    {
        if( Camera.main == null ) return;
        var _dir = ( Camera.main.transform.position - transform.position ).normalized;
        _dir.y = 0;
        transform.forward = _dir;        
    }

    void BackBillboard()
    {
        if( Camera.main == null ) return;
        var _dir = ( transform.position - Camera.main.transform.position ).normalized;
        transform.forward = _dir;      
    }

    void FixedYBackBillboard()
    {
        if( Camera.main == null ) return;
        var _dir = ( transform.position - Camera.main.transform.position ).normalized;
        _dir.y = 0;
        transform.forward = _dir;      
    }
}
