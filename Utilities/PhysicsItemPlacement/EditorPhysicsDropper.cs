using UnityEditor;
using UnityEngine;

public static class EditorPhysicsDropper
{
    [MenuItem("Tools/Drop With Physics %#d")] // Ctrl+Shift+D
    public static void DropWithPhysics()
    {
        foreach (var go in Selection.gameObjects)
        {
            var rb = go.GetComponent<Rigidbody>();
            if (rb) Object.DestroyImmediate(rb);

            if (!go.GetComponent<EditorGravityDrop>())
                go.AddComponent<EditorGravityDrop>();
        }
    }
}