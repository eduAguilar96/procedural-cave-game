using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : Cube {

    public readonly Door doorXup;
    public readonly Door doorXdown;
    public readonly Door doorYup;
    public readonly Door doorYdown;
    public readonly Door doorZup;
    public readonly Door doorZdown;
    public readonly Vector3 midPoint;
    public Door[] doors;

    public Room() {

    }

    public Room(Cube cube) {
        this.x = cube.x;
        this.y = cube.y;
        this.z = cube.z;
        this.disX = cube.disX;
        this.disY = cube.disY;
        this.disZ = cube.disZ;

        midPoint = new Vector3(x + (disX / 2.0f), y + (disY / 2.0f), z + (disZ / 2.0f));

        doorXup = new Door(x, y, z, disX, disY, disZ, Enum.Direction.xUp);
        doorXdown = new Door(x, y, z, disX, disY, disZ, Enum.Direction.xDown);
        doorYup = new Door(x, y, z, disX, disY, disZ, Enum.Direction.yUp);
        doorYdown = new Door(x, y, z, disX, disY, disZ, Enum.Direction.yDown);
        doorZup = new Door(x, y, z, disX, disY, disZ, Enum.Direction.zUp);
        doorZdown = new Door(x, y, z, disX, disY, disZ, Enum.Direction.zDown);

        doors = new Door[6];
        doors[0] = doorXup;
        doors[1] = doorXdown;
        doors[2] = doorYup;
        doors[3] = doorYdown;
        doors[4] = doorZup;
        doors[5] = doorZdown;
    }

    public void GenerateSphereAtDoors() {
        GameObject sphere1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere1.transform.position = doorXup.Midpoint;
        GameObject sphere2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere2.transform.position = doorXdown.Midpoint;
        GameObject sphere3 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere3.transform.position = doorYup.Midpoint;
        GameObject sphere4 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere4.transform.position = doorYdown.Midpoint;
        GameObject sphere5 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere5.transform.position = doorZup.Midpoint;
        GameObject sphere6 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere6.transform.position = doorZdown.Midpoint;
    }

    public void GenerateCube()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        cube.transform.position = midPoint;
        cube.transform.localScale = new Vector3(disX, disY, disZ);
    }

    public void DrawPathLine(Room room) {
        float min = 1000;
        int imin = 0;
        int jmin = 0;

        //Debug.Log(midPoint);
        //Debug.Log(room.MidPoint);

        for (int i = 0; i < 6; i++) {
            for (int j = 0; j < 6; j++) {
                float distance = Vector3.Distance(doors[i].Midpoint, room.doors[j].Midpoint);
                if (distance < min){
                    min = distance;
                    imin = i;
                    jmin = j;
                }
            }
        }
        Debug.DrawLine(doors[imin].Midpoint, room.doors[jmin].Midpoint, Color.green, 3600, true);
    }

    //This method is required by the IComparable
    //interface. 
    public int CompareTo(Room other)
    {
        if (other == null)
        {
            return 1;
        }

        //Return the difference in power.
        return 1;
    }

}
