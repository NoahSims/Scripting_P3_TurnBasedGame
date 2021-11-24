using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPiece : MonoBehaviour
{
    [Header("Chess Piece")]
    [SerializeField] public ChessTeamEnum ChessPieceTeam;
    [SerializeField] public ChessPieceEnum ChessPieceType;
    public int xPos;
    public int zPos;
    public bool inPlay = true;

    public abstract List<Vector2> GetPossibleMoves();

    // returns true if tile is empty, returns false if tile is occupied or out of bounds
    protected bool CheckRelativeTile(int x, int z, List<Vector2> result)
    {
        int tile = GameBoardController.Current.CheckTileContents(xPos + x, zPos + z);

        if (tile == ((int)ChessTeamEnum.EMPTY))
        {
            result.Add(new Vector2(xPos + x, zPos + z));
            return true;
        }
        // else if on oposite teams
        else if(GameBoardController.Current.PiecesAllowedToAttack && 
            ((tile == ((int)ChessTeamEnum.BLACK) && ChessPieceTeam == ChessTeamEnum.DEFENDER) || 
            ((tile == ((int)ChessTeamEnum.WHITE) || tile == ((int)ChessTeamEnum.DEFENDER)) && ChessPieceTeam == ChessTeamEnum.BLACK)))
        {
            result.Add(new Vector2(xPos + x, zPos + z));
            return false;
        }
        else
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

        //GameBoardController.Current.GameBoard.GridArray[xPos, zPos].TileContents = ((int)ChessPieceEnum.EMPTY);
        GameBoardController.Current.GameBoard.GridArray[xPos, zPos].TilePiece = null;
        SetChessPiecePosition(newX, newZ);
    }

    public void SetChessPiecePosition(int newX, int newZ)
    {
        //GameBoardController.Current.GameBoard.GridArray[newX, newZ].TileContents = ((int)ChessPieceType);
        GameBoardController.Current.GameBoard.GridArray[newX, newZ].TilePiece = this;
        transform.position = GameBoardController.Current.GetChessWorldSpaceFromTile(newX, newZ);
        xPos = newX;
        zPos = newZ;
    }

    public void PieceCaptured()
    {
        Debug.Log(gameObject.name + ": Captured");
        //GameBoardController.Current.GameBoard.GridArray[xPos, zPos].TileContents = ((int)ChessPieceEnum.EMPTY);
        GameBoardController.Current.GameBoard.GridArray[xPos, zPos].TilePiece = null;
        gameObject.SetActive(false);
        inPlay = false;
    }

    public virtual void SetTileIndicator(bool status)
    {
        GameBoardController.Current.GameBoard.GridArray[xPos, zPos].TileIndicator.SetActive(status);
    }
}
