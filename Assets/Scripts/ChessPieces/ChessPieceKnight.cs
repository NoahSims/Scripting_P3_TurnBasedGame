using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPieceKnight : ChessPiece
{
    public override List<Vector2> GetPossibleMoves()
    {
        List<Vector2> result = new List<Vector2>();

        if (GameBoardController.Current.CheckTileContents(xPos + 1, zPos + 2) >= 0)
            result.Add(new Vector2(xPos + 1, zPos + 2));
        if (GameBoardController.Current.CheckTileContents(xPos + 2, zPos + 1) >= 0)
            result.Add(new Vector2(xPos + 2, zPos + 1));
        if (GameBoardController.Current.CheckTileContents(xPos + 2, zPos - 1) >= 0)
            result.Add(new Vector2(xPos + 2, zPos - 1));
        if (GameBoardController.Current.CheckTileContents(xPos + 1, zPos -2) >= 0)
            result.Add(new Vector2(xPos + 1, zPos - 2));
        if (GameBoardController.Current.CheckTileContents(xPos - 1, zPos -2) >= 0)
            result.Add(new Vector2(xPos - 1, zPos - 2));
        if (GameBoardController.Current.CheckTileContents(xPos - 2, zPos - 1) >= 0)
            result.Add(new Vector2(xPos - 2, zPos - 1));
        if (GameBoardController.Current.CheckTileContents(xPos - 2, zPos + 1) >= 0)
            result.Add(new Vector2(xPos - 2, zPos + 1));
        if (GameBoardController.Current.CheckTileContents(xPos - 1, zPos + 2) >= 0)
            result.Add(new Vector2(xPos - 1, zPos + 2));

        return result;
    }
}
