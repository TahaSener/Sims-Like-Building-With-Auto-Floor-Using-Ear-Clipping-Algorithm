using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCreate : MonoBehaviour
{
    public Vector3[] Vertices;
    public int[] Triangles;
    Mesh mesh;
    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;


    }
    
    
    public void CreateMesh()
    {
        
        mesh.vertices = Vertices;
        mesh.triangles = Triangles;
        mesh.RecalculateNormals();

    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
