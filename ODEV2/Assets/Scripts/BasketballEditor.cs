using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Basketball))]
public class BasketballEditor : Editor
{
    float size = 2f;
    private void OnSceneGUI()
    {
        Basketball ball = target as Basketball;
        EditorGUI.BeginChangeCheck();
        Quaternion rot = Handles.RotationHandle(ball.rotation, ball.transform.position);
       
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(ball, "Rotated RotateAt Point");
            ball.rotation = rot;
            
            ball.UpdateBallRotation();
        }
        if (Event.current.type==EventType.Repaint)
        {
            Transform transform = ((Basketball)target).transform;
            Handles.color = Handles.zAxisColor;
            Handles.ArrowHandleCap(0, transform.position + new Vector3(0, 0, 0f), transform.rotation * Quaternion.LookRotation(Vector3.forward), size, EventType.Repaint);
        }
        
        Handles.color = Color.cyan;
        float hizScale=Handles.ScaleSlider(ball.Hiz, ball.transform.position, ball.transform.forward, ball.transform.rotation, HandleUtility.GetHandleSize(ball.transform.position), 0.1f);
        
        ball.Hiz = hizScale;
        Handles.Label(ball.transform.position + (Vector3.up * 1.25f), string.Format("Açı: {0}\nHız: {1}", ball.transform.rotation.eulerAngles.x,ball.Hiz));
    }
}
