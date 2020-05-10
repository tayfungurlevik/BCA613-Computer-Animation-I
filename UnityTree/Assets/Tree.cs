using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public TreeNode<Transform> root;
    // Start is called before the first frame update
    void Awake()
    {
        root = new TreeNode<Transform>(transform,null);
        CreateTree(root);
        
        
    }

    

    private void CreateTree(TreeNode<Transform> root)
    {
        var childCount = root.Data.childCount;
        if (childCount==0)
        {
            return;
        }
        for (int i = 0; i < childCount; i++)
        {
            
            var ChildNode = new TreeNode<Transform>(root.Data.GetChild(i),root);
            root.AddChild(ChildNode.Data);
            CreateTree(root.FindInChildren(root.Data.GetChild(i)));
        }
        
    }
}
