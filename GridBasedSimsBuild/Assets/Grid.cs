using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid :MonoBehaviour
{
    public int width=10;
    public int height=10;
    [System.NonSerialized]
    public int gridSizeX;
    [System.NonSerialized]
    public int gridSizeY;
    [System.NonSerialized]
    public Node[,] grids;
    Vector3 bottomLeftCorner;
    float nodeDiameter;
    public float nodeRadius=1;


    public void Awake()
    {
        Initiliaze();
        
    }
    public void Initiliaze()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(width / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(height / nodeDiameter);
        grids = new Node[gridSizeX,gridSizeY];
        bottomLeftCorner = new Vector3(-(gridSizeX/2)*nodeDiameter,0,-(gridSizeY/2)*nodeDiameter);
        CreateGrid();
    }
    public void CreateGrid() {
        
        
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 pos = bottomLeftCorner+((x*(Vector3.right*nodeDiameter))+(y*(Vector3.forward*nodeDiameter)));
                grids[x, y] = new Node(pos,x,y);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (grids != null)
        {
            foreach (Node node in grids)
            {
                Gizmos.DrawCube(node.positon, 0.2F * Vector3.one);
            }
        }
        
    }

}
