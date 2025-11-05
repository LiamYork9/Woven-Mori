using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerInfoSaver)), CanEditMultipleObjects]
public class PlayerPrefsEditor : Editor
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlayerInfoSaver script = (PlayerInfoSaver)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Delete Saves"))
        {
            script.DeletePrefs();
        }
    }
}

