using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateNoise : MonoBehaviour
{
    public int resolution = 256;

    private Texture2D texture;

    private void onAwaken()
    {
        texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, false);
        texture.name = "Texture";

        GetComponent<MeshRenderer>().material.mainTexture = texture;
        fillTexture();
    }

    private void fillTexture()
    {
        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                texture.SetPixel(i, j, Color.red);
            }
        }
        texture.Apply();
    }
}
