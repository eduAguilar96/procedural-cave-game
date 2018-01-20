using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enum{

    public enum Direction { xUp, xDown, yUp, yDown, zUp, zDown };
    public enum Axis { x, y, z }; 
    public Axis RandomAxis { 
        get {
            int aux = Random.Range(1, 3);
            if (aux == 1) return Enum.Axis.x;
            else if (aux == 2) return Enum.Axis.y;
            else return Enum.Axis.z;
        }
    }
}
