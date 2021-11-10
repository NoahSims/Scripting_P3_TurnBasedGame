using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public float CellSize { get; private set; }

    public Tile[,] GridArray { get; private set; }

    public TileGrid(int width, int height, float cellSize)
    {
        Width = width;
        Height = height;
        CellSize = cellSize;
        GridArray = new Tile[Width, Height];

        for (int col = 0; col < Width; col++)
        {
            for (int row = 0; row < Height; row++)
            {
                GridArray[col, row] = new Tile();
            }
        }
    }
}

public class Tile
{
    public int TileContents;
    public GameObject TileIndicator;

    public Tile()
    {
        TileContents = 0;
        TileIndicator = null;
    }
}
