using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
[ExecuteInEditMode]
public class LopeaLine : MonoBehaviour
{
    #region Variables/Accessors
    //all the nodes used for the mesh
    [SerializeField] GameObject[] nodes;
    //radius of the line
    [SerializeField] float Radius = 10;
    //the amount of vertices surrounding each node
    [SerializeField] int quality = 10;
    //all the vertices in the mesh
    Vector3[] verts;
    //store mesh
    Mesh mesh;
    //all triangle index values
    int[] triangles;
    //editor variables: only used if Unity Editor modifies values
#if UNITY_EDITOR
    int _oldtriangles;
    int _oldnodecount;
#endif
    public GameObject[] Nodes { get { return nodes; } }
    #endregion

    #region Unity functions
    void Awake()
    {
       GenerateNewMesh();
    }
    void Update()
    {
    //updating is only necessary when in the editor, not in standalone
#if UNITY_EDITOR    
        //checks if there is a node that has been added or removed
        if (_oldnodecount != nodes.Length)
    {
        //create new mesh
        GenerateNewMesh();
    }
    //checks if amount of triangles changes
    if (_oldtriangles != triangles.Length)
    {
        //create new triangle array
        UpdateTriangles();
    }
    }
#endif
    #endregion

    #region Modifiers
    void UpdateTriangles()
    {

        //reset triangle array
        triangles = new int[quality * (nodes.Length - 1) * 6];
        //update old values 
        _oldtriangles = triangles.Length;
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
                verts[i] = nodes[y].transform.TransformPoint( 
                                       new Vector3(Mathf.Sin(2 * Mathf.PI * x / quality+1) * Radius, 
                                       Mathf.Cos(2 * Mathf.PI * x/ quality+1) * Radius,
                                       0));
                
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

    /// <summary>
    /// Adds a new node at the end of the line
    /// </summary>
    /// <param name="position">The position of the new node </param>
    public void AddNode(Vector3 position)
    {
        var newArray = new GameObject[nodes.Length + 1];
        for (int i = 0; i < nodes.Length; i++)
        {
            newArray[i] = nodes[i];
        }
        newArray[nodes.Length] = new GameObject(nodes.Length.ToString());
        nodes = newArray;
        GenerateNewMesh();
    }
    
    /// <summary>
    /// Adds an array of node positions to the mesh
    /// </summary>
    /// <param name="positions">Array of node positions added to the current list of nodes</param>
    public void AddNodes(GameObject[] positions)
    {
        var newArray = new GameObject[positions.Length + nodes.Length];
        for (int i =0; i < newArray.Length; i++)
        {
            newArray[i] = (i < nodes.Length) ? nodes[i] : positions[i];
        }
        nodes = newArray;
        GenerateNewMesh();
    }
    
    /// <summary>
    /// Replaces all nodes with an array of new ones
    /// </summary>
    /// <param name="positions">The new array that replaces the old node array</param>
    public void ReplaceNodes(GameObject[] positions)
    {
        nodes = positions;
        GenerateNewMesh();
    }
    #endregion

}
