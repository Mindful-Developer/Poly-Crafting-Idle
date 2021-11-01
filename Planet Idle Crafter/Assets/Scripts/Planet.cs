using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public Material icoMats;
    GameObject icoPlanetMesh;

    List<Polygon> icoPolys;
    List<Vector3> icoVerts;

    public void Start()
    {
        initAsIco();
        triDivide(3);
        genIcoMesh();
    }

    public void initAsIco()
    {
        icoPolys = new List<Polygon>();
        icoVerts = new List<Vector3>();

        float t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;

        icoVerts.Add(new Vector3(-1,  t,  0).normalized);
        icoVerts.Add(new Vector3( 1,  t,  0).normalized);
        icoVerts.Add(new Vector3(-1, -t,  0).normalized);
        icoVerts.Add(new Vector3( 1, -t,  0).normalized);
        icoVerts.Add(new Vector3( 0, -1,  t).normalized);
        icoVerts.Add(new Vector3( 0,  1,  t).normalized);
        icoVerts.Add(new Vector3( 0, -1, -t).normalized);
        icoVerts.Add(new Vector3( 0,  1, -t).normalized);
        icoVerts.Add(new Vector3( t,  0, -1).normalized);
        icoVerts.Add(new Vector3( t,  0,  1).normalized);
        icoVerts.Add(new Vector3(-t,  0, -1).normalized);
        icoVerts.Add(new Vector3(-t,  0,  1).normalized);

        icoPolys.Add(new Polygon( 0, 11,  5));
        icoPolys.Add(new Polygon( 0,  5,  1));
        icoPolys.Add(new Polygon( 0,  1,  7));
        icoPolys.Add(new Polygon( 0,  7, 10));
        icoPolys.Add(new Polygon( 0, 10, 11));
        icoPolys.Add(new Polygon( 1,  5,  9));
        icoPolys.Add(new Polygon( 5, 11,  4));
        icoPolys.Add(new Polygon(11, 10,  2));
        icoPolys.Add(new Polygon(10,  7,  6));
        icoPolys.Add(new Polygon( 7,  1,  8));
        icoPolys.Add(new Polygon( 3,  9,  4));
        icoPolys.Add(new Polygon( 3,  4,  2));
        icoPolys.Add(new Polygon( 3,  2,  6));
        icoPolys.Add(new Polygon( 3,  6,  8));
        icoPolys.Add(new Polygon( 3,  8,  9));
        icoPolys.Add(new Polygon( 4,  9,  5));
        icoPolys.Add(new Polygon( 2,  4, 11));
        icoPolys.Add(new Polygon( 6,  2, 10));
        icoPolys.Add(new Polygon( 8,  6,  7));
        icoPolys.Add(new Polygon( 9,  8,  1));
    }

    public void triDivide(int recur)
    {
        var midPointCache = new Dictionary<int, int>();

        for (int i = 0; i < recur; i++)
        {
            var newPolys = new List<Polygon>();
            foreach (var poly in icoPolys)
            {
                int a1 = poly.icoVerts[0];
                int b2 = poly.icoVerts[1];
                int c3 = poly.icoVerts[2];

                int a1b2 = getMidInd(midPointCache, a1, b2);
                int b2c3 = getMidInd(midPointCache, b2, c3);
                int c3a1 = getMidInd(midPointCache, c3, a1);

                newPolys.Add(new Polygon(a1, a1b2, c3a1));
                newPolys.Add(new Polygon(b2, b2c3, a1b2));
                newPolys.Add(new Polygon(c3, c3a1, b2c3));
                newPolys.Add(new Polygon(a1b2, b2c3, c3a1));
            }

            icoPolys = newPolys;
        }
    }
    public int getMidInd(Dictionary<int, int> cache, int indexA, int indexB)
    {

        int smallerIndex = Mathf.Min(indexA, indexB);
        int greaterIndex = Mathf.Max(indexA, indexB);
        int key = (smallerIndex << 16) + greaterIndex;

        int ret;
        if (cache.TryGetValue(key, out ret))
            return ret;

        Vector3 p1 = icoVerts[indexA];
        Vector3 p2 = icoVerts[indexB];
        Vector3 middle = Vector3.Lerp(p1, p2, 0.5f).normalized;

        ret = icoVerts.Count;
        icoVerts.Add(middle);

        cache.Add(key, ret);
        return ret;
    }

    public void genIcoMesh()
    {
        if (icoPlanetMesh)
        {
            Destroy(icoPlanetMesh);
        }

        icoPlanetMesh = new GameObject("Planet Mesh");

        MeshRenderer surfaceRenderer = icoPlanetMesh.AddComponent<MeshRenderer>();
        surfaceRenderer.material = icoMats;

        Mesh terrainMesh = new Mesh();

        int vertexCount = icoPolys.Count * 3;

        int[] indices = new int[vertexCount];

        Vector3[] vertices = new Vector3[vertexCount];
        Vector3[] normals = new Vector3[vertexCount];
        Color32[] colors = new Color32[vertexCount];

        Color32 green = new Color32(20,  255, 30, 255);
        Color32 brown = new Color32(220, 150, 70, 255);

        for (int i = 0; i < icoPolys.Count; i++)
        {
            var poly = icoPolys[i];

            indices[i * 3 + 0] = i * 3 + 0;
            indices[i * 3 + 1] = i * 3 + 1;
            indices[i * 3 + 2] = i * 3 + 2;

            vertices[i * 3 + 0] = icoVerts[poly.icoVerts[0]];
            vertices[i * 3 + 1] = icoVerts[poly.icoVerts[1]];
            vertices[i * 3 + 2] = icoVerts[poly.icoVerts[2]];

            Color32 polyColor = Color32.Lerp(green, brown, Random.Range(0.0f, 1.0f)); 

            colors[i * 3 + 0] = polyColor;
            colors[i * 3 + 1] = polyColor;
            colors[i * 3 + 2] = polyColor;

            normals[i * 3 + 0] = icoVerts[poly.icoVerts[0]];
            normals[i * 3 + 1] = icoVerts[poly.icoVerts[1]];
            normals[i * 3 + 2] = icoVerts[poly.icoVerts[2]];
        }

        terrainMesh.vertices = vertices;
        terrainMesh.normals  = normals;
        terrainMesh.colors32 = colors;

        terrainMesh.SetTriangles(indices, 0);

        MeshFilter terrainFilter = icoPlanetMesh.AddComponent<MeshFilter>();
        terrainFilter.mesh = terrainMesh;
    }
}