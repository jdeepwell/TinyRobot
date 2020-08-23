using System;
using UnityEditor;
using UnityEngine;

public class StaticGameManagerEditor : EditorWindow
{
   
    [MenuItem("Window/Static Game Manager Editor")]
    public static void  ShowWindow () {
        EditorWindow.GetWindow(typeof(StaticGameManagerEditor));
    }

    void OnGUI () {
        EditorGUILayout.LabelField("Game Manager", EditorStyles.boldLabel);
        if (!Application.isPlaying)
        {
            if (GUILayout.Button("Reset Game Manager"))
            {
                StaticGameManager.ResetInstance();
            }
        }

        GUILayout.Button("Refresh values"); // Actually clicking into the inspector window does suffice for that.
        if (!StaticGameManager.Exists())
        {
            GUILayout.Label("No Game Manager...");
            return;
        }

        if (Application.isPlaying)
        {
            StaticGameManager.Instance.lives.value =
                EditorGUILayout.IntField("Lives", StaticGameManager.Instance.lives.value);
            StaticGameManager.Instance.coins.value =
                EditorGUILayout.IntField("Coins", StaticGameManager.Instance.coins.value);
        }
    }
}