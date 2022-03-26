using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public Transform Wall;
    public Transform Window;
    Snap Snap;
    Vector3 GlobalStartPoint;
    Vector3 OffsetToOldPoint;
    Transform obj;
    bool BuildOn=false;
    List<Vertices> Vertices;
    List<Vector3> RealVertices;
    List<Vector3> VerticesForMesh;
    List<int> TriangleNums;
    public Transform Mesh;
    MeshCreate meshCreator;
    Vector3 OldDirection=new Vector3(99,0,0);
    int count = 0;


    public enum directionOfLastPosition {Up,Down};
    public void Awake()
    {
        VerticesForMesh=new List<Vector3>();
        TriangleNums=new List<int>();
        Vertices = new List<Vertices>();
        RealVertices = new List<Vector3>();
        Snap = GetComponent<Snap>();
        meshCreator = Mesh.GetComponent<MeshCreate>();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && !BuildOn)
        {
            
            obj = Snap.getMouseHit();
            if (obj != null)
            {
                Quaternion rot = obj.rotation;
                Vector3 pos = obj.position;
                Destroy(obj.gameObject);
                Instantiate(Window, pos, rot);
                
            }
            

        }
        if(Input.GetMouseButtonDown(0)&& !BuildOn)
        {
            BuildOn = true;
            GlobalStartPoint = Snap.GetGridToSnap(Snap.GetMousePos());
            OffsetToOldPoint = GlobalStartPoint;
            Vertices.Add(new Vertices(GlobalStartPoint,0));
        }
        else if(Input.GetMouseButtonDown(0) && BuildOn)
        {
            Vector3 posOfMouse = Snap.GetGridToSnap(Snap.GetMousePos());
            if (posOfMouse == GlobalStartPoint)
            {
                BuildWalls(posOfMouse);
                BuildOn = false;
                Vertices.RemoveAt(Vertices.Count - 1);
                if (Vertices[Vertices.Count - 1].pos.z > GlobalStartPoint.z)
                {
                    Debug.Log(BuildOn + " Building Ended " + Vertices.Count + " direction" + directionOfLastPosition.Up);
                    StartFloorMeshing(Vertices,directionOfLastPosition.Up);
                }
                else
                {
                    Debug.Log(BuildOn + " Building Ended " + Vertices.Count + " direction" + directionOfLastPosition.Down);
                    StartFloorMeshing(Vertices, directionOfLastPosition.Down);
                }
                
                
            }
            else
            {
              
                BuildWalls(posOfMouse);
            }
        }
        else if (Input.GetKeyDown(KeyCode.B) && !BuildOn)
        {
            StartFloorMeshing(Vertices, directionOfLastPosition.Up);
        }
        
    }

    public void StartFloorMeshing(List<Vertices> vertices,BuildingManager.directionOfLastPosition directionOfLastVert)
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            RealVertices.Add(vertices[i].pos);

        }

        while (vertices.Count > 3)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                int pointA = GetPoint(i - 1);
                int pointC = GetPoint(i + 1);
                Vector3 a = vertices[pointA].pos;
                int realPointA = vertices[pointA].index;
                Vector3 b = vertices[i].pos;
                int realPointB = vertices[i].index;
                Vector3 c = vertices[pointC].pos;
                int realPointC = vertices[pointC].index;
                Debug.Log(a);

                bool isEar = true;
                Vector3 ba = a - b;
                Vector3 bc = c - b;
                Vector3 cross = Vector3.Cross(ba, bc);
                if (directionOfLastVert == directionOfLastPosition.Up && cross.y > 0f)
                {
                    Debug.Log("Girdi Upa");
                    //Debug.Log(cross + " POİNTS " + GetPoint(i - 1) + " " + i + " " + GetPoint(i + 1) + " OLUR");
                    //If cross product of this non triangle point is negative than then it means this mesh is not possible
                    for (int j = 0; j < vertices.Count; j++)
                    {
                        if (vertices[j].pos == a || vertices[j].pos == b || vertices[j].pos == c)
                            continue;
                        if (IsPointInside(vertices[j].pos, a, b, c, 0))
                        {
                            isEar = false;
                            break;
                        }
                    }
                }
                else if (directionOfLastVert == directionOfLastPosition.Down && cross.y < 0f)
                {
                    Debug.Log("Girdi Downa");
                    //Debug.Log(cross + " POİNTS " + GetPoint(i - 1) + " " + i + " " + GetPoint(i + 1) + " OLUR");
                    for (int j = 0; j < vertices.Count; j++)
                    {
                        if (vertices[j].pos == a || vertices[j].pos == b || vertices[j].pos == c)
                            continue;
                        if (IsPointInside(vertices[j].pos, a, b, c, 1))
                        {
                            isEar = false;
                            break;
                        }
                    }
                }
                Debug.Log(isEar);
                if (isEar)
                {

                    
                    VerticesForMesh.Add(a);
                    VerticesForMesh.Add(c);
                    VerticesForMesh.Add(b);
                    TriangleNums.Add(realPointA);
                    TriangleNums.Add(realPointC);
                    TriangleNums.Add(realPointB);
                    vertices.RemoveAt(i);
                    if(vertices[vertices.Count - 1].pos.z > vertices[0].pos.z)
                    {
                        directionOfLastVert = directionOfLastPosition.Up;
                    }
                    else
                    {
                        directionOfLastVert = directionOfLastPosition.Down;
                    }
                    
                    Debug.Log(vertices.Count);
                    count++;
                    break;
                }
            }
        }
        
        TriangleNums.Add(vertices[2].index);
        TriangleNums.Add(vertices[1].index);
        TriangleNums.Add(vertices[0].index);





        meshCreator.Vertices = RealVertices.ToArray();
        Debug.Log(meshCreator.Vertices.Length);
        meshCreator.Triangles = TriangleNums.ToArray();
        meshCreator.CreateMesh();
        
    }
   
    private bool IsPointInside(Vector3 p, Vector3 a, Vector3 b, Vector3 c,int switchM) {
        Vector3 bc = c - b;
        Vector3 ca = a - c;
        Vector3 ab = b - a;

        Vector3 bp = p - b;
        Vector3 cp = p - c;
        Vector3 ap = p - a;

        float cross1 = Vector3.Cross(bc, bp).y;
        float cross2 = Vector3.Cross(ca, cp).y;
        float cross3 = Vector3.Cross(ab, ap).y;

        if(cross1 >0f && switchM == 0 || cross2 > 0f && switchM == 0 || cross3 > 0f && switchM == 0){
            Debug.Log("Girdi1");
            return false;
        }
        else if (cross1 > 0f && switchM == 0 || cross2 > 0f && switchM == 0 || cross3 > 0f && switchM == 1)
        {
            Debug.Log("Girdi2");
            return true;
        }
        else if (cross1 < 0f && switchM == 0 || cross2 < 0f && switchM == 0 || cross3 < 0f && switchM == 1)
        {
            Debug.Log("Girdi3");
            return false;
        }
        else if (cross1 < 0f && switchM == 0 || cross2 < 0f && switchM == 0 || cross3 < 0f && switchM == 0)
        {
            Debug.Log("Girdi4");
            return true;
        }
        return false;
    }

    private int GetPoint(int index)
    {
        if (index == -1)
        {
            return Vertices.Count - 1;
        }
        else if (index == Vertices.Count)
        {
            return 0;
        }
        return index;
    }

    public void BuildWalls(Vector3 pos)
    {
        int wallsToBuild=0;
        Vector3 temp = pos - OffsetToOldPoint;
        Vector3 direction = temp / temp.magnitude;
        if (direction != OldDirection)
        {
            Vertices.Add(new Vertices(pos,Vertices.Count));
            OldDirection = direction;
        }
        Vector3 positionOfWall=Vector3.zero; 
        if (direction == Vector3.right || direction == (-Vector3.right))
        {
            wallsToBuild = Mathf.RoundToInt(Mathf.Abs(pos.x - OffsetToOldPoint.x))/4;
            if (direction.x == -1)
            {
                
                for (int i = 0; i < wallsToBuild; i++)
                {
                    positionOfWall = new Vector3((OffsetToOldPoint.x-4) + (i * -4), OffsetToOldPoint.y, OffsetToOldPoint.z);
                    Instantiate(Wall, positionOfWall , Quaternion.identity);
                }
                
            }
            else if (direction.x == 1)
            {
                for (int i = 0; i < wallsToBuild; i++)
                {
                    positionOfWall = new Vector3((OffsetToOldPoint.x) + (i * 4), OffsetToOldPoint.y, OffsetToOldPoint.z);
                    Instantiate(Wall, positionOfWall, Quaternion.identity);
                }
            }
            
        }
        else if(direction == Vector3.forward || direction == (-Vector3.forward))
        {
            wallsToBuild = Mathf.RoundToInt(Mathf.Abs(pos.z - OffsetToOldPoint.z)) / 4;
            if (direction.z == -1)
            {
                
                for (int i = 0; i < wallsToBuild; i++)
                {
                    positionOfWall = new Vector3(OffsetToOldPoint.x, OffsetToOldPoint.y, (OffsetToOldPoint.z) + (i * -4));
                    Vector3 rotation = new Vector3(0, 90, 0);
                    Quaternion rot = Quaternion.Euler(rotation);
                    Instantiate(Wall, positionOfWall, rot);
                }
            }
            else if (direction.z == 1)
            {
                
                for (int i = 0; i < wallsToBuild; i++)
                {
                    positionOfWall = new Vector3(OffsetToOldPoint.x, OffsetToOldPoint.y, (OffsetToOldPoint.z+4) + (i * 4));
                    Vector3 rotation = new Vector3(0, 90, 0);
                    Quaternion rot = Quaternion.Euler(rotation);
                    Instantiate(Wall, positionOfWall, rot);
                }
            }
            
        }
        if(direction.z==1|| direction.z == -1 || direction.x == 1 || direction.x == -1)
        {
            OffsetToOldPoint = pos;
        }
        else
        {
            Vertices.RemoveAt(Vertices.Count - 1);
            Debug.Log("Walls Must Be 90 degree order");
        }
        //Debug.Log(Vertices.Count);
    }

}
