using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMapGenerator : ScriptableObject
{
    public Texture2D CreateNormalMap(Texture2D heightMap) 
   {
        Texture2D normal = new Texture2D(heightMap.width, heightMap.height, TextureFormat.ARGB32, false);
        for (int i = 1; i < heightMap.width - 1; i++ )
            for (int j = 1; j < heightMap.height - 1; j++)
            {                
                float t = intensity(heightMap.GetPixel(i - 1, j).r, heightMap.GetPixel(i - 1, j).g, heightMap.GetPixel(i - 1, j).b);   
                float r = intensity(heightMap.GetPixel(i, j + 1).r, heightMap.GetPixel(i, j + 1).g, heightMap.GetPixel(i, j + 1).b);            
                float b = intensity(heightMap.GetPixel(i + 1, j).r, heightMap.GetPixel(i + 1, j).g, heightMap.GetPixel(i + 1, j).b);
                float l = intensity(heightMap.GetPixel(i, j - 1).r, heightMap.GetPixel(i, j - 1).g, heightMap.GetPixel(i, j - 1).b);
 
                float dX = 2.0f * r - 2.0f * l;
                float dY = 2.0f * b - 2.0f * t;
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