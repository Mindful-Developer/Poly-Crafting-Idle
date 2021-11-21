using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseManager : MonoBehaviour
{
    public RawImage noiseTexture;
    public Terrain noiseTerrain;

    public int width = 256;
    public int height = 256;

    private int _seed, _lastSeed;

    private float _scaling, _lastScaling;


    private Noise _noise;

    private void Awake()
    {
        _scaling = 0.1f;
        _seed = 0;
        _noise = new PerlinNoise();
        _recalcNoise();
    }

    private void _recalcNoise()
    {
        _noise.seed = _seed;

        float[,] noise = new float[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                noise[y, x] = _noise.GetNoiseMap(x, y, _scaling);
            }
        }
        _setNoiseTexture(noise);
    }

    private void _setNoiseTexture(float[,] noise)
    {
        Color[] colors = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colors[x + width * y] = Color.Lerp(Color.black, Color.white, noise[y, x]);
            }
        }

        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(colors);
        texture.Apply();

        noiseTexture.texture = texture;

        noiseTerrain.terrainData.SetHeights(0, 0, noise);
    }

    private void _UpdateGUI()
    {
        if (_scaling == _lastScaling && _seed == _lastSeed)
            return;

        _recalcNoise();

        _lastScaling = _scaling;
        _lastSeed = _seed;
    }

    private void OnGUI()
    {
        _scaling = GUI.HorizontalSlider(new Rect(10, 10, 100, 30), _scaling, 0.1f, 0.3f);
        _seed = (int)GUI.HorizontalSlider(new Rect(10, 50, 100, 30), _seed, 0, 100);

        if (GUI.changed)
        {
            _recalcNoise();
        }
    }
}
