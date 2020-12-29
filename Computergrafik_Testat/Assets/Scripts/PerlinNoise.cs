using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : ScriptableObject
{
    public Texture2D GenerateTexture(int pX, int pY, float scale)
    {
        Texture2D texture = new Texture2D(pX, pY);

        for (int x = 0; x < pX; x++)
        {
            for (int y = 0; y < pY; y++)
            {
                Color color = CalculateColor(x, y, pX, pY, scale);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }

    private Color CalculateColor(int x, int y, int width, int length, float scale)
    {
        float xCoord = (float)x / width * scale;
        float yCoord = (float)y / length * scale;
        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, 1f, 1f);
    }
}
