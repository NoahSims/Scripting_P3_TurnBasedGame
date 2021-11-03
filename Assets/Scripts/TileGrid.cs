using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public float CellSize { get; private set; }

    public int[,] GridArray { get; private set; }

    public TileGrid(int width, int height, float cellSize)
    {
        Width = width;
        Height = height;
        CellSize = cellSize;
        GridArray = new int[Width, Height];
    }
}
