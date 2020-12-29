using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMapGenerator : ScriptableObject
{
    public Texture2D CreateNormalMap(Texture2D heightMap) 
    {
        Texture2D texture = new Texture2D(heightMap.width, heightMap.height);

        for (int i = 0; i < heightMap.width; i++)
        {
            for (int j = 0; j < heightMap.height; j++)
            {
                float R = (heightMap.GetPixel(i + 1, j).r + heightMap.GetPixel(i + 1, j).g + heightMap.GetPixel(i + 1, j).b) / 3.0f;
                float L = (heightMap.GetPixel(i - 1, j).r + heightMap.GetPixel(i - 1, j).g + heightMap.GetPixel(i - 1, j).b) / 3.0f;
                float T = (heightMap.GetPixel(i, j + 1).r + heightMap.GetPixel(i, j + 1).g + heightMap.GetPixel(i, j + 1).b) / 3.0f;
                float B = (heightMap.GetPixel(i, j - 1).r + heightMap.GetPixel(i, j - 1).g + heightMap.GetPixel(i, j - 1).b) / 3.0f;

                Vector3 normal = new Vector3(R-L, B-T, 1.0f);
                normal.Normalize();
                //Vector3 normal = Vector3(2*(R-L), 2*(B-T), -4).Normalize();
                texture.SetPixel(i, j, new Color(normal.x, 0.5f, normal.y, normal.z));
            }
        }

        texture.Apply();
        return texture;
    }
}
