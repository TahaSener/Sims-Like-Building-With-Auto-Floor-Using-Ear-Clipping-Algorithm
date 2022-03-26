using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public Transform obj;
    Snap snap;
    Vector3 globalStartPoint;
    int countOfWalls=0;
    bool buildOn = false;
    List<WallNode> posOfSectors;
    public Transform Wall;
    private void Awake()
    {
        snap = GetComponent<Snap>();
        posOfSectors = new List<WallNode>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && !buildOn)
        {
            buildOn = true;
            globalStartPoint = snap.GetGridToSnap(snap.GetMousePos());
            posOfSectors.Add(new WallNode(globalStartPoint,Vector3.zero));
        }
        else if(Input.GetMouseButtonUp(0) && buildOn)
        {
            Vector3 pos = snap.GetGridToSnap(snap.GetMousePos());
            
            if (pos == globalStartPoint)
            {
                AddToNextSector(pos);
                buildOn = false;
                Build();
            }
            else
            {
                AddToNextSector(pos);
                Debug.Log("girdi");
                if (pos.x == posOfSectors[posOfSectors.Count-1].position.x || pos.z == posOfSectors[posOfSectors.Count-1].position.z)
                {
                    
                }
                else
                {
                   // Debug.LogError("Wall must be horizontal");
                }
                
            }
        }
        
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 pos = snap.GetGridToSnap(snap.GetMousePos());
        //    StartBuildRoutine(pos);
        //}
        //if (Input.GetMouseButton(0))
        //{
        //    Vector3 dir = (globalStartPoint - snap.GetMousePos());
        //    if (dir.magnitude > 4f * countOfWalls) 
        //    {
        //        countOfWalls++;
        //        Debug.Log(countOfWalls);
        //    }
        //    else if (dir.magnitude < 4f * countOfWalls)
        //    {
        //        countOfWalls--;
        //        Debug.Log(countOfWalls);
        //    }

            
        //}
    }

    public void StartBuildRoutine(Vector3 posOfStartPoint)
    {
        globalStartPoint = posOfStartPoint;
        countOfWalls = 0;
    }

    public void AddToNextSector(Vector3 pos)
    {
        int wallNeedToBeAdded = 0;
        Vector3 a = pos - posOfSectors[posOfSectors.Count - 1].position;
        Vector3 dir = a / a.magnitude;
        if (pos.x == posOfSectors[posOfSectors.Count-1].position.x+ Vector3.right.x*4 || pos.x == posOfSectors[posOfSectors.Count - 1].position.x + -(Vector3.right.x * 4) || pos.x == posOfSectors[posOfSectors.Count - 1].position.x)
        {
            wallNeedToBeAdded=Mathf.RoundToInt(Mathf.Abs(pos.z - posOfSectors[posOfSectors.Count-1].position.z)/4);
            Debug.Log(wallNeedToBeAdded + "Xler aynı");
            for (int i = 1; i < wallNeedToBeAdded; i++)
            {
                if (dir == Vector3.forward)
                {
                    posOfSectors.Add(new WallNode(posOfSectors[posOfSectors.Count - 1].position + Vector3.forward * 4,Vector3.up*90));
                }
                else
                {
                    posOfSectors.Add(new WallNode(posOfSectors[posOfSectors.Count - 1].position + -Vector3.forward * 4, Vector3.up * 90));
                }
                
            }
        }
        else if(pos.z == posOfSectors[posOfSectors.Count-1].position.z+Vector3.forward.z*4|| pos.z == posOfSectors[posOfSectors.Count - 1].position.z + -(Vector3.forward.z * 4)|| pos.z == posOfSectors[posOfSectors.Count - 1].position.z)
        {
            wallNeedToBeAdded = Mathf.RoundToInt(Mathf.Abs(pos.x - posOfSectors[posOfSectors.Count-1].position.x) / 4);
            Debug.Log(wallNeedToBeAdded + "Z'ler Aynı");
            for (int i = 1; i < wallNeedToBeAdded; i++)
            {
                if (dir == Vector3.right)
                {
                    posOfSectors.Add(new WallNode(posOfSectors[posOfSectors.Count - 1].position + Vector3.right * 4, Vector3.zero));
                }
                else
                {
                    posOfSectors.Add(new WallNode(posOfSectors[posOfSectors.Count - 1].position + -Vector3.right * 4, Vector3.zero));
                }
                
            }
        }
        
        
    }

    public void Build() {
        foreach (WallNode pos in posOfSectors)
        {
            Quaternion rotation = Quaternion.Euler(pos.rotation);
            Instantiate(Wall, pos.position, rotation);

        }
        posOfSectors.Clear();
    }
    
}
