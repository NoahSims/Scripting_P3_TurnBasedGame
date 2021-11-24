using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerTurnChessGameState : ChessGameState
{
    public static event Action<int> PlayerTurnBegan;
    public static event Action PlayerTurnEnded;

    private ChessPiece _selectedPiece = null;

    public override void Enter()
    {
        Debug.Log("Player Turn: ...Entering");
        PlayerTurnBegan?.Invoke(StateMachine.RoundNumber);

        // hook into events
        ChessGameUIController.ContinueButtonPressed += OnPressedConfirm;
        InputController.Current.PressedMouse += OnMousePressed;

        // initiate turn
        _selectedPiece = null;
        HighlightDefenders();
    }

    private void OnMousePressed(int button)
    {
        if (button == 0)
        {
            // get tile clicked on
            Vector2 tile = GameBoardController.Current.GetTileFromWorldSpace(InputController.Current.GetMouseWorldPosition());
            //int tileContents = GameBoardController.Current.CheckTileContents(((int)tile.x), ((int)tile.y));
            ChessPiece piece = GameBoardController.Current.GameBoard.GridArray[(int)tile.x, (int)tile.y].TilePiece;

            // if a defender was clicked on
            if (piece?.ChessPieceTeam == ChessTeamEnum.DEFENDER)
            {
                GameBoardController.Current.DisableAllIndicators();
                Debug.Log("piece selected: " + piece.ChessPieceType);
                _selectedPiece = piece;
                /*
                // figure out which piece is selected
                foreach (ChessPiece piece in GameBoardController.Current._defenders)
                {
                    //Debug.Log("checking piece: " + piece.ChessPieceType);
                    if (tileContents == ((int)piece.ChessPieceType))
                    {
                        Debug.Log("piece selected: " + piece.ChessPieceType);
                        _selectedPiece = piece;
                        break;
                    }
                }
                */

                // highlight possible moves
                //_selectedPiece.SetTileIndicator(true);

                List<Vector2> tiles = _selectedPiece.GetPossibleMoves();
                foreach (Vector2 vec in tiles)
                {
                    GameBoardController.Current.GameBoard.GridArray[((int)vec.x), ((int)vec.y)].TileIndicator.SetActive(true);
                }

            }
            else if(_selectedPiece != null && GameBoardController.Current.GameBoard.GridArray[((int)tile.x), ((int)tile.y)].TileIndicator.activeSelf)
            {
                _selectedPiece.MoveChessPiece(((int)tile.x), ((int)tile.y));
                StateMachine.ChangeState<EnemyTurnChessGameState>();
            }
        }

        if(button == 1)
        {
            GameBoardController.Current.DisableAllIndicators();
            HighlightDefenders();
            _selectedPiece = null;
        }
    }

    private void HighlightDefenders()
    {
        foreach (ChessPiece piece in GameBoardController.Current._defenders)
        {
            //Debug.Log("checking piece: " + piece.ChessPieceType);
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
