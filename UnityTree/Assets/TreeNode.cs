using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEditor.Analytics;

[Serializable]
public class TreeNode<T>
{

    public delegate bool TraversalDataDelegate(T data);
    public delegate bool TraversalNodeDelegate(TreeNode<T> node);

    private T _data;
    private TreeNode<T> _parent;
    private int _level;
    private List<TreeNode<T>> _children;

    public TreeNode(T data)
    {
        _data = data;
        _children = new List<TreeNode<T>>();
        _level = 0;
    }

    public TreeNode(T data, TreeNode<T> parent) : this(data)
    {
        _parent = parent;
        _level = _parent != null ? _parent.Level + 1 : 0;
    }

    public int Level { get { return _level; } }
    public int Count { get { return _children.Count; } }
    public bool IsRoot { get { return _parent == null; } }
    public bool IsLeaf { get { return _children.Count == 0; } }
    public T Data { get { return _data; } }
    public TreeNode<T> Parent { get { return _parent; } }

    public TreeNode<T> this[int key]
    {
        get { return _children[key]; }
    }

    public void Clear()
    {
        _children.Clear();
    }

    public TreeNode<T> AddChild(T value)
    {
        TreeNode<T> node = new TreeNode<T>(value, this);
        _children.Add(node);
        node._parent = this;
        node._level = this._level + 1;
        return node;
    }

    public bool HasChild(T data)
    {
        return FindInChildren(data) != null;
    }

    public TreeNode<T> FindInChildren(T data)
    {
        int i = 0, l = Count;
        for (; i < l; ++i)
        {
            TreeNode<T> child = _children[i];
            if (child.Data.Equals(data)) return child;
        }

        return null;
    }

    public bool RemoveChild(TreeNode<T> node)
    {
        return _children.Remove(node);
    }

    public void Traverse(TraversalDataDelegate handler)
    {
        if (handler(_data))
        {
            int i = 0, l = Count;
            for (; i < l; ++i) _children[i].Traverse(handler);
        }
    }

    public void Traverse(TraversalNodeDelegate handler)
    {
        if (handler(this))
        {
            int i = 0, l = Count;
            for (; i < l; ++i) _children[i].Traverse(handler);
        }
    }
    public TreeNode<T> Find(T node, TreeNode<T> root)
    {
        TreeNode<T> answer;
        if (root.Data.Equals(node))
        {
            answer = root;
            return answer;
        }
        else
        {
            foreach (var child in root._children)
            {
                var q = Find(node, child);
                if (q != null)
                {
                    return q;
                }
            }

        }

        return null;


    }
    public TreeNode<T> LCA(TreeNode<T> a, TreeNode<T> b, TreeNode<T> root)
    {

        if (a == root || b == root)
            return root;
        int count = 0;
        TreeNode<T> temp = null;
        foreach (var node in root._children)
        {
            TreeNode<T> res = LCA(a, b, node);
            if (res != null)
            {
                count++;
                temp = res;
            }
        }
        if (count == 2)
            return root;

        return temp;


    }
    


}
