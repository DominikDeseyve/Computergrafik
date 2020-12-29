using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public float length = 255;
    public float width = 255;
    public float height = 10f;

    public float moistureScale = 10f;

    [Range(0, 1)]
    public float detailLevel = 0.8f;


    void Start()
    {
        // PerlinNoise 
        //PerlinNoise perlinNoise = (PerlinNoise) ScriptableObject.CreateInstance("PerlinNoise");
        //GetComponent<Renderer>().material.SetTexture("_MoistureMap", perlinNoise.GenerateTexture(width, height));

        // DiamondSquareTerrain
        //DiamondSquareTerrain diamondSquare = (DiamondSquareTerrain) ScriptableObject.CreateInstance("DiamondSquareTerrain");
        //GetComponent<Renderer>().material.SetTexture("_HeightMap", diamondSquare.CreateTerrain());

        // NormalMap
        //NormalMap normalMap = (NormalMap) ScriptableObject.CreateInstance("NormalMap");
        //GetComponent<Renderer>().material.SetTexture("_NormalMap", normalMap.CreateTerrain());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
