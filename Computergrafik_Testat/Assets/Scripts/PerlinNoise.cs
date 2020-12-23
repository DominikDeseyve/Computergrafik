using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : ScriptableObject
{
    public int height;
    public int width;
    private float scale = 10f;

    public Texture2D texture;

    public void Init(int height, int width)
    {
        this.height = height;
        this.width = width;

        this.texture = GenerateTexture(height, width);
    }

    public static PerlinNoise CreateInstance(int height, int width)
    {
        var pn = ScriptableObject.CreateInstance<PerlinNoise>();
        pn.Init(height, width);
        return pn;
    }

    private void Start()
    {
        height = 256;
        width = 256;
        scale = 10f;
    }

    public Texture2D getMoistureMap(int pX, int pY)
    {
        return GenerateTexture(pX, pY);
    }

    private Texture2D GenerateTexture(int pX, int pY)
    {
        Texture2D texture = new Texture2D(pX, pY);

        for (int x = 0; x < pX; x++)
        {
            for (int y = 0; y < pY; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return texture;
    }

    private Color CalculateColor(int x, int y)
    {
        float xCoord = (float)x / width * scale;
        float yCoord = (float)y / height * scale;
        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);
    }
}
