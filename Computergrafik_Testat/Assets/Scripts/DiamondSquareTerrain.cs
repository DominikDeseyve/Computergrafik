using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondSquareTerrain : ScriptableObject
{

    private int divisions;
    private float size;
    private float n;

    Vector3[] verts;
    int vertCount;


    public Texture2D CreateTerrain(int width, int length, float height, float detailLevel)
    {
        size = (width >= length) ? (width - 1) : (length - 1);
        n = Mathf.Ceil(Mathf.Log(size, 2));
        divisions = (int)Mathf.Pow(2, n);
        size = divisions + 1;

        vertCount = (divisions + 1) * (divisions + 1);
        verts = new Vector3[vertCount];
        Vector2[] uvs = new Vector2[vertCount];
        int[] tris = new int[divisions * divisions * 6];

        float halfSize = size * 0.5f;
        float divisionSize = size / divisions;

        for (int i = 0; i <= divisions; i++)
        {

            for (int j = 0; j <= divisions; j++)
            {
                verts[i * (divisions + 1) + j] = new Vector3(-halfSize + j * divisionSize, 0.0f, halfSize - i * divisionSize);
            }
        }

        verts[0].y = Random.Range(-height, height);
        verts[divisions].y = Random.Range(-height, height);
        verts[verts.Length - 1].y = Random.Range(-height, height);
        verts[verts.Length - 1 - divisions].y = Random.Range(-height, height);

        int iterations = (int)Mathf.Log(divisions, 2);
        int numSquares = 1;
        int squareSize = divisions;

        for (int i = 0; i < iterations; i++)
        {
            int row = 0;
            for (int j = 0; j < numSquares; j++)
            {
                int col = 0;
                for (int k = 0; k < numSquares; k++)
                {
                    DiamondSquare(row, col, squareSize, height);
                    col += squareSize;
                }
                row += squareSize;
            }
            numSquares *= 2;
            squareSize /= 2;
            height *= detailLevel;
        }

        float[] minMax = minMaxHeight();
        var texture = new Texture2D((int)width, (int)length);     //Variable am Anfang für Größe der Textur
        float total = minMax[1] - minMax[0];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                float h = (verts[i * (divisions + 1) + j].y - minMax[0]) / total;
                texture.SetPixel(i, j, new Vector4(h, 1.0f, 1.0f, 1.0f));
            }
        }
        texture.Apply();
        return texture;
    }

    float[] minMaxHeight()
    {
        float min = verts[0].y;
        float max = verts[0].y;
        for (int i = 0; i < verts.Length; i++)
        {
            min = System.Math.Min(verts[i].y, min);
            max = System.Math.Max(verts[i].y, max);
        }
        return new float[2] { min, max };
    }

    void DiamondSquare(int row, int col, int size, float offset)
    {
        int halfSize = (int)(size * 0.5f);
        int topLeft = row * (divisions + 1) + col;
        int botLeft = (row + size) * (divisions + 1) + col;

        int mid = (int)(row + halfSize) * (divisions + 1) + (int)(col + halfSize);
        verts[mid].y = (verts[topLeft].y + verts[topLeft + size].y + verts[botLeft].y + verts[botLeft + size].y) * 0.25f + Random.Range(-offset, offset);

        verts[topLeft + halfSize].y = (verts[topLeft].y + verts[topLeft + size].y + verts[mid].y) / 3 + Random.Range(-offset, offset);
        verts[mid - halfSize].y = (verts[topLeft].y + verts[botLeft].y + verts[mid].y) / 3 + Random.Range(-offset, offset);
        verts[mid + halfSize].y = (verts[topLeft + size].y + verts[topLeft + size].y + verts[mid].y) / 3 + Random.Range(-offset, offset);
        verts[botLeft + halfSize].y = (verts[botLeft].y + verts[botLeft + size].y + verts[mid].y) / 3 + Random.Range(-offset, offset);
    }
}
