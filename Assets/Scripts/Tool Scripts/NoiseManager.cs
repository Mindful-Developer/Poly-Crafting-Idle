using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseManager : MonoBehaviour
{
    public RawImage noiseMapImage;
    public Terrain noiseTerrain;
    
    public int width = 256;
    public int height = 256;

    private Noisifier _noise;

    private void Awake()
    {
        _noise = new PerlinNoise();
        recalculateNoise();
    }

    private void recalculateNoise()
    {
        float[,] noise = new float[width, height];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                noise[j, i] = _noise.GetNoiseMap(i, j, 0.2f); 
            }
        }
        setNoiseTexture(noise);
    }

    private void setNoiseTexture(float[,] noise)
    {
        Color[] colorMapping = new Color[width * height];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                colorMapping[i * width + j] = Color.Lerp(Color.black, Color.white, noise[j, i]);           }
        }

        Texture2D textureMapping = new Texture2D(width, height);
        textureMapping.SetPixels(colorMapping);
        textureMapping.Apply();

        noiseMapImage.texture = textureMapping;
        noiseTerrain.terrainData.SetHeights(0, 0, noise);

    }
}
