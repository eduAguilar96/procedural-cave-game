using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door {

    private readonly Enum.Direction dir;
    private readonly Vector3 midPoint;

    public Door(Vector3 midPoint, Enum.Direction dir) {
        this.midPoint = midPoint;
        this.dir = dir;
    }

    public Door(int x, int y, int z, Enum.Direction dir) {
        midPoint = new Vector3(x,y,z);
        this.dir = dir;
    }

    public Door(int x, int y, int z, int disX, int disY, int disZ, Enum.Direction dir){

        Vector3 aux = new Vector3(0,0,0);

        if (dir == Enum.Direction.xUp) {
            aux = new Vector3(x + disX, y + (disY / 2.0f), z + (disZ / 2.0f));
        }
        else if (dir == Enum.Direction.xDown) {
            aux = new Vector3(x, y + (disY / 2.0f), z + (disZ / 2.0f));
        }
        else if (dir == Enum.Direction.yUp) {
            aux = new Vector3(x + (disX / 2.0f), y + disY, z + (disZ / 2.0f));
        }
        else if (dir == Enum.Direction.yDown) {
            aux = new Vector3(x + (disX / 2.0f), y, z + (disZ / 2.0f));
        }
        else if (dir == Enum.Direction.zUp) {
            aux = new Vector3(x + (disX / 2.0f), y + (disY / 2.0f), z + disZ);
        }
        else if (dir == Enum.Direction.zDown) {
            aux = new Vector3(x + (disX / 2.0f), y + (disY / 2.0f), z);
        }

        midPoint = aux;
        this.dir = dir;
    }

    public Enum.Direction Direction { get { return dir; } }
    public Vector3 Midpoint { get { return midPoint; } }
}
