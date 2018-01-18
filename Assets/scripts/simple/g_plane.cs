using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class g_plane : MonoBehaviour {

    public int width = 20;
    Mesh mesh;

    // Use this for initialization
    void Start ()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GenerateTriangles();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void GenerateTriangles() {
        int[] triangles = new int[(width - 1) * (width - 1) * 6];

        int triangleIndex = 0;
        for (int x = 0; x < width - 1; x++) {
            for (int y = 0; y < width - 1; y++) {
                int vertexIndex = x * width + y;

                triangles[triangleIndex + 0] = vertexIndex;
                triangles[triangleIndex + 1] = vertexIndex + width;
                triangles[triangleIndex + 2] = vertexIndex + width + 1;

                triangles[triangleIndex + 3] = vertexIndex;
                triangles[triangleIndex + 4] = vertexIndex + width + 1;
                triangles[triangleIndex + 5] = vertexIndex + 1;

                triangleIndex += 6;
            }
        }

        GetComponent<MeshFilter>().mesh.triangles = triangles;
    }
}
 