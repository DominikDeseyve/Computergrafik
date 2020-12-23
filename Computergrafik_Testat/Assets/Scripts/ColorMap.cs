using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorMap : ScriptableObject
{
    public Texture2D texture;
    public void Init()
    {
        this.texture = Resources.Load<Texture2D>("Images/color_map");

    }

    public static ColorMap CreateInstance(int height, int width)
    {
        var cm = ScriptableObject.CreateInstance<ColorMap>();
        cm.Init();
        return cm;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
