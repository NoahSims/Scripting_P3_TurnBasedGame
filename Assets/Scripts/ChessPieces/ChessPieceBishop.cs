using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPieceBishop : ChessPiece
{
    public override List<Vector2> GetPossibleMoves()
    {
        List<Vector2> result = new List<Vector2>();

        bool validTile = true;
        int pos = 0;

        while (validTile)
        {
            pos++;
            int tileContent = GameBoardController.Current.CheckTileContents(xPos + pos, zPos + pos);
            if (tileContent == 0)
            {
                result.Add(new Vector2(xPos + pos, zPos + pos));
            }
            else
            {
                if (tileContent == ((int)ChessPieceEnum.B_ENEMY))
                    result.Add(new Vector2(xPos + pos, zPos + pos));

                validTile = false;
            }
        }
        validTile = true;
        pos = 0;
        while (validTile)
        {
            pos++;
            int tileContent = GameBoardController.Current.CheckTileContents(xPos + pos, zPos - pos);
            if (tileContent == 0)
            {
                result.Add(new Vector2(xPos + pos, zPos - pos));
            }
            else
            {
                if (tileContent == ((int)ChessPieceEnum.B_ENEMY))
                    result.Add(new Vector2(xPos + pos, zPos - pos));

                validTile = false;
            }
        }
        validTile = true;
        pos = 0;
        while (validTile)
        {
            pos++;
            int tileContent = GameBoardController.Current.CheckTileContents(xPos - pos, zPos - pos);
            if (tileContent == 0)
            {
                result.Add(new Vector2(xPos - pos, zPos - pos));
            }
            else
            {
                if (tileContent == ((int)ChessPieceEnum.B_ENEMY))
                    result.Add(new Vector2(xPos - pos, zPos - pos));

                validTile = false;
            }
        }
        validTile = true;
        pos = 0;
        while (validTile)
        {
            pos++;
            int tileContent = GameBoardController.Current.CheckTileContents(xPos - pos, zPos + pos);
            if (tileContent == 0)
            {
                result.Add(new Vector2(xPos - pos, zPos + pos));
            }
            else
            {
                if (tileContent == ((int)ChessPieceEnum.B_ENEMY))
                    result.Add(new Vector2(xPos - pos, zPos + pos));

                validTile = false;
            }
        }

        return result;
    }
}
