using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallNode 
{
    public Vector3 position;
    public Vector3 rotation;
    public WallNode(Vector3 _pos,Vector3 _rot)
    {
        position = _pos;
        rotation = _rot;
    }
}
