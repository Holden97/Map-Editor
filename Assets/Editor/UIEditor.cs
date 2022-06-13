//Author：GuoYiBo
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TestUI))]
public class UIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();
        base.OnInspectorGUI();
        GUILayout.Button("创建对象");
        //TestUI test = (TestUI)target;
        //if (GUILayout.Button("创建对象"))
        //{
        //    test.CreateObject();
        //    Debug.Log("创建对象！");
        //}
    }
}
