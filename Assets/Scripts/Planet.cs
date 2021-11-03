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

    public void triDivide(int recursions)
    {
        var midPointCache = new Dictionary<int, int>();

        for (int i = 0; i < recursions; i++)
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
        icoPlanetMesh = new GameObject("IcoPlanetMesh");
        icoPlanetMesh.transform.parent = gameObject.transform;
        icoPlanetMesh.transform.localPosition = Vector3.zero;
        icoPlanetMesh.transform.localRotation = Quaternion.identity;
        icoPlanetMesh.transform.localScale = Vector3.one;

        Mesh mesh = new Mesh();
        mesh.name = "IcoPlanetMesh";

        int vertexCount = icoPolys.Count * 3;

        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        Color32[] colors = new Color32[vertexCount];
        List<Vector3> normals = new List<Vector3>();

        Color32 white = new Color32(255, 255, 255, 255);
        Color32 blue = new Color32(100, 100, 255, 255);

        foreach (var poly in icoPolys)
        {
            int a = poly.icoVerts[0];
            int b = poly.icoVerts[1];
            int c = poly.icoVerts[2];

            verts.Add(icoVerts[a]);
            verts.Add(icoVerts[b]);
            verts.Add(icoVerts[c]);

            tris.Add(verts.Count - 3);
            tris.Add(verts.Count - 2);
            tris.Add(verts.Count - 1);

            Color32 polyColor = Color32.Lerp(blue, white, Random.Range(0.0f, 1.0f)); 

            colors[verts.Count - 3] = polyColor;
            colors[verts.Count - 2] = polyColor;
            colors[verts.Count - 1] = polyColor;

            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(1, 1));
        }

        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.colors32 = colors;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        MeshFilter meshFilter = icoPlanetMesh.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = icoPlanetMesh.AddComponent<MeshRenderer>();
        meshRenderer.material = icoMats;

        Shader shader;
        shader = Shader.Find("Legacy Shaders/Particles/Multiply");
        icoPlanetMesh.GetComponent<Renderer>().material.shader = shader;
    }
}