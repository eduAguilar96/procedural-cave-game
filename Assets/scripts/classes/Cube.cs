using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube{

    public int x;
    public int y;
    public int z;
    public int disX;
    public int disY;
    public int disZ;

    public Cube(int x, int y, int z, int disX, int disY, int disZ)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.disX = disX;
        this.disY = disY;
        this.disZ = disZ;
    }

    public Vector3 MidPoint {
        get {
            return new Vector3(x + (disX / 2), y + (disY / 2), z + (disZ / 2));
        }
    }

    public void GenerateCube()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        cube.transform.position = MidPoint;
        cube.transform.localScale = new Vector3(disX, disY, disZ);
    }

    public void DrawCube(Color color)
    {
        Debug.DrawLine(new Vector3(x, y, z), new Vector3(disX, y, z), color, 3600, false);
        Debug.DrawLine(new Vector3(x, y, z), new Vector3(x, disY, z), color, 3600, false);
        Debug.DrawLine(new Vector3(x, y, z), new Vector3(x, y, disZ), color, 3600, false);
        Debug.DrawLine(new Vector3(x, disY, disZ), new Vector3(disX, disY, disZ), color, 3600, false);
        Debug.DrawLine(new Vector3(disX, y, disZ), new Vector3(disX, disY, disZ), color, 3600, false);
        Debug.DrawLine(new Vector3(disX, disY, z), new Vector3(disX, disY, disZ), color, 3600, false);
        Debug.DrawLine(new Vector3(x, disY, z), new Vector3(disX, disY, z), color, 3600, false);
        Debug.DrawLine(new Vector3(disX, disY, z), new Vector3(disX, y, z), color, 3600, false);
        Debug.DrawLine(new Vector3(disX, y, z), new Vector3(disX, y, disZ), color, 3600, false);
        Debug.DrawLine(new Vector3(x, disY, z), new Vector3(x, disY, disZ), color, 3600, false);
        Debug.DrawLine(new Vector3(x, disY, disZ), new Vector3(x, y, disZ), color, 3600, false);
        Debug.DrawLine(new Vector3(x, y, disZ), new Vector3(disX, y, disZ), color, 3600, false);
    }
}
