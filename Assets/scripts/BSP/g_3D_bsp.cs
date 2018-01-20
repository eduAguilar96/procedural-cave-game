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

    TreeNode<Cube> rootSections;
    ListNode<Room> headRooms;
    List<Room> listRooms;

    IEnumerator coroutine;

    // Use this for initialization
    void Start () {
        Cube cave = new Cube(0, 0, 0, caveX, caveY, caveZ);
        rootSections = new TreeNode<Cube>(cave);
        listRooms = new List<Room>();

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

        bool canCutX = CanCutInEnumAxis(section, Enum.Axis.x);
        bool canCutY = CanCutInEnumAxis(section, Enum.Axis.y);
        bool canCutZ = CanCutInEnumAxis(section, Enum.Axis.z);

        //can cut in the three dimensions?
        if (canCutX && canCutY && canCutZ){
            //random pick
            Enum.Axis axis = RandomAxis();
            if (axis == Enum.Axis.x) { CutAxis(section, Enum.Axis.x, node); }
            else if (axis == Enum.Axis.y) { CutAxis(section, Enum.Axis.y, node); }
            else if (axis == Enum.Axis.z) { CutAxis(section, Enum.Axis.z, node); }
            else { Debug.LogError("Invalid Random Enum.Axis"); }
        }else if (canCutX && canCutY){
            //can cut in x and y? random pick
            if (RandomBool()) { CutAxis(section, Enum.Axis.x, node); }
            else { CutAxis(section, Enum.Axis.y, node); }
        }else if (canCutX && canCutZ){
            //can cut in x and z? random pick
            if (RandomBool()) { CutAxis(section, Enum.Axis.x, node); }
            else { CutAxis(section, Enum.Axis.z, node); }
        }else if (canCutY && canCutZ){
            //can cut in y and z? random pick
            if (RandomBool()) { CutAxis(section, Enum.Axis.y, node); }
            else { CutAxis(section, Enum.Axis.z, node); }
        }//can only cut in one, do it
        else if (canCutX) { CutAxis(section, Enum.Axis.x, node); }
        else if (canCutY) { CutAxis(section, Enum.Axis.y, node); }
        else if (canCutZ) { CutAxis(section, Enum.Axis.z, node); }
        else {
            //Debug.Log("Recursive End");
        }
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
    void CutAxis(Cube section, Enum.Axis axis, TreeNode<Cube> node) {
        int value;
        Cube sectionLeft = null;
        Cube sectionRight = null;

        if (axis == Enum.Axis.x) {
            value = Random.Range(section.x + minSectionX, section.disX - minSectionX);
            //DrawCut(Enum.Axis.x, value, section.y, section.z, section.disY, section.disZ, Color.red);

            sectionLeft = new Cube(section.x, section.y, section.z, value, section.disY, section.disZ);
            sectionRight = new Cube(value, section.y, section.z, section.disX, section.disY, section.disZ);
        }
        else if (axis == Enum.Axis.y) {
            value = Random.Range(section.y + minSectionY, section.disY - minSectionY);
            //DrawCut(Enum.Axis.y, value, section.x, section.z, section.disX, section.disZ, Color.green);

            sectionLeft = new Cube(section.x, section.y, section.z, section.disX, value, section.disZ);
            sectionRight = new Cube(section.x, value, section.z, section.disX, section.disY, section.disZ);
        }
        else if (axis == Enum.Axis.z) {
            value = Random.Range(section.z + minSectionZ, section.disZ - minSectionZ);
            //DrawCut(Enum.Axis.z, value, section.x, section.y, section.disX, section.disY, Color.blue);

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

    /* CanCutInEnum.Axis

    determines if we can further cut section(Cube) in the [X,Y,Z] axis,
        this taking consideration in maxSection[X,Y,Z]

    parameters:
       Cube section: section(Cube) being considered for cut
       Enum.Axis axis: axis being determined

    return:
        bool: true if we can cut
    */
    bool CanCutInEnumAxis(Cube section, Enum.Axis axis) {

        int distance;

        if (axis == Enum.Axis.x) {
            distance = section.disX - section.x;
            if (distance <= maxSectionX) return false;
        }
        else if (axis == Enum.Axis.y){
            distance = section.disY - section.y;
            if (distance <= maxSectionY) return false;
        }
        else if (axis == Enum.Axis.z){
            distance = section.disZ - section.z;
            if (distance <= maxSectionZ) return false;
        }
        else {
            Debug.LogError("Invalid Enum.Axis");
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
    void DrawCut(Enum.Axis axis, int value, int p1, int p2, int size1, int size2, Color color) {
        int lineInterval = 5;
        if (axis == Enum.Axis.x){
            //cut with an x value
            for (int i = p1; i < size1; i+= lineInterval){
                Debug.DrawLine(new Vector3( value, i, p2), new Vector3( value, i, size2), color, 5, false);
            }
        }
        else if (axis == Enum.Axis.y){
            //cut with a y value
            for (int i = p1; i < size1; i+= lineInterval){
                Debug.DrawLine(new Vector3(i, value, p2), new Vector3(i, value, size2), color, 5, false);
            }
        }else if (axis == Enum.Axis.z){
            //cut with a z value
            for (int i = p1; i < size1; i+= lineInterval){
                Debug.DrawLine(new Vector3(i, p2, value), new Vector3(i, size2, value), color, 5, false);
            }
        }
        else{
            Debug.LogError("Invalid axis number");
        }
    }

    /* GenerateRoomDimensions

    calculates random values for a room(Cube) which rests inside a section(Cube),
        this taking into account: minRoom[X,Y,Z] and sectionToRoomPadding
    
    parameters:
        Cube section: section(Cube) in which a room(Cube) is being generated

    return:
        aux: generated room
    */
    Cube GenerateRoomDimensions(Cube section) {
        int distanceX = Random.Range(minRoomX, section.disX - section.x - 2 * sectionToRoomPadding);
        int distanceY = Random.Range(minRoomY, section.disY - section.y - 2 * sectionToRoomPadding);
        int distanceZ = Random.Range(minRoomZ, section.disZ - section.z - 2 * sectionToRoomPadding);

        int coordenateX = Random.Range(section.x + sectionToRoomPadding, section.disX - sectionToRoomPadding - distanceX);
        int coordenateY = Random.Range(section.y + sectionToRoomPadding, section.disY - sectionToRoomPadding - distanceY);
        int coordenateZ = Random.Range(section.z + sectionToRoomPadding, section.disZ - sectionToRoomPadding - distanceZ);

        Cube aux = new Cube(coordenateX, coordenateY, coordenateZ, distanceX, distanceY, distanceZ);
        return aux;
    }

    bool RandomBool(){
        if (Random.value >= 0.5)
        {
            return true;
        }
        return false;
    }

    Enum.Axis RandomAxis(){
        int aux = Random.Range(1, 3);
        if (aux == 1) return Enum.Axis.x;
        else if (aux == 2) return Enum.Axis.y;
        else return Enum.Axis.z;
    }

    /* PostOrderTraversal
    
    Traverses tha main BSP tree in a post order fashion,
        Generates Dungeoun
    
    parameters:
        TreeNode<Cube> sectionNode: Tree node being traversed

    return:
        IEnumerator: this is a couroutine that returns when finished
    
    */
    IEnumerator PostOrderTraversal(TreeNode<Cube> sectionNode) {
        if (sectionNode != null){
            if (sectionNode.SelfCube != null){
                yield return StartCoroutine(PostOrderTraversal(sectionNode.LeftNode));
                yield return StartCoroutine(PostOrderTraversal(sectionNode.RightNode));
                //if leaf: section has no inner sections
                if (sectionNode.IsLeaf){
                    sectionNode.room = new Room(GenerateRoomDimensions(sectionNode.SelfCube));
                    sectionNode.room.GenerateCube();
                    sectionNode.room.GenerateSphereAtDoors();
                    listRooms.Add(new Room(sectionNode.room));
                }
                //if NOT leaf, section with with inner sections
                else {
                    List<Room> listRoomLeft = new List<Room>();
                    List<Room> listRoomRight = new List<Room>();
                    CollectRooms(ref listRoomLeft, sectionNode.LeftNode);
                    CollectRooms(ref listRoomRight, sectionNode.RightNode);

                    Room selectedLeft = new Room();
                    Room selectedRight = new Room();
                    float minDistance = 10000.0f;

                    foreach (Room roomLeft in listRoomLeft) {
                        foreach (Room roomRight in listRoomRight){
                            float distance = Vector3.Distance(roomLeft.MidPoint, roomRight.midPoint);
                            if (distance <= minDistance) {
                                minDistance = distance;
                                selectedLeft = roomLeft;
                                selectedRight = roomRight;
                            }
                        }
                    }
                    selectedLeft.DrawPathLine(selectedRight);
                }
            }
        }
    }

    /* CollectRooms
    
    Traverses the BSP tree in a pre-order fashion and adds rooms to
        a list

    parameters:
        ref List<Room> list: list where rooms are being stored, passed as reference
        TreeNode<Cube> sectionNode: TreeNode being traversed

    return:
        void
    */
    void CollectRooms(ref List<Room> list, TreeNode<Cube> sectionNode) {
        if (sectionNode != null) {
            if (sectionNode.SelfCube != null) {
                if (sectionNode.IsLeaf) {
                    list.Add(sectionNode.room);
                }
                CollectRooms(ref list, sectionNode.LeftNode);
                CollectRooms(ref list, sectionNode.RightNode);
            }
        }
    }
}
