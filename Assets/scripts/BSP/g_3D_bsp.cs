using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class g_3D_bsp : MonoBehaviour {

    [Header("NOTE: 'Y' is up you doofus")]
    [Space(10)]
    [Header("Cave size")]
    public int caveX = 1000;
    public int caveY = 1000;
    public int caveZ = 1000;
    [Space(10)]
    [Header("Max Section Size")]
    [Range(1, 1000)] public int maxSectionY = 250;
    [Range(1, 1000)] public int maxSectionX = 250;
    [Range(1, 1000)] public int maxSectionZ = 250;
    [Space(10)]
    [Header("Min Section Size")]
    [Range(1, 1000)] public int minSectionY = 100;
    [Range(1, 1000)] public int minSectionX = 100;
    [Range(1, 1000)] public int minSectionZ = 100;
    [Space(10)]
    [Header("Min Room Size")]
    [Range(1, 1000)] public int minRoomY = 60;
    [Range(1, 1000)] public int minRoomX = 60;
    [Range(1, 1000)] public int minRoomZ = 60;
    [Space(10)]
    [Header("Min Distance from section to room")]
    [Range(1, 1000)] public int sectionToRoomPadding = 20;

    struct Cube {
        public int x, y, z, disX, disY, disZ;
        public Cube(int x, int y, int z, int disX, int disY, int disZ) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.disX = disX;
            this.disY = disY;
            this.disZ = disZ;
        }
    }

    // Use this for initialization
    void Start () {
        Cube cave = new Cube(0, 0, 0, caveX, caveY, caveZ);
        DrawCube(cave, Color.green);
        GenerateCutRec(cave);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void GenerateCutRec(Cube section)
    {
        bool canCutX = CanCutInX(section);
        bool canCutY = CanCutInY(section);
        bool canCutZ = CanCutInZ(section);
        
        //can cut in the three dimensions?
        if (canCutX && canCutY && canCutZ)
        {
            int axis = Random3();
            if (axis == 1) {
                
            }
            else if (axis == 2) { }
            else if (axis == 3) { }
            else {
                Debug.LogError("Invalid Random int");
            }
        }
        else if (canCutX && canCutY) { }
        else if (canCutX && canCutZ) { }
        else if (canCutY && canCutZ) { }
        else if (canCutX) { }
        else if (canCutY) { }
        else if (canCutZ) { }
        else { GenerateRoom(section); }
    }

    void GenerateRoom(Cube section) { }

    bool CanCutInX(Cube section)
    {
        int distance = section.disX - section.x;
        if (distance <= maxSectionX) {
            return false;
        }
        return true;
    }

    bool CanCutInY(Cube section)
    {
        int distance = section.disY - section.y;
        if (distance <= maxSectionY)
        {
            return false;
        }
        return true;
    }

    bool CanCutInZ(Cube section)
    {
        int distance = section.disZ - section.z;
        if (distance <= maxSectionZ)
        {
            return false;
        }
        return true;
    }

    void GenerateCube(Cube c) {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Vector3 midpoint = new Vector3(c.x + (c.disX / 2), c.y + (c.disY / 2), c.z + (c.disZ / 2));

        cube.transform.position = midpoint;
        cube.transform.localScale = new Vector3(c.disX, c.disY, c.disZ);
    }

    void DrawCube(Cube c, Color color) {
        Debug.DrawLine(new Vector3(c.x, c.y, c.z), new Vector3(c.disX, c.y, c.z), color, 100, false);
        Debug.DrawLine(new Vector3(c.x, c.y, c.z), new Vector3(c.x, c.disY, c.z), color, 100, false);
        Debug.DrawLine(new Vector3(c.x, c.y, c.z), new Vector3(c.x, c.y, c.disZ), color, 100, false);
        Debug.DrawLine(new Vector3(c.x, c.disY, c.disZ), new Vector3(c.disX, c.disY, c.disZ), color, 100, false);
        Debug.DrawLine(new Vector3(c.disX, c.y, c.disZ), new Vector3(c.disX, c.disY, c.disZ), color, 100, false);
        Debug.DrawLine(new Vector3(c.disX, c.disY, c.z), new Vector3(c.disX, c.disY, c.disZ), color, 100, false);
        Debug.DrawLine(new Vector3(c.x, c.disY, c.z), new Vector3(c.disX, c.disY, c.z), color, 100, false);
        Debug.DrawLine(new Vector3(c.disX, c.disY, c.z), new Vector3(c.disX, c.y, c.z), color, 100, false);
        Debug.DrawLine(new Vector3(c.disX, c.y, c.z), new Vector3(c.disX, c.y, c.disZ), color, 100, false);
        Debug.DrawLine(new Vector3(c.x, c.disY, c.z), new Vector3(c.x, c.disY, c.disZ), color, 100, false);
        Debug.DrawLine(new Vector3(c.x, c.disY, c.disZ), new Vector3(c.x, c.y, c.disZ), color, 100, false);
        Debug.DrawLine(new Vector3(c.x, c.y, c.disZ), new Vector3(c.disX, c.y, c.disZ), color, 100, false);
    }

    void DrawCut(int axis, int value, int p1, int p2, int size1, int size2, Color color) {
        if (axis == 1)
        {
            //cut with an x value
            for (int i = p1; i < size1; i+=5)
            {
                Debug.DrawLine(new Vector3( value, i, p2), new Vector3( value, i, size2), color, 100, false);
            }
        }
        else if (axis == 2)
        {
            //cut with a y value
            for (int i = p1; i < size1; i+=5)
            {
                Debug.DrawLine(new Vector3(i, value, p2), new Vector3(i, value, size2), color, 100, false);
            }
        }
        else if (axis == 3)
        {
            //cut with a z value
            for (int i = p1; i < size1; i+=5)
            {
                Debug.DrawLine(new Vector3(p2, i, value), new Vector3(size2, i, value), color, 100, false);
            }
        }
        else
        {
            Debug.LogError("Invalid axis number");
        }
    }


    bool RandomBool()
    {
        if (Random.value >= 0.5)
        {
            return true;
        }
        return false;
    }

    int Random3()
    {
        return Random.Range(1, 3);
    }
}
