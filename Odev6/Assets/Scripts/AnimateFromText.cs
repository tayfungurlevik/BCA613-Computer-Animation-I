using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Text.RegularExpressions;
using UnityEngine;

public class AnimateFromText : MonoBehaviour
{
    public TextAsset AnimationFile;
    public int FPS = 30;
    public bool isAnimating = true;
    private float delta;
    private TreeNode<Transform> bones;
    string[] lines;
    // Start is called before the first frame update
    void Start()
    {
        string fs = AnimationFile.text;
        lines = Regex.Split(fs, "\n|\r\n");
        bones = GetComponentInChildren<BoneTree>().root;
        delta = 1 / (float)FPS;
        StartCoroutine(Animate(delta));
    }

    private IEnumerator Animate(float timeStep)
    {
        while(isAnimating)
        {
            for (int j = 0; j < lines.Length / 15; j++)
            {
                for (int i = 0; i <  15; i++)
                {
                    var values = lines[i+(j*15)].Split(';');
                    string boneName = values[0];
                    float xAngle = float.Parse(values[1]);
                    float yAngle = float.Parse(values[2]);
                    float zAngle = float.Parse(values[3]);
                    var boneToRotate = bones.Find(boneName, bones);
                    boneToRotate.Data.rotation = Quaternion.Euler(xAngle, yAngle, zAngle);
                }
                yield return new WaitForSeconds(timeStep);
            }
            
            
        }
    }
}
