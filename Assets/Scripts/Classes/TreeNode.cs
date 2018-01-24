using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode<Cube> {

    private readonly Cube _cube;
    private readonly int _level;
    private readonly TreeNode<Cube> _parent;
    private TreeNode<Cube> _left;
    private TreeNode<Cube> _right;
    public Room room;

    public TreeNode(Cube cube) {
        _cube = cube;
        _left = null;
        _right = null;
        _level = 0;
    }

    public TreeNode(Cube cube, TreeNode<Cube> parent) : this(cube) {
        _parent = parent;
        _level = _parent != null ? _parent.Level + 1 : 0;
    }

    public int Level { get { return _level; } }
    public int Count {
        get {
            if (_cube != null) {
                return 1 + ((_left != null) ? _left.Count : 0) + ((_right != null) ? _right.Count : 0);
            }
            return 0;
        }
    }
    public bool IsRoot { get { return _parent == null; } }
    public bool IsLeaf {
        get {
            if (_cube != null)
            {
                return ((_left != null) ? _left.Count : 0) + ((_right != null) ? _right.Count : 0) == 0;
            }
            return false;
        }
    }
    public Cube SelfCube { get { return _cube; } }
    public TreeNode<Cube> LeftNode { get { return _left; } }
    public TreeNode<Cube> RightNode { get { return _right; } }

    public void Clear() {
        _left.Clear();
        _right.Clear();
    }

    public TreeNode<Cube> AddLeft(Cube value) {
        TreeNode<Cube> node = new TreeNode<Cube>(value, this);
        _left = node;

        return node;
    }

    public TreeNode<Cube> AddRight(Cube value) {
        TreeNode<Cube> node = new TreeNode<Cube>(value, this);
        _right = node;

        return node;
    }

    public bool Removeleft(TreeNode<Cube> node) {
        _left = null;
        return _left == null ? true : false;
    }

    public bool RemoveRight(TreeNode<Cube> node) {
        _right = null;
        return _right == null ? true : false;
    }

    public int DistanceTo(TreeNode<Cube> node) {
        return 0;
    }
}