using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{

    public int height;
    public int width;
    public float scale;

    private void Start()
    {
        height = 256;
        width = 256;
        scale = 20f;
    }
    void Update()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture(width, height);
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
        float xCoord = (float)x * scale;
        float yCoord = (float)y * scale;
        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);
    }
}
