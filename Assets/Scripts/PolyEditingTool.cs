using UnityEngine;
using UnityEditor;

public class PolyEditingTool : EditorWindow
{
    GameObject planetToSpawn;

    string planetName = "";

    float planetScale;

    [MenuItem("Tools/Poly Tool/Poly Hobby Editing Tool")]
    public static void ShowWindow()
    {
        GetWindow(typeof(PolyEditingTool));
    }

    void OnGUI()
    {
        GUILayout.Label("Planet Editing Tools", EditorStyles.boldLabel);
        planetToSpawn = (GameObject)EditorGUILayout.ObjectField("Planet Prefab", planetToSpawn, typeof(GameObject), false);
        planetName = EditorGUILayout.TextField("Planet Name", planetName);
        planetScale = EditorGUILayout.FloatField("Planet Scale", planetScale);

        if (GUILayout.Button("Spawn Planet"))
        {
            SpawnPlanet();
        }
    }

    void SpawnPlanet()
    {
        if (planetToSpawn == null)
        {
            Debug.LogError("No planet prefab selected");
            return;
        }

        if (planetName == "")
        {
            Debug.LogError("No planet name entered");
            return;
        }

        if (planetScale <= 0)
        {
            Debug.LogError("Planet scale must be greater than 0");
            return;
        }

        GameObject planet = Instantiate(planetToSpawn, Vector3.zero, Quaternion.identity);
        planet.name = planetName;
        planet.transform.localScale = new Vector3(planetScale, planetScale, planetScale);

        PolygonCollider2D collider = planet.GetComponent<PolygonCollider2D>();
        if (collider == null)
        {
            Debug.LogError("Planet prefab must have a PolygonCollider2D component");
            return;
        }

        Vector2[] points = new Vector2[collider.points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = collider.points[i] * planetScale;
        }

        collider.points = points;
    }
}