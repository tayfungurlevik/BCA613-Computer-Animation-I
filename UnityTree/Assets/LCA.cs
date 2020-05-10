using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LCA : MonoBehaviour
{
    public Transform NodeA;
    public Transform NodeB;
	TreeNode<Transform> tree;
    // Start is called before the first frame update
    void Start()
    {
		tree = GetComponent<Tree>().root;
        TreeNode<Transform> A = tree.Find(NodeA,tree);
        TreeNode<Transform> B = tree.Find(NodeB, tree);
        

        var lca = tree.LCA(A, B, tree);
        
        Debug.Log(string.Format("Lowest Common Ancestor of {0} and {1} is: {2}",NodeA.gameObject.name,NodeB.gameObject.name,
        lca.Data.gameObject.name));
        Selection.activeObject = lca.Data.gameObject;
    }

    
	
}
