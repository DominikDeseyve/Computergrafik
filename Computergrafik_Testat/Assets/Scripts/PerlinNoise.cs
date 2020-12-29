using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    public int height = 256;
    public int width = 256;
    private float moistureScale = 10f;

    public Texture2D texture;

    private void Start()
    {
        GenerateTexture(width, height);
    }

    public Texture2D GenerateTexture(int pX, int pY)
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
        
        GetComponent<Renderer>().material.SetTexture("_MoistureMap", texture);
        texture.Apply();
        return texture;
    }

    private Color CalculateColor(int x, int y)
    {
        float xCoord = (float)x / width * moistureScale;
        float yCoord = (float)y / height * moistureScale;
        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, 1f, 1f);
    }
}
