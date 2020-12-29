using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondSquareTerrain : ScriptableObject
{

    private int mDivisions;
    private float size;
    private float n;
    //public float width = 255;
    //public float length = 255;
    //public float height = 10;

    [Range(0, 1)]
    public float Detailierungsgrad = 0.8f;

    Vector3[] mVerts;
    int mVertCount;


    public Texture2D CreateTerrain(int width, int length, float height)
    {
        size = (width >= length) ? (width - 1) : (length - 1);
        n = Mathf.Ceil(Mathf.Log(size, 2));
        mDivisions = (int)Mathf.Pow(2, n);
        size = mDivisions + 1;

        mVertCount = (mDivisions + 1) * (mDivisions + 1);
        mVerts = new Vector3[mVertCount];
        Vector2[] uvs = new Vector2[mVertCount];
        int[] tris = new int[mDivisions * mDivisions * 6];

        float halfSize = size * 0.5f;
        float divisionSize = size / mDivisions;

        for (int i = 0; i <= mDivisions; i++)
        {

            for (int j = 0; j <= mDivisions; j++)
            {
                mVerts[i * (mDivisions + 1) + j] = new Vector3(-halfSize + j * divisionSize, 0.0f, halfSize - i * divisionSize);
            }
        }

        mVerts[0].y = Random.Range(-height, height);
        mVerts[mDivisions].y = Random.Range(-height, height);
        mVerts[mVerts.Length - 1].y = Random.Range(-height, height);
        mVerts[mVerts.Length - 1 - mDivisions].y = Random.Range(-height, height);

        int iterations = (int)Mathf.Log(mDivisions, 2);
        int numSquares = 1;
        int squareSize = mDivisions;

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
            height *= Detailierungsgrad;
        }

        float[] minMax = minMaxHeight();

        var texture = new Texture2D((int)width, (int)length);     //Variable am Anfang für Größe der Textur

        float total = minMax[1] - minMax[0];


        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                float h = (mVerts[i * (mDivisions + 1) + j].y - minMax[0]) / total;
                texture.SetPixel(i, j, new Vector4(h, 1.0f, 1.0f, 1.0f));
            }
        }

        //GetComponent<Renderer>().material.SetTexture("_HeightMap", texture);
        texture.Apply();
        return texture;
    }

    float[] minMaxHeight()
    {
        float min = mVerts[0].y;
        float max = mVerts[0].y;
        for (int i = 0; i < mVerts.Length; i++)
        {
            min = System.Math.Min(mVerts[i].y, min);
            max = System.Math.Max(mVerts[i].y, max);
        }
        return new float[2] { min, max };
    }

    void DiamondSquare(int row, int col, int size, float offset)
    {

        int halfSize = (int)(size * 0.5f);
        int topLeft = row * (mDivisions + 1) + col;
        int botLeft = (row + size) * (mDivisions + 1) + col;

        int mid = (int)(row + halfSize) * (mDivisions + 1) + (int)(col + halfSize);
        mVerts[mid].y = (mVerts[topLeft].y + mVerts[topLeft + size].y + mVerts[botLeft].y + mVerts[botLeft + size].y) * 0.25f + Random.Range(-offset, offset);

        mVerts[topLeft + halfSize].y = (mVerts[topLeft].y + mVerts[topLeft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);
        mVerts[mid - halfSize].y = (mVerts[topLeft].y + mVerts[botLeft].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);
        mVerts[mid + halfSize].y = (mVerts[topLeft + size].y + mVerts[topLeft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);
        mVerts[botLeft + halfSize].y = (mVerts[botLeft].y + mVerts[botLeft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);
    }
}
