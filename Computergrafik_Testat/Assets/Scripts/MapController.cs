﻿using System.Collections;
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
        GetComponent<Renderer>().material.SetTexture("_MoistureMap", perlinNoise.GenerateTexture(width, length, moistureScale));

        // DiamondSquareTerrain
        DiamondSquareTerrain diamondSquare = (DiamondSquareTerrain)ScriptableObject.CreateInstance<DiamondSquareTerrain>();
        GetComponent<Renderer>().material.SetTexture("_HeightMap", diamondSquare.CreateTerrain(width, length, height));

        // NormalMap
        //NormalMap normalMap = (NormalMap) ScriptableObject.CreateInstance("NormalMap");
        //GetComponent<Renderer>().material.SetTexture("_NormalMap", normalMap.CreateTerrain());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
