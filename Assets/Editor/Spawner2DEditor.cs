using System;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(Spawner2D))]

public class Spawner2DEditor : Editor
{
    SerializedProperty prefab;
    SerializedProperty radius;
    SerializedProperty count;

    bool foldSetup = true;
    bool foldRuntime = true;

    private void OnEnable()
    {
        prefab = serializedObject.FindProperty("prefab");
        radius = serializedObject.FindProperty("radius");
        count = serializedObject.FindProperty("count");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var spawner = (Spawner2D)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Spanwer2D", EditorStyles.boldLabel);

        //FoldoutSetup
        foldSetup = EditorGUILayout.BeginFoldoutHeaderGroup(foldSetup, "Setup");

        if (foldSetup)
        {
            EditorGUILayout.PropertyField(prefab);

            if (prefab.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("Nessun prefab, inseriscine uno per lo spawn", MessageType.Warning);
            }
            EditorGUILayout.Slider(radius, 0.5f, 50f, new GUIContent("Radius"));
            EditorGUILayout.IntSlider(count, 1, 100, new GUIContent("Count"));
        }
        EditorGUI.EndFoldoutHeaderGroup();

        EditorGUILayout.Space();

        //FoldoutInRuntime

        foldRuntime = EditorGUILayout.BeginFoldoutHeaderGroup(foldRuntime, "Runtime");
        if (foldRuntime)
        {
            GUI.enabled = prefab.objectReferenceValue != null;

            if (GUILayout.Button("Spawn Now", GUILayout.Height(30)))
            {
                Spawn(spawner);
            }

            if (GUILayout.Button("Clear", GUILayout.Height(25)))
            {
                Clear(spawner);
            }
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        serializedObject.ApplyModifiedProperties();


    }

    private void Spawn(Spawner2D spawner)
    {
        if (spawner.prefab == null) return;

        if (spawner.root == null)
        {
            var rootObj = new GameObject("SpawnedObject");
            rootObj.transform.SetParent(spawner.transform);
            rootObj.transform.localPosition = Vector3.zero;
            spawner.root = rootObj.transform;
        }
        for (int i = 0; i < spawner.count; i++)
        {
            float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2);
            Vector3 pos = spawner.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * spawner.radius;

            Instantiate(spawner.prefab, pos, Quaternion.identity, spawner.root);
        }
    }

    private void Clear(Spawner2D spawner)
    {
        if (spawner.root == null) return;

        for (int i = spawner.root.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(spawner.root.GetChild(i).gameObject);
        }
    }
    private void OnSceneGUI()
    {
        Spawner2D spawner= (Spawner2D)target;

        EditorGUI.BeginChangeCheck();
        float newRadius=Handles.RadiusHandle(Quaternion.identity, spawner.transform.position,spawner.radius);

        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spawner, "Change Radius");
            spawner.radius= newRadius;
        }
    }
}
