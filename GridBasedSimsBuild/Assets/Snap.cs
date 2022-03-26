using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snap : MonoBehaviour
{
    
    public Transform obj;
    Grid grid;
    public LayerMask wallMask;

    private void Start()
    {
        grid = GetComponent<Grid>();
        Vector3 mousePos=GetMousePos();
        //obj.position = mousePos;

    }
    private void Update()
    {
        Vector3 pos = GetGridToSnap(GetMousePos());
        SnapToPoint(pos);
        
    }


    public Vector3 GetMousePos()
    {
        Vector3 mouse = Input.mousePosition;
        Ray cast = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if(Physics.Raycast(cast,out hit, Mathf.Infinity))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
    public Transform getMouseHit()
    {
        Vector3 mouse = Input.mousePosition;
        Ray cast = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(cast, out hit, Mathf.Infinity,wallMask))
        {
            return hit.transform;
        }
        return null;
    }

    public Vector3 GetGridToSnap(Vector3 posOfMouse)
    {
        float percentX = (((grid.gridSizeX / 2) + posOfMouse.x/4) / grid.gridSizeX);
        float percentY = (((grid.gridSizeY / 2) + posOfMouse.z/4) / grid.gridSizeY);
        int x = Mathf.RoundToInt((grid.gridSizeX -1)*percentX);
        int y= Mathf.RoundToInt((grid.gridSizeY -1)*percentY);

        Vector3[] positions =
        {
            grid.grids[x, y].positon,
            grid.grids[x+1, y].positon,
            grid.grids[x, y+1].positon,
            grid.grids[x+1, y+1].positon
        };
        float distance = float.MaxValue;
        int pos = 0;
        for (int i = 0; i < positions.Length; i++)
        {
            float tmpDistance = Mathf.Sqrt(Mathf.Pow((posOfMouse.x - positions[i].x), 2) + Mathf.Pow((posOfMouse.z - positions[i].z), 2));
            if (distance > tmpDistance)
            {
                pos = i;
                distance = tmpDistance;
            }
        }
        return positions[pos];
    }
    public void SnapToPoint(Vector3 pos)
    {
        obj.position = pos;
    }
}