using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class g_2D_bsp : MonoBehaviour {

    public int caveSize = 1000;
    [Range(1, 1000)]
    public int maxSeccionSize = 250;
    [Range(1, 1000)]
    public int minSeccionSize = 100;
    [Range(1, 1000)]
    public int minRoomSize = 60;
    [Range(1, 1000)]
    public int sectionToRoomPadding = 20;

    struct Square {
        public int x, y, width, height;
        public Square(int x, int y, int width, int height) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
    }
	// Use this for initialization
	void Start () {
        CheckPublicVariables();
        GenerateSplit(0, 0, caveSize, caveSize);
        Debug.DrawLine(Vector3.zero, new Vector3(caveSize, 0, 0), Color.red, 100, false);
        Debug.DrawLine(Vector3.zero, new Vector3(0, caveSize, 0), Color.red, 100, false);
        Debug.DrawLine(new Vector3(caveSize, 0, 0), new Vector3(caveSize, caveSize, 0), Color.red, 100, false);
        Debug.DrawLine(new Vector3(0, caveSize, 0), new Vector3(caveSize, caveSize, 0), Color.red, 100, false);
    }
	
	// Update is called once per frame
	void Update () {
    }

    void CheckPublicVariables()
    {
        if (maxSeccionSize <= minSeccionSize)
        {
            Debug.LogError("Invalid Max and Min cave size values");
        }

    }

    void GenerateSplit(int x1, int y1, int x2, int y2 ) {

        //can cut in both axis?
        if (CanCutInDimension(x1, x2) && CanCutInDimension(y1, y2)) {
            //use random
            if (RandomBool())
            {
                //cut in x
                CutAndCallGenerateSplit(x1, y1, x2, y2, true);
            }
            else
            {
                //cut in y
                CutAndCallGenerateSplit(x1, y1, x2, y2, false);
            }
        }
        //can only cut in x?
        else if (CanCutInDimension(x1, x2))
        {
            //cut in x
            CutAndCallGenerateSplit(x1, y1, x2, y2, true);
        }
        //can only cut in y?
        else if (CanCutInDimension(y1, y2))
        {
            //cut in y
            CutAndCallGenerateSplit(x1, y1, x2, y2, false);
        }
        else
        {
            GenerateRoom(x1, y1, x2, y2);
        }
    }

    void CutAndCallGenerateSplit(int x1, int y1, int x2, int y2, bool cutX)
    {
        if (cutX)
        {
            int cutx = Random.Range(x1 + minSeccionSize, x2 - minSeccionSize);
            Debug.DrawLine(new Vector3(cutx, y1, 0), new Vector3(cutx, y2, 0), Color.red, 100, true);
            GenerateSplit(x1, y1, cutx, y2);
            GenerateSplit(cutx, y1, x2, y2);
        }
        else
        {
            int cuty = Random.Range(y1 + minSeccionSize, y2 - minSeccionSize);
            Debug.DrawLine(new Vector3(x1, cuty, 0), new Vector3(x2, cuty, 0), Color.red, 100, true);
            GenerateSplit(x1, y1, x2, cuty);
            GenerateSplit(x1, cuty, x2, y2);
        }
    }

    void GenerateRoom(int x1, int y1, int x2, int y2)
    {
        int width = Random.Range(minRoomSize, x2 - x1 - 2 * sectionToRoomPadding);
        int height = Random.Range(minRoomSize, y2 - y1 - 2 * sectionToRoomPadding);

        int x = Random.Range(x1 + sectionToRoomPadding, x2 - sectionToRoomPadding - width);
        int y = Random.Range(y1 + sectionToRoomPadding, y2 - sectionToRoomPadding - height);

        Square aux = new Square(x, y, width, height);

        GenerateSquare(aux);

    }

    bool CanCutInDimension(int coor1, int coor2) {
        int length = coor2 - coor1;
        if (length <= maxSeccionSize + minSeccionSize) {
            return false;
        }
        return true;
    }

    bool RandomBool() {
        if (Random.value >= 0.5) {
            return true;
        }
        return false;
    }

    void GenerateSquare(Square square) {

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(square.x + (square.width / 2), square.y + (square.height / 2), 0);
        cube.transform.localScale = new Vector3(square.width, square.height, 50);

    }

    void DrawSquare(Square square)
    {
        Debug.DrawLine(new Vector3(square.x, square.y, 0), new Vector3(square.x + square.width, square.y, 0), Color.green, 100, false);
        Debug.DrawLine(new Vector3(square.x, square.y, 0), new Vector3(square.x, square.y + square.height, 0), Color.green, 100, false);
        Debug.DrawLine(new Vector3(square.x + square.width, square.y, 0), new Vector3(square.x + square.width, square.y + square.height, 0), Color.green, 100, false);
        Debug.DrawLine(new Vector3(square.x, square.y + square.height, 0), new Vector3(square.x + square.width, square.y + square.height, 0), Color.green, 100, false);
    }
}
