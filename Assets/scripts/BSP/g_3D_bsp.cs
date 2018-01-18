using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class g_3D_bsp : MonoBehaviour {

    [Header("NOTE: 'Y' is up you doofus")]
    [Space(10)]
    [Header("Cave size")]
    [Range(1, 1000)] public int caveX = 1000;
    [Range(1, 1000)] public int caveY = 1000;
    [Range(1, 1000)] public int caveZ = 1000;
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
        DrawCube(cave, Color.yellow);
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
            if (axis == 1) { CutX(section); }
            else if (axis == 2) { CutY(section); }
            else if (axis == 3) { CutZ(section); }
            else { Debug.LogError("Invalid Random int"); }
        }
        else if (canCutX && canCutY)
        {
            if (RandomBool()) { CutX(section); }
            else { CutY(section); }
        }
        else if (canCutX && canCutZ)
        {
            if (RandomBool()) { CutX(section); }
            else { CutZ(section); }
        }
        else if (canCutY && canCutZ)
        {
            if (RandomBool()) { CutY(section); }
            else { CutZ(section); }
        }
        else if (canCutX) { CutX(section); }
        else if (canCutY) { CutY(section); }
        else if (canCutZ) { CutZ(section); }
        else { GenerateRoom(section); }
    }

    void GenerateRoom(Cube section) {
        int distanceX = Random.Range(minRoomX, section.disX - section.x - 2 * sectionToRoomPadding);
        int distanceY = Random.Range(minRoomY, section.disY - section.y - 2 * sectionToRoomPadding);
        int distanceZ = Random.Range(minRoomZ, section.disZ - section.z - 2 * sectionToRoomPadding);

        int coordenateX = Random.Range(section.x + sectionToRoomPadding, section.disX - sectionToRoomPadding - distanceX);
        int coordenateY = Random.Range(section.y + sectionToRoomPadding, section.disY - sectionToRoomPadding - distanceY);
        int coordenateZ = Random.Range(section.z + sectionToRoomPadding, section.disZ - sectionToRoomPadding - distanceZ);

        Cube aux = new Cube(coordenateX, coordenateY, coordenateZ, distanceX, distanceY, distanceZ);
        GenerateCube(aux);
    }

    void CutX(Cube section) {
        int value = Random.Range(section.x + minSectionX, section.disX - minSectionX);
        DrawCut(1, value, section.y, section.z, section.disY, section.disZ, Color.red);

        Cube sectionLeft = new Cube(section.x, section.y, section.z, value, section.disY, section.disZ);
        Cube sectionRight = new Cube(value, section.y, section.z, section.disX, section.disY, section.disZ);

        GenerateCutRec(sectionLeft);
        GenerateCutRec(sectionRight);
    }

    void CutY(Cube section)
    {
        int value = Random.Range(section.y + minSectionY, section.disY - minSectionY);
        DrawCut(2, value, section.x, section.z, section.disX, section.disZ, Color.green);

        Cube sectionLeft = new Cube(section.x, section.y, section.z, section.disX, value, section.disZ);
        Cube sectionRight = new Cube(section.x, value, section.z, section.disX, section.disY, section.disZ);

        GenerateCutRec(sectionLeft);
        GenerateCutRec(sectionRight);
    }

    void CutZ(Cube section)
    {
        int value = Random.Range(section.z + minSectionZ, section.disZ - minSectionZ);
        DrawCut(3, value, section.x, section.y, section.disX, section.disY, Color.blue);

        Cube sectionLeft = new Cube(section.x, section.y, section.z, section.disX, section.disY, value);
        Cube sectionRight = new Cube(section.x, section.y, value, section.disX, section.disY, section.disZ);

        GenerateCutRec(sectionLeft);
        GenerateCutRec(sectionRight);
    }

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
        Debug.DrawLine(new Vector3(c.x, c.y, c.z), new Vector3(c.disX, c.y, c.z), color, 3600, false);
        Debug.DrawLine(new Vector3(c.x, c.y, c.z), new Vector3(c.x, c.disY, c.z), color, 3600, false);
        Debug.DrawLine(new Vector3(c.x, c.y, c.z), new Vector3(c.x, c.y, c.disZ), color, 3600, false);
        Debug.DrawLine(new Vector3(c.x, c.disY, c.disZ), new Vector3(c.disX, c.disY, c.disZ), color, 3600, false);
        Debug.DrawLine(new Vector3(c.disX, c.y, c.disZ), new Vector3(c.disX, c.disY, c.disZ), color, 3600, false);
        Debug.DrawLine(new Vector3(c.disX, c.disY, c.z), new Vector3(c.disX, c.disY, c.disZ), color, 3600, false);
        Debug.DrawLine(new Vector3(c.x, c.disY, c.z), new Vector3(c.disX, c.disY, c.z), color, 3600, false);
        Debug.DrawLine(new Vector3(c.disX, c.disY, c.z), new Vector3(c.disX, c.y, c.z), color, 3600, false);
        Debug.DrawLine(new Vector3(c.disX, c.y, c.z), new Vector3(c.disX, c.y, c.disZ), color, 3600, false);
        Debug.DrawLine(new Vector3(c.x, c.disY, c.z), new Vector3(c.x, c.disY, c.disZ), color, 3600, false);
        Debug.DrawLine(new Vector3(c.x, c.disY, c.disZ), new Vector3(c.x, c.y, c.disZ), color, 3600, false);
        Debug.DrawLine(new Vector3(c.x, c.y, c.disZ), new Vector3(c.disX, c.y, c.disZ), color, 3600, false);
    }

    void DrawCut(int axis, int value, int p1, int p2, int size1, int size2, Color color) {
        int lineInterval = 5;
        if (axis == 1)
        {
            //cut with an x value
            for (int i = p1; i < size1; i+= lineInterval)
            {
                Debug.DrawLine(new Vector3( value, i, p2), new Vector3( value, i, size2), color, 5, false);
            }
        }
        else if (axis == 2)
        {
            //cut with a y value
            for (int i = p1; i < size1; i+= lineInterval)
            {
                Debug.DrawLine(new Vector3(i, value, p2), new Vector3(i, value, size2), color, 5, false);
            }
        }
        else if (axis == 3)
        {
            //cut with a z value
            for (int i = p1; i < size1; i+= lineInterval)
            {
                Debug.DrawLine(new Vector3(i, p2, value), new Vector3(i, size2, value), color, 5, false);
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
