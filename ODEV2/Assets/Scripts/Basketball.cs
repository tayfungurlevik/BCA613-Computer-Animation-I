using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Basketball : MonoBehaviour
{
    public Quaternion rotation = Quaternion.identity;
    public float Hiz = 1f;
    
    public float Cap=1f;
    public void UpdateBallRotation()
    {
        transform.rotation = rotation;
        
    }
    private void Start()
    {
        transform.localScale = new Vector3(Cap, Cap, Cap);
    }
}
