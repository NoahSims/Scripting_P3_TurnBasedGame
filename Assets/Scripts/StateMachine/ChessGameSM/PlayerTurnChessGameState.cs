using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerTurnChessGameState : ChessGameState
{
    public static event Action PlayerTurnBegan;
    public static event Action<bool> PlayerTurnContinueReady;
    public static event Action PlayerTurnEnded;

    private ChessPiece _selectedPiece = null;
    private Vector2 _selectedPiecePreviousPos;
    private ChessPiece _capturedPiece = null;

    private bool _moveMade = false;

    public override void Enter()
    {
        Debug.Log("Player Turn: ...Entering");
        PlayerTurnBegan?.Invoke();

        // hook into events
        ChessGameUIController.ContinueButtonPressed += OnPressedConfirm;
        ChessGameUIController.UndoButtonPressed += OnPressedUndo;
        InputController.Current.PressedMouse += OnMousePressed;

        // initiate turn
        _selectedPiece = null;
        _capturedPiece = null;
        _moveMade = false;
        HighlightDefenders();
    }

    private void OnMousePressed(int button)
    {
        if (button == 0 && !_moveMade)
        {
            // get tile clicked on
            Vector2 tile = GameBoardController.Current.GetTileFromWorldSpace(InputController.Current.GetMouseWorldPosition());
            //int tileContents = GameBoardController.Current.CheckTileContents(((int)tile.x), ((int)tile.y));
            ChessPiece piece = GameBoardController.Current.GetPieceFromTile((int)tile.x, (int)tile.y);

            // if a defender was clicked on
            if (piece?.ChessPieceTeam == ChessTeamEnum.DEFENDER && piece.inPlay)
            {
                GameBoardController.Current.DisableAllIndicators();
                Debug.Log("piece selected: " + piece.ChessPieceType);
                _selectedPiece = piece;

                List<Vector2> tiles = _selectedPiece.GetPossibleMoves();
                foreach (Vector2 vec in tiles)
                {
                    GameBoardController.Current.GameBoard.GridArray[((int)vec.x), ((int)vec.y)].TileIndicator.SetActive(true);
                }

            }
            else if(_selectedPiece != null && GameBoardController.Current.GameBoard.GridArray[((int)tile.x), ((int)tile.y)].TileIndicator.activeSelf)
            {
                // set indicators before move
                GameBoardController.Current.DisableAllIndicators();
                _selectedPiece.SetTileIndicator(true);
                // move
                _selectedPiecePreviousPos = new Vector2(_selectedPiece.xPos, _selectedPiece.zPos);
                _capturedPiece = _selectedPiece.MoveChessPiece(((int)tile.x), ((int)tile.y));
                // set indicators after move
                _selectedPiece.SetTileIndicator(true);
                // continue ?
                PlayerTurnContinueReady?.Invoke(true);
                _moveMade = true;
            }
        }

        if(button == 1 && !_moveMade)
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
            if(piece.gameObject.activeSelf)
                piece.SetTileIndicator(true);
        }
    }

    void OnPressedConfirm()
    {
        StateMachine.ChangeState<EnemyTurnChessGameState>();
    }

    void OnPressedUndo()
    {
        if (_moveMade)
        {
            PlayerTurnContinueReady?.Invoke(false);
            _moveMade = false;

            Debug.Log("selected piece = " + _selectedPiece.gameObject.name);

            //undo move
            _selectedPiece.MoveChessPiece(((int)_selectedPiecePreviousPos.x), ((int)_selectedPiecePreviousPos.y));

            // reset captured piece
            if (_capturedPiece != null)
            {
                _capturedPiece.gameObject.SetActive(true);
                _capturedPiece.inPlay = true;
                _capturedPiece.SetChessPiecePosition(_capturedPiece.xPos, _capturedPiece.zPos, true);
            }

            // set indicators
            GameBoardController.Current.DisableAllIndicators();
            HighlightDefenders();
        }
    }

    public override void Exit()
    {
        // unhook from events
        ChessGameUIController.ContinueButtonPressed -= OnPressedConfirm;
        ChessGameUIController.UndoButtonPressed -= OnPressedUndo;
        InputController.Current.PressedMouse -= OnMousePressed;

        GameBoardController.Current.DisableAllIndicators();

        PlayerTurnEnded?.Invoke();
        Debug.Log("Player Turn: Exiting...");
    }
}
