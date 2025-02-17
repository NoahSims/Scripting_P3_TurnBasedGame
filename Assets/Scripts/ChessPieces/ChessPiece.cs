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
    [SerializeField] private AudioClip _moveSound = null;
    [SerializeField] private AudioClip _captureSound = null;

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
            //Insert to the front of the move list so that moves that capture can be evauluated be minimax first; better for alpha-beta pruning
            result.Insert(0, new Vector2(xPos + x, zPos + z));
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

    // if a piece is captured, returns reference to captured piece, else returns null
    public ChessPiece MoveChessPiece(int newX, int newZ)
    {
        ChessPiece capturedPiece = null;

        if (GameBoardController.Current.CheckTileContents(newX, newZ) > 0)
        {
            capturedPiece = GameBoardController.Current.GameBoard.GridArray[newX, newZ].TilePiece;
            capturedPiece.PieceCaptured();
            Debug.Log(capturedPiece.gameObject.name + ": Captured by : " + gameObject.name);
            Feedback(null, _captureSound);
        }
        else
            Feedback(null, _moveSound);

        GameBoardController.Current.GameBoard.GridArray[xPos, zPos].TilePiece = null;
        SetChessPiecePosition(newX, newZ, false);

        return capturedPiece;
    }

    public void SetChessPiecePosition(int newX, int newZ, bool useSound)
    {
        GameBoardController.Current.GameBoard.GridArray[newX, newZ].TilePiece = this;
        transform.position = GameBoardController.Current.GetChessWorldSpaceFromTile(newX, newZ);
        xPos = newX;
        zPos = newZ;

        if(useSound)
            Feedback(null, _moveSound);
    }

    public void PieceCaptured()
    {
        //Debug.Log(gameObject.name + ": Captured");
        GameBoardController.Current.GameBoard.GridArray[xPos, zPos].TilePiece = null;
        gameObject.SetActive(false);
        inPlay = false;
    }

    public virtual void SetTileIndicator(bool status)
    {
        GameBoardController.Current.GameBoard.GridArray[xPos, zPos].TileIndicator.SetActive(status);
        GameBoardController.Current.GameBoard.GridArray[xPos, zPos].TileIndicatorRed.SetActive(false);
    }

    public virtual void SetTileIndicatorRed(bool status)
    {
        GameBoardController.Current.GameBoard.GridArray[xPos, zPos].TileIndicator.SetActive(false);
        GameBoardController.Current.GameBoard.GridArray[xPos, zPos].TileIndicatorRed.SetActive(status);
    }

    private void Feedback(ParticleSystem particles, AudioClip sound)
    {
        // particles
        if (particles != null)
        {
            ParticleSystem _particles = Instantiate(particles, transform.position, Quaternion.identity);
            _particles.Play();
        }
        // audio. TODO - consider Object Pooling for performance
        if (sound != null)
        {
            AudioHelper.PlayClip2D(sound, 1f);
        }
    }
}
