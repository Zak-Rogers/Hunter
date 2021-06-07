using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnSystem))]
public class SpawnSystemEditor : Editor
{
    // Add button to spawn enviroment objects to the scene.
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SpawnSystem spawnSystem = (SpawnSystem)target;

        if(GUILayout.Button("Spawn Enviroment"))
        {
            spawnSystem.SpawnEnviroment();
        }
    }
}
