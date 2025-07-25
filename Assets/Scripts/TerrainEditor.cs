using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TerrainGenerator terrain = (TerrainGenerator)target;

        if (GUILayout.Button("Generate terrain"))
        {
            terrain.GenerateTerrain();
        }

        //Save any edits made to terrain during runtime
        if (GUILayout.Button("Save Edit"))
        {
            EditorUtility.SetDirty(terrain);
            AssetDatabase.SaveAssets();
        }
    }
    
}
