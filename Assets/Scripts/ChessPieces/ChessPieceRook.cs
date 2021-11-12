using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPieceRook : ChessPiece
{
    public override List<Vector2> GetPossibleMoves()
    {
        List<Vector2> result = new List<Vector2>();

        bool validTile = true;
        int pos = 0;

        while(validTile)
        {
            pos++;
            int tileContent = GameBoardController.Current.CheckTileContents(xPos + pos, zPos);
            if (tileContent == 0)
            {
                result.Add(new Vector2(xPos + pos, zPos));
            }
            else
            {
                if (tileContent == ((int)ChessPieceEnum.B_ENEMY))
                    result.Add(new Vector2(xPos + pos, zPos));

                validTile = false;
            }
        }
        validTile = true;
        pos = 0;
        while (validTile)
        {
            pos--;
            int tileContent = GameBoardController.Current.CheckTileContents(xPos + pos, zPos);
            if (tileContent == 0)
            {
                result.Add(new Vector2(xPos + pos, zPos));
            }
            else
            {
                if (tileContent == ((int)ChessPieceEnum.B_ENEMY))
                    result.Add(new Vector2(xPos + pos, zPos));

                validTile = false;
            }
        }
        validTile = true;
        pos = 0;
        while (validTile)
        {
            pos++;
            int tileContent = GameBoardController.Current.CheckTileContents(xPos, zPos + pos);
            if (tileContent == 0)
            {
                result.Add(new Vector2(xPos, zPos + pos));
            }
            else
            {
                if (tileContent == ((int)ChessPieceEnum.B_ENEMY))
                    result.Add(new Vector2(xPos, zPos + pos));

                validTile = false;
            }
        }
        validTile = true;
        pos = 0;
        while (validTile)
        {
            pos--;
            int tileContent = GameBoardController.Current.CheckTileContents(xPos, zPos + pos);
            if (tileContent == 0)
            {
                result.Add(new Vector2(xPos, zPos + pos));
            }
            else
            {
                if (tileContent == ((int)ChessPieceEnum.B_ENEMY))
                    result.Add(new Vector2(xPos, zPos + pos));

                validTile = false;
            }
        }

        return result;
    }
}
