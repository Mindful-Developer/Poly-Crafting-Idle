using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SimplifyMesh))]
public class SimplifyMeshEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
