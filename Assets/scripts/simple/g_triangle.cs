using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class g_triangle : MonoBehaviour {

    [SerializeField] float speed = 90f;
    Mesh mesh;

    // Use this for initialization
    void Start () {
        mesh = new Mesh();

        //assign points
        Vector3[] vertices = new Vector3[3];
        vertices[0] = new Vector3(-1, -1, 0);
        vertices[1] = new Vector3(0, 0.8f, 0);
        vertices[2] = new Vector3(1, -1, -0);
        mesh.vertices = vertices;

        //assign triangles
        int[] triangles = new int[3] { 0, 1, 2 };
        mesh.triangles = triangles;

        GetComponent<MeshFilter>().mesh = mesh;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
