using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearObject
{
    public bool isNear;
    public Menu obj;
    public bool isHatch;
    public GameObject hatch;

    public NearObject(bool _isNear, 
        Menu _obj,
        bool _isHatch = false,
        GameObject _hatch = null) {
        isNear = _isNear;
        obj = _obj;
        isHatch = _isHatch;
        hatch = _hatch;
    }
}
