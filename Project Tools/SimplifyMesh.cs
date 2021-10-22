using UnityEngine;
using System.Collections;
using UnityEditor;

public class SimplifyMesh : MonoBehaviour
{
    public float TargetPercent = 0.0f;
    public float QualityThreshhold = 1.0f;
    public float BoundaryWeight = 1.0f;
    public float ExtraTexCoordWeight = 1.0f;

    public bool PreserveBoundary;
    public bool PreserveNormals;
    public bool OptimalPlacement;
    public bool PlanarQuadric;
    public bool Selected;
}
