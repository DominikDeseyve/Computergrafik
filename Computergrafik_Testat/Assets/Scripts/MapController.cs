using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public int length = 255;
    public int width = 255;
    public float height = 10f;

    public float moistureScale = 10f;

    [Range(0, 1)]
    public float detailLevel = 0.8f;


    void Start()
    {
        // PerlinNoise 
        PerlinNoise perlinNoise = (PerlinNoise)ScriptableObject.CreateInstance<PerlinNoise>();
        Texture2D moistureMap = perlinNoise.GenerateTexture(width, length, moistureScale);
        GetComponent<Renderer>().material.SetTexture("_MoistureMap", moistureMap);

        // DiamondSquareTerrain
        DiamondSquareTerrain diamondSquare = (DiamondSquareTerrain)ScriptableObject.CreateInstance<DiamondSquareTerrain>();
        Texture2D heightMap = diamondSquare.CreateTerrain(width, length, height, detailLevel);
        GetComponent<Renderer>().material.SetTexture("_HeightMap", heightMap);

        // NormalMap
        NormalMapGenerator normalMapGenerator = (NormalMapGenerator)ScriptableObject.CreateInstance<NormalMapGenerator>();
        Texture2D normalMap = normalMapGenerator.CreateNormalMap(heightMap);
        GetComponent<Renderer>().material.SetTexture("_NormalMap", normalMap);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
