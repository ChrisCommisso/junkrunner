using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformState
{
    public TransformState() {
        location = Vector3.zero;
        scale    = Vector3.zero;
        rotation = Vector3.zero;          
    }
    public TransformState(Transform t) { 
    location = t.position;
    rotation = t.localRotation.eulerAngles;
        scale = t.localScale;
    }
    public Vector3 location;
    public Vector3 scale;
    public Vector3 rotation;
}
