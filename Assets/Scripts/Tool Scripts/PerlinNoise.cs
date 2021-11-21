using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : Noise
{
    private const float C = 1000f;
    public override float GetNoiseMap(float x, float y, float scale = 1f)
    {
        x = (x + seed * C) * scale;
        y = (y + seed * C) * scale;

        return Mathf.PerlinNoise(x, y);
    }
    
}
