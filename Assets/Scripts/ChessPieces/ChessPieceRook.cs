using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPieceRook : ChessPiece
{
    public override List<Vector2> GetPossibleMoves()
    {
        List<Vector2> result = new List<Vector2>();

        CheckTilesInDirection(1, 0, result);
        CheckTilesInDirection(-1, 0, result);
        CheckTilesInDirection(0, 1, result);
        CheckTilesInDirection(0, -1, result);

        return result;
    }
}
