using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DefenderPlacementChessGameState : ChessGameState
{
    public static event Action DefenderPlacementBegan;
    public static event Action DefenderPlacementEnded;
    public static event Action<int> DefenderPlacementCancelHolding;
    public static event Action<bool> DefenderPlacementContinueReady;

    [SerializeField] GameObject _mouseTracker = null;
    [SerializeField] GameObject _defenderKnight = null;
    [SerializeField] GameObject _defenderBishop = null;
    [SerializeField] GameObject _defenderRook = null;

    private int _holding = 0;
    private bool _defenderKnightPlaced = false;
    private bool _defenderBishopPlaced = false;
    private bool _defenderRookPlaced = false;
    private bool _readyToContinue = false;

    public override void Enter()
    {
        Debug.Log("Defender Placement: ... Entering");
        DefenderPlacementBegan?.Invoke();

        GameBoardController.Current.SetIndicators(0, 3, 0, GameBoardController.Current.GameBoard.Width);

        InputController.Current.PressedMouse += OnMousePressed;
        ChessGameUIController.DefenderPlacementKnightButtonPressed += OnDefenderKnightButton;
        ChessGameUIController.DefenderPlacementBishopButtonPressed += OnDefenderBishopButton;
        ChessGameUIController.DefenderPlacementRookButtonPressed += OnDefenderRookButton;
        ChessGameUIController.ContinueButtonPressed += OnContinue;
    }

    public override void Tick()
    {
        Vector2 tileCoords = GameBoardController.Current.GetTileFromWorldSpace(InputController.Current.GetMouseWorldPosition());
        _mouseTracker.transform.position = GameBoardController.Current.GetChessWorldSpaceFromTile((int)tileCoords.x, (int)tileCoords.y);

        if(!_readyToContinue && _defenderKnightPlaced && _defenderBishopPlaced && _defenderRookPlaced)
        {
            _readyToContinue = true;
            DefenderPlacementContinueReady?.Invoke(true);
        }
        else if(_readyToContinue && !(_defenderKnightPlaced && _defenderBishopPlaced && _defenderRookPlaced))
        {
            _readyToContinue = false;
            DefenderPlacementContinueReady?.Invoke(false);
        }
    }

    private void OnMousePressed(int buttonNum)
    {
        if(_holding != 0 && buttonNum == 1)
        {
            OnDefenderPlacementCancelHolding(_holding);
        }

        else if(_holding != 0 && buttonNum == 0)
        {
            Vector2 tileCoords = GameBoardController.Current.GetTileFromWorldSpace(InputController.Current.GetMouseWorldPosition());

            switch (_holding)
            {
                case (1):
                    if (GameBoardController.Current.AttemptPlacePiece((int)tileCoords.x, (int)tileCoords.y, ChessPieceEnum.W_KNIGHT))
                    {
                        _defenderKnight.transform.parent = null;
                        _holding = 0;
                        _defenderKnightPlaced = true;
                    }
                    break;
                case (2):
                    if (GameBoardController.Current.AttemptPlacePiece((int)tileCoords.x, (int)tileCoords.y, ChessPieceEnum.W_BISHOP))
                    {
                        _defenderBishop.transform.parent = null;
                        _holding = 0;
                        _defenderBishopPlaced = true;
                    }
                    break;
                case (3):
                    if (GameBoardController.Current.AttemptPlacePiece((int)tileCoords.x, (int)tileCoords.y, ChessPieceEnum.W_ROOK))
                    {
                        _defenderRook.transform.parent = null;
                        _holding = 0;
                        _defenderRookPlaced = true;
                    }
                    break;
            }
        }

        else if(buttonNum == 0)
        {
            Vector2 tileCoords = GameBoardController.Current.GetTileFromWorldSpace(InputController.Current.GetMouseWorldPosition());
            int piece = GameBoardController.Current.CheckTileContents((int)tileCoords.x, (int)tileCoords.y);
            switch(piece)
            {
                case (((int)ChessPieceEnum.W_KNIGHT)):
                    OnDefenderKnightButton();
                    GameBoardController.Current.GameBoard.GridArray[(int)tileCoords.x, (int)tileCoords.y].TileContents = ((int)ChessPieceEnum.EMPTY);
                    GameBoardController.Current.GameBoard.GridArray[(int)tileCoords.x, (int)tileCoords.y].TileIndicator.SetActive(true);
                    break;
                case (((int)ChessPieceEnum.W_BISHOP)):
                    OnDefenderBishopButton();
                    GameBoardController.Current.GameBoard.GridArray[(int)tileCoords.x, (int)tileCoords.y].TileContents = ((int)ChessPieceEnum.EMPTY);
                    GameBoardController.Current.GameBoard.GridArray[(int)tileCoords.x, (int)tileCoords.y].TileIndicator.SetActive(true);
                    break;
                case (((int)ChessPieceEnum.W_ROOK)):
                    OnDefenderRookButton();
                    GameBoardController.Current.GameBoard.GridArray[(int)tileCoords.x, (int)tileCoords.y].TileContents = ((int)ChessPieceEnum.EMPTY);
                    GameBoardController.Current.GameBoard.GridArray[(int)tileCoords.x, (int)tileCoords.y].TileIndicator.SetActive(true);
                    break;
            }
        }
    }

    private void OnContinue()
    {
        StateMachine.ChangeState<EnemyTurnChessGameState>();
    }

    private void OnDefenderKnightButton()
    {
        if (_holding != 0)
            OnDefenderPlacementCancelHolding(_holding);

        _holding = 1;
        _defenderKnightPlaced = false;

        _defenderKnight.SetActive(true);
        _defenderKnight.transform.parent = _mouseTracker.transform;
        _defenderKnight.transform.localPosition = new Vector3(0, _defenderKnight.transform.localPosition.y, 0);
    }

    private void OnDefenderBishopButton()
    {
        if (_holding != 0)
            OnDefenderPlacementCancelHolding(_holding);

        _holding = 2;
        _defenderBishopPlaced = false;

        _defenderBishop.SetActive(true);
        _defenderBishop.transform.parent = _mouseTracker.transform;
        _defenderBishop.transform.localPosition = new Vector3(0, _defenderBishop.transform.localPosition.y, 0);
    }

    private void OnDefenderRookButton()
    {
        if (_holding != 0)
            OnDefenderPlacementCancelHolding(_holding);

        _holding = 3;
        _defenderRookPlaced = false;

        _defenderRook.SetActive(true);
        _defenderRook.transform.parent = _mouseTracker.transform;
        _defenderRook.transform.localPosition = new Vector3(0, _defenderRook.transform.localPosition.y, 0);
    }

    private void OnDefenderPlacementCancelHolding(int pieceNum)
    {
        DefenderPlacementCancelHolding?.Invoke(pieceNum);

        switch(pieceNum)
        {
            case (1):
                _defenderKnight.transform.parent = null;
                _defenderKnight.SetActive(false);
                break;
            case (2):
                _defenderBishop.transform.parent = null;
                _defenderBishop.SetActive(false);
                break;
            case (3):
                _defenderRook.transform.parent = null;
                _defenderRook.SetActive(false);
                break;
        }

        _holding = 0;
    }

    public override void Exit()
    {
        InputController.Current.PressedMouse -= OnMousePressed;
        ChessGameUIController.ContinueButtonPressed -= OnContinue;
        ChessGameUIController.DefenderPlacementKnightButtonPressed -= OnDefenderKnightButton;
        ChessGameUIController.DefenderPlacementBishopButtonPressed -= OnDefenderBishopButton;
        ChessGameUIController.DefenderPlacementRookButtonPressed -= OnDefenderRookButton;

        GameBoardController.Current.DisableAllIndicators();

        DefenderPlacementEnded?.Invoke();
        Debug.Log("Defender Placement: Exiting ...");
    }
}
