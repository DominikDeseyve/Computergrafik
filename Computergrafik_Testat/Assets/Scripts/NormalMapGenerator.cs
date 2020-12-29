using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMapGenerator : ScriptableObject
{
    public Texture2D CreateNormalMap(Texture2D heightMap) 
   {
        Color[] colors = heightMap.GetPixels();
        Texture2D normal = new Texture2D(heightMap.width, heightMap.height, TextureFormat.ARGB32, false);
        for (int i = 1; i < heightMap.width - 1; i++ )
            for (int j = 1; j < heightMap.height - 1; j++)
            {
                //using Sobel operator
                float tl, t, tr, l, right, bl, bot, br;
                tl = intensity(heightMap.GetPixel(i - 1, j - 1).r, heightMap.GetPixel(i- 1, j-1).g, heightMap.GetPixel(i-1, j-1).b);
                t = intensity(heightMap.GetPixel(i - 1, j).r, heightMap.GetPixel(i - 1, j).g, heightMap.GetPixel(i - 1, j).b);
                tr = intensity(heightMap.GetPixel(i - 1, j + 1).r, heightMap.GetPixel(i - 1, j + 1).g, heightMap.GetPixel(i - 1, j + 1).b);
                right = intensity(heightMap.GetPixel(i, j + 1).r, heightMap.GetPixel(i, j + 1).g, heightMap.GetPixel(i, j + 1).b);
                br = intensity(heightMap.GetPixel(i + 1, j + 1).r, heightMap.GetPixel(i + 1, j + 1).g, heightMap.GetPixel(i + 1, j + 1).b);
                bot = intensity(heightMap.GetPixel(i + 1, j).r, heightMap.GetPixel(i + 1, j).g, heightMap.GetPixel(i + 1, j).b);
                bl = intensity(heightMap.GetPixel(i + 1, j - 1).r, heightMap.GetPixel(i + 1, j - 1).g, heightMap.GetPixel(i + 1, j - 1).b);
                l = intensity(heightMap.GetPixel(i, j - 1).r, heightMap.GetPixel(i, j - 1).g, heightMap.GetPixel(i, j - 1).b);
 
                //Sobel filter
                float dX = (tr + 2.0f * right + br) - (tl + 2.0f * l + bl);
                float dY = (bl + 2.0f * bot + br) - (tl + 2.0f * t + tr);
                float dZ = 1.0f / 2.0f;
 
                Vector3 vc = new Vector3(dX, dY, dZ);
                vc.Normalize();
 
                normal.SetPixel(i, j, new Color(vc.x, 0.5f, vc.y, vc.z));
            }
        normal.Apply();
        return normal;
    }
 
    public float intensity(float r, float g, float b)
    {
        return (r + g + b) / 3.0f;
    }
}
