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
        TreeNode<Transform> A = new TreeNode<Transform>(NodeA);
        TreeNode<Transform> B = new TreeNode<Transform>(NodeB);
        Debug.Log(A.Data.gameObject.name);
        Debug.Log(B.Data.gameObject.name);
        var lca = tree.LCA(tree.Find(A, tree), tree.Find(B, tree), tree);
        //var lca2 = tree.LCA2(tree,tree.Find(A, tree), tree.Find(B, tree));
        Debug.Log(lca.Data.gameObject.name);
        //Debug.Log(lca2.Data.gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
}
