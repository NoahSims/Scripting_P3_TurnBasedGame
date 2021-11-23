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

    // returns true if tile is empty, returns false if tile is occupied or out of bounds
    protected bool CheckRelativeTile(int x, int z, List<Vector2> result)
    {
        int tile = GameBoardController.Current.CheckTileContents(xPos + x, zPos + z);
        if (tile == ((int)ChessPieceEnum.EMPTY))
        {
            result.Add(new Vector2(xPos + x, zPos + z));
            return true;
        }
        if(tile == ((int)ChessPieceEnum.B_ENEMY))
        {
            result.Add(new Vector2(xPos + x, zPos + z));
            return false;
        }
        return false;
    }

    protected void CheckTilesInDirection(int x, int z, List<Vector2> result)
    {
        bool validTile = true;
        int i = 0;

        while (validTile)
        {
            i++;
            validTile = CheckRelativeTile(i * x, i * z, result);
        }
    }

    public void MoveChessPiece(int newX, int newZ)
    {
        if (GameBoardController.Current.CheckTileContents(newX, newZ) > 0)
            GameBoardController.Current.GameBoard.GridArray[newX, newZ].TilePiece.PieceCaptured();

        GameBoardController.Current.GameBoard.GridArray[xPos, zPos].TileContents = ((int)ChessPieceEnum.EMPTY);
        GameBoardController.Current.GameBoard.GridArray[xPos, zPos].TilePiece = null;
        GameBoardController.Current.GameBoard.GridArray[newX, newZ].TileContents = ((int)ChessPieceType);
        GameBoardController.Current.GameBoard.GridArray[newX, newZ].TilePiece = this;
        transform.position = GameBoardController.Current.GetChessWorldSpaceFromTile(newX, newZ);
        xPos = newX;
        zPos = newZ;
    }

    public void PieceCaptured()
    {
        Debug.Log(gameObject.name + ": Captured");
        GameBoardController.Current.GameBoard.GridArray[xPos, zPos].TileContents = ((int)ChessPieceEnum.EMPTY);
        GameBoardController.Current.GameBoard.GridArray[xPos, zPos].TilePiece = null;
        gameObject.SetActive(false);
    }

    public virtual void SetTileIndicator(bool status)
    {
        GameBoardController.Current.GameBoard.GridArray[xPos, zPos].TileIndicator.SetActive(status);
    }
}

public enum ChessTeams { WHITE, BLACK };
