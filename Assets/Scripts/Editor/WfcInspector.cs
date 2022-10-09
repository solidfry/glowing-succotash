using DefaultNamespace;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Test))]
public class WfcInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Test myScript = (Test)target;

        if (GUILayout.Button("Create Tilemap"))
        {
            myScript.CreateWfc();
            myScript.CreateTilemap();
        }
        if(GUILayout.Button("Save Tilemap"))
            myScript.SaveTilemap();
    }
}
