using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPiece : MonoBehaviour
{
    [Header("Chess Piece")]
    [SerializeField] protected ChessTeams Team;
    [SerializeField] public ChessPieceEnum ChessPieceType;
    public int xPos;
    public int zPos;

    public abstract List<Vector2> GetPossibleMoves();

    public virtual void SetTileIndicator(bool status)
    {
        GameBoardController.Current.GameBoard.GridArray[xPos, zPos].TileIndicator.SetActive(status);
    }
}

public enum ChessTeams { WHITE, BLACK };
