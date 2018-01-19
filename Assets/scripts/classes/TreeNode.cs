using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode<T> {

    private readonly T _self;
    private readonly int _level;
    private readonly TreeNode<T> _parent;
    private TreeNode<T> _left;
    private TreeNode<T> _right;

    public TreeNode(T self) {
        _self = self;
        _left = null;
        _right = null;
        _level = 0;
    }

    public TreeNode(T self, TreeNode<T> parent) : this(self) {
        _parent = parent;
        _level = _parent != null ? _parent.Level + 1 : 0;
    }

    public int Level { get { return _level; } }
    public int Count {
        get {
            if (_self != null) {
                return 1 + ((_left != null) ? _left.Count : 0) + ((_right != null) ? _right.Count : 0);
            }
            return 0;
        }
    }
    public bool IsRoot { get { return _parent == null; } }
    public bool IsLeaf { get { return _left.Count + _right.Count == 0; } }
    public T Self { get { return _self; } }
    public TreeNode<T> LeftNode { get { return _left; } }
    public TreeNode<T> RightNode { get { return _right; } }

    public void Clear() {
        _left.Clear();
        _right.Clear();
    }

    public TreeNode<T> AddLeft(T value) {
        TreeNode<T> node = new TreeNode<T>(value, this);
        _left = node;

        return node;
    }

    public TreeNode<T> AddRight(T value) {
        TreeNode<T> node = new TreeNode<T>(value, this);
        _right = node;

        return node;
    }

    public bool Removeleft(TreeNode<T> node) {
        _left = null;
        return _left == null ? true : false;
    }

    public bool RemoveRight(TreeNode<T> node) {
        _right = null;
        return _right == null ? true : false;
    }
}