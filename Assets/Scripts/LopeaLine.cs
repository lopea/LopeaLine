using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
[ExecuteInEditMode]
public class LopeaLine : MonoBehaviour
{
    [SerializeField] Vector3[] nodes = { new Vector3(0,0,0), new Vector3(0,0,1)};
    [SerializeField] float Radius = 10;
    [SerializeField] int quality = 10;
    Vector3[] verts;
    Mesh mesh;
    int[] triangles;

    // Start is called before the first frame update
    void Start()
    {
       GenerateNewMesh();
        
    }
    void UpdateTriangles()
    {
        //reset triangle array
        triangles = new int[quality * (nodes.Length - 1) * 6];
        
        for (int i = 0, y = 0, i0 = 0; y < nodes.Length - 1; y++)
        for (int x = 0; x < quality; x++, i += 6, i0++)
        {
            //Generate default triangle variables
            triangles[i] = i0;
            triangles[i + 1] = triangles[i + 4] = quality + i0;
            triangles[i + 2] = triangles[i + 3] = 1 + i0;
            triangles[i + 5] = 1 + quality + i0;

            //last few meshes need to loop back to starting values
            //override default variables if necessary
            if (x == quality - 1)
            {
                triangles[i + 3] = i0;
                triangles[i + 4] = 1 + i0;
                triangles[i + 5] = i0 - (quality - 1);
            }
        }
        //apply triangle array to mesh
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
    void UpdateVerts()
    {
        //init vert triangles
        verts = new Vector3[quality * nodes.Length];
        for (int y= 0,i = 0; y < nodes.Length; y++)
        {
            for (int x = 0; x < quality; x++, i++) {
                // create evenly spaced verts in a circle formation
                verts[i] = new Vector3(Mathf.Sin(2 * Mathf.PI * x / quality+1) * Radius, 
                                       Mathf.Cos(2 * Mathf.PI * x/ quality+1) * Radius,
                                       0) + nodes[y];
            }
        }
        //apply vert positions
        mesh.vertices = verts;
    }
    void GenerateNewMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        UpdateVerts();
        UpdateTriangles();

    }
    // Update is called once per frame
    void Update()
    {
        GenerateNewMesh();
    }
   
}
