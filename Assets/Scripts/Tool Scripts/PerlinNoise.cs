using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : Noisifier
{
    public override float GetNoiseMap(float x, float y, float scale = 1)
    {
        return Mathf.PerlinNoise(x * scale, y * scale);
    }
}
