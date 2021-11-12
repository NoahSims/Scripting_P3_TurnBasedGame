using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerTurnChessGameState : ChessGameState
{
    public static event Action<int> PlayerTurnBegan;
    public static event Action PlayerTurnEnded;

    public override void Enter()
    {
        Debug.Log("Player Turn: ...Entering");
        PlayerTurnBegan?.Invoke(StateMachine.RoundNumber);

        // hook into events
        ChessGameUIController.ContinueButtonPressed += OnPressedConfirm;
        InputController.Current.PressedMouse += OnMousePressed;

        //GameBoardController.Current.GameBoard.GridArray[whiteKnight.GetComponent<ChessPieceKnight>().xPos, whiteKnight.GetComponent<ChessPieceKnight>().zPos].TileIndicator.SetActive(true);
        HighlightDefenders();
    }

    private void OnMousePressed(int button)
    {
        if (button == 0)
        {
            GameBoardController.Current.DisableAllIndicators();

            Vector2 tile = GameBoardController.Current.GetTileFromWorldSpace(InputController.Current.GetMouseWorldPosition());
            int tileContents = GameBoardController.Current.CheckTileContents(((int)tile.x), ((int)tile.y));
            if (tileContents == ((int)ChessPieceEnum.W_KNIGHT) || tileContents == ((int)ChessPieceEnum.W_BISHOP) || tileContents == ((int)ChessPieceEnum.W_ROOK))
            {
                ChessPiece selectedPiece = null;
                foreach (ChessPiece piece in GameBoardController.Current._defenders)
                {
                    Debug.Log("checking piece: " + piece.ChessPieceType);
                    if (tileContents == ((int)piece.ChessPieceType))
                    {
                        Debug.Log("piece selected: " + piece.ChessPieceType);
                        selectedPiece = piece;
                        break;
                    }
                }

                selectedPiece.SetTileIndicator(true);

                List<Vector2> tiles = selectedPiece.GetPossibleMoves();
                foreach (Vector2 vec in tiles)
                {
                    GameBoardController.Current.GameBoard.GridArray[((int)vec.x), ((int)vec.y)].TileIndicator.SetActive(true);
                }
            }
        }

        if(button == 1)
        {
            GameBoardController.Current.DisableAllIndicators();
            HighlightDefenders();
        }
    }

    private void HighlightDefenders()
    {
        foreach (ChessPiece piece in GameBoardController.Current._defenders)
        {
            Debug.Log("checking piece: " + piece.ChessPieceType);
            piece.SetTileIndicator(true);
        }
    }

    public override void Exit()
    {
        // unhook from events
        ChessGameUIController.ContinueButtonPressed -= OnPressedConfirm;
        InputController.Current.PressedMouse -= OnMousePressed;

        GameBoardController.Current.DisableAllIndicators();

        PlayerTurnEnded?.Invoke();
        Debug.Log("Player Turn: Exiting...");
    }

    void OnPressedConfirm()
    {
        StateMachine.ChangeState<EnemyTurnChessGameState>();
    }
}
