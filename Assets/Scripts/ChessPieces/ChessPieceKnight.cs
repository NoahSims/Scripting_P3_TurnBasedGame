using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPieceKnight : ChessPiece
{
    public override List<Vector2> GetPossibleMoves()
    {
        List<Vector2> result = new List<Vector2>();

        CheckRelativeTile(1, 2, result);
        CheckRelativeTile(2, 1, result);
        CheckRelativeTile(2, -1, result);
        CheckRelativeTile(1, -2, result);
        CheckRelativeTile(-1, -2, result);
        CheckRelativeTile(-2, -1, result);
        CheckRelativeTile(-2, 1, result);
        CheckRelativeTile(-1, 2, result);

        return result;
    }
}
