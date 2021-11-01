using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polygon
{
    public List<int> icoVerts;

    public Polygon(int ptA, int ptB, int ptC)
    {
        icoVerts = new List<int>() { ptA, ptB, ptC };
    }
}