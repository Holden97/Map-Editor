//Author：GuoYiBo
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Person))]
public class PersonInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Label("!!!!");
        GUILayout.Button("Button");
    }
}
