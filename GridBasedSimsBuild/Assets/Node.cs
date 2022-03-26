using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 positon;
    int gridX;
    int gridY;
    public Node(Vector3 pos,int gX,int gY)
    {
        positon = pos;
        gridX=gX;
        gridY = gY;
    }
}
