using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public int height;
    public int width;

    void Start()
    {
        // DisplacementMap


        // PerlinNoise 
        PerlinNoise perlinNoise = (PerlinNoise)ScriptableObject.CreateInstance("PerlinNoise");
        GetComponent<Renderer>().material.SetTexture("PERLINNOISE", perlinNoise.texture);

        // ColorMap
        ColorMap colorMap = (ColorMap)ScriptableObject.CreateInstance("ColorMap");
        GetComponent<Renderer>().material.SetTexture("COLORMAP", colorMap.texture);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
