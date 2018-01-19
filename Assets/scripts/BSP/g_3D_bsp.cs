using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
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

    enum Axis {x,y,z}
    TreeNode<Cube> rootSections;
    TreeNode<Cube> rootRooms;

    IEnumerator coroutine;

    // Use this for initialization
    void Start () {
        Cube cave = new Cube(0, 0, 0, caveX, caveY, caveZ);
        rootSections = new TreeNode<Cube>(cave);

        GenerateCutRec(cave, rootSections);
        Debug.Log(rootSections.Count);
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space")) {
            Debug.Log("Startign Post-Order traversal");
            StartCoroutine(PostOrderTraversal(rootSections));
        }

	}

    /* GenerateCutRec

    determines if it can cut a section(Cube) in either axis[x,y,z],
        if multiple axis can be cut, uses random.

    parameters:
        Cube section: section being proccesd
        TreeNode<Cube>: reference to BSP tree
    
    return:
        void

    recursive: 
        calls methods for either axis being cut which
        each call GenerateCubeRec 2 times.
    */
    void GenerateCutRec(Cube section, TreeNode<Cube> node){

        bool canCutX = CanCutInAxis(section, Axis.x);
        bool canCutY = CanCutInAxis(section, Axis.y);
        bool canCutZ = CanCutInAxis(section, Axis.z);

        //can cut in the three dimensions?
        if (canCutX && canCutY && canCutZ){
            //random pick
            Axis axis = RandomAxis();
            if (axis == Axis.x) { CutAxis(section, Axis.x, node); }
            else if (axis == Axis.y) { CutAxis(section, Axis.y, node); }
            else if (axis == Axis.z) { CutAxis(section, Axis.z, node); }
            else { Debug.LogError("Invalid Random Axis"); }
        }else if (canCutX && canCutY){
            //can cut in x and y? random pick
            if (RandomBool()) { CutAxis(section, Axis.x, node); }
            else { CutAxis(section, Axis.y, node); }
        }else if (canCutX && canCutZ){
            //can cut in x and z? random pick
            if (RandomBool()) { CutAxis(section, Axis.x, node); }
            else { CutAxis(section, Axis.z, node); }
        }else if (canCutY && canCutZ){
            //can cut in y and z? random pick
            if (RandomBool()) { CutAxis(section, Axis.y, node); }
            else { CutAxis(section, Axis.z, node); }
        }//can only cut in one, do it
        else if (canCutX) { CutAxis(section, Axis.x, node); }
        else if (canCutY) { CutAxis(section, Axis.y, node); }
        else if (canCutZ) { CutAxis(section, Axis.z, node); }
        else { GenerateRoom(section); }
    }

    /* CutAxis
    calculates random value in [X,Y,Z] axis to cut section(Cube)
        creating 2 sections(Cube s) and call GenerateCutRec

    parameters:
        Cube section: section(Cube) being considered for cut
        Axis axis: axis in which we are performing a cut
        TreeNode<Cube>: reference to BSP tree

    return:
        void  
    */
    void CutAxis(Cube section, Axis axis, TreeNode<Cube> node) {
        int value;
        Cube sectionLeft = null;
        Cube sectionRight = null;

        if (axis == Axis.x) {
            value = Random.Range(section.x + minSectionX, section.disX - minSectionX);
            //DrawCut(Axis.x, value, section.y, section.z, section.disY, section.disZ, Color.red);

            sectionLeft = new Cube(section.x, section.y, section.z, value, section.disY, section.disZ);
            sectionRight = new Cube(value, section.y, section.z, section.disX, section.disY, section.disZ);
        }
        else if (axis == Axis.y) {
            value = Random.Range(section.y + minSectionY, section.disY - minSectionY);
            //DrawCut(Axis.y, value, section.x, section.z, section.disX, section.disZ, Color.green);

            sectionLeft = new Cube(section.x, section.y, section.z, section.disX, value, section.disZ);
            sectionRight = new Cube(section.x, value, section.z, section.disX, section.disY, section.disZ);
        }
        else if (axis == Axis.z) {
            value = Random.Range(section.z + minSectionZ, section.disZ - minSectionZ);
            //DrawCut(Axis.z, value, section.x, section.y, section.disX, section.disY, Color.blue);

            sectionLeft = new Cube(section.x, section.y, section.z, section.disX, section.disY, value);
            sectionRight = new Cube(section.x, section.y, value, section.disX, section.disY, section.disZ);
        }
        else {
            Debug.LogError("Invalid axis number repesentation");
        }

        node.AddLeft(sectionLeft);
        node.AddRight(sectionRight);
        GenerateCutRec(sectionLeft, node.LeftNode);
        GenerateCutRec(sectionRight, node.RightNode);
    }

    /* CanCutInAxis

    determines if we can further cut section(Cube) in the [X,Y,Z] axis,
        this taking consideration in maxSection[X,Y,Z]

    parameters:
       Cube section: section(Cube) being considered for cut
       Axis axis: axis being determined

    return:
        bool: true if we can cut
    */
    bool CanCutInAxis(Cube section, Axis axis) {

        int distance;

        if (axis == Axis.x) {
            distance = section.disX - section.x;
            if (distance <= maxSectionX) return false;
        }
        else if (axis == Axis.y){
            distance = section.disY - section.y;
            if (distance <= maxSectionY) return false;
        }
        else if (axis == Axis.z){
            distance = section.disZ - section.z;
            if (distance <= maxSectionZ) return false;
        }
        else {
            Debug.LogError("Invalid Axis");
        }
        return true;
    }

    /* DrawCut
        
    Generates a 3D plane in space composed of Debug.DrawLine gizmos
    
    parameters:
        int axis: -
        int value: axis value
        int p1: coordenate [X,Y,Z]
        int p2: coordenate [X,Y,Z]
        int size1: size[X,Y,Z]
        int size2: size[X,Y,Z]
        Color color: -
    */
    void DrawCut(Axis axis, int value, int p1, int p2, int size1, int size2, Color color) {
        int lineInterval = 5;
        if (axis == Axis.x){
            //cut with an x value
            for (int i = p1; i < size1; i+= lineInterval){
                Debug.DrawLine(new Vector3( value, i, p2), new Vector3( value, i, size2), color, 5, false);
            }
        }
        else if (axis == Axis.y){
            //cut with a y value
            for (int i = p1; i < size1; i+= lineInterval){
                Debug.DrawLine(new Vector3(i, value, p2), new Vector3(i, value, size2), color, 5, false);
            }
        }else if (axis == Axis.z){
            //cut with a z value
            for (int i = p1; i < size1; i+= lineInterval){
                Debug.DrawLine(new Vector3(i, p2, value), new Vector3(i, size2, value), color, 5, false);
            }
        }
        else{
            Debug.LogError("Invalid axis number");
        }
    }

    /* GenerateRoom 

    calculates random values for a room(Cube) which rests inside a section(Cube),
        this taking into account: minRoom[X,Y,Z] and sectionToRoomPadding
    
    parameters:
        Cube section: section(Cube) in which a room(Cube) is being generated

    return:
        void
    */
    void GenerateRoom(Cube section) {
        int distanceX = Random.Range(minRoomX, section.disX - section.x - 2 * sectionToRoomPadding);
        int distanceY = Random.Range(minRoomY, section.disY - section.y - 2 * sectionToRoomPadding);
        int distanceZ = Random.Range(minRoomZ, section.disZ - section.z - 2 * sectionToRoomPadding);

        int coordenateX = Random.Range(section.x + sectionToRoomPadding, section.disX - sectionToRoomPadding - distanceX);
        int coordenateY = Random.Range(section.y + sectionToRoomPadding, section.disY - sectionToRoomPadding - distanceY);
        int coordenateZ = Random.Range(section.z + sectionToRoomPadding, section.disZ - sectionToRoomPadding - distanceZ);

        Cube aux = new Cube(coordenateX, coordenateY, coordenateZ, distanceX, distanceY, distanceZ);
        //aux.GenerateCube();
    }

    bool RandomBool(){
        if (Random.value >= 0.5)
        {
            return true;
        }
        return false;
    }

    Axis RandomAxis(){
        int aux = Random.Range(1, 3);
        if (aux == 1) return Axis.x;
        else if (aux == 2) return Axis.y;
        else return Axis.z;
    }

    IEnumerator PostOrderTraversal(TreeNode<Cube> cube) {
        //Debug.Log("Inside");
        if (cube != null)
        {
            if (cube.Self != null)
            {
                yield return StartCoroutine(PostOrderTraversal(cube.LeftNode));
                yield return StartCoroutine(PostOrderTraversal(cube.RightNode));
                yield return new WaitForSeconds(0.1f);
                cube.Self.DrawCube(Color.yellow);
                Debug.Log(cube.Count);
            }
        }
    }
}

//TODO: Check null nodes, null cubes in nodes, in BSP tree. No consistency?
//      Find way to print tree
