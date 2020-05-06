using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPlane : MonoBehaviour
{
    //public float width, height;
    public Vector3[] koseler;
    private MeshFilter meshFilter;
    public Vector3 vector3 { get => NormalVector(); }
    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;
        
        mesh.vertices = koseler;
        int[] tri = new int[6];
        tri[0] = 0;
        tri[1] = 2;
        tri[2] = 1;
        tri[3] = 2;
        tri[4] = 3;
        tri[5] = 1;
        mesh.triangles = tri;
        Vector3[] normals = new Vector3[4];
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -Vector3.forward;
        }
        mesh.normals = normals;
        Vector2[] uv = new Vector2[4];
        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(1, 0);
        uv[2] = new Vector2(0, 1);
        uv[3] = new Vector2(1, 1);

        mesh.uv = uv;
    }
    private Vector3 NormalVector()
    {
        return Vector3.Cross(koseler[1] - koseler[0], koseler[2] - koseler[0]).normalized;
    }

   
}
