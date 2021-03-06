using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DefenderRespawnChessGameState : ChessGameState
{
    public static event Action DefenderRespawnBegan;
    public static event Action DefenderRespawnEnded;
    public static event Action<int> DefenderMenuReset;
    public static event Action<bool> DefenderRespawnContinueReady;

    private ChessPiece _missingPiece = null;
    private ChessPiece _spentPawn = null;

    private bool _isMissingPiecePlaced = false;

    public override void Enter()
    {
        Debug.Log("Defender Respawn: ... Entering");
        DefenderRespawnBegan?.Invoke();

        _isMissingPiecePlaced = false;

        DetermineMissingPiece();
        SetUI();
        SetPawnIndicators();

        // hook into events
        InputController.Current.PressedMouse += OnMousePressed;
        ChessGameUIController.ContinueButtonPressed += OnContinue;
        ChessGameUIController.UndoButtonPressed += OnUndo;
    }

    private void DetermineMissingPiece()
    {
        foreach (ChessPiece defender in GameBoardController.Current._defenders)
        {
            if (!defender.inPlay)
                _missingPiece = defender;
        }
    }

    private void SetUI()
    {
        DefenderMenuReset?.Invoke(((int)_missingPiece.ChessPieceType));
    }

    private void SetPawnIndicators()
    {
        foreach (ChessPiece pawn in GameBoardController.Current._whitePawns)
        {
            if (pawn.inPlay)
                pawn.SetTileIndicator(true);
        }
    }

    private void OnMousePressed(int buttonNum)
    {
        if(buttonNum == 0 && !_isMissingPiecePlaced)
        {
            // get tile clicked on
            Vector2 tile = GameBoardController.Current.GetTileFromWorldSpace(InputController.Current.GetMouseWorldPosition());
            ChessPiece piece = GameBoardController.Current.GetPieceFromTile((int)tile.x, (int)tile.y);

            if(piece?.ChessPieceType == ChessPieceEnum.PAWN && piece?.ChessPieceTeam == ChessTeamEnum.WHITE)
            {
                // capture pawn
                _spentPawn = piece;
                _spentPawn.PieceCaptured();
                // respawn missing piece
                _missingPiece.inPlay = true;
                _missingPiece.gameObject.SetActive(true);
                _missingPiece.SetChessPiecePosition(_spentPawn.xPos, _spentPawn.zPos, true);

                GameBoardController.Current.DisableAllIndicators();
                DefenderRespawnContinueReady?.Invoke(true);
                _isMissingPiecePlaced = true;
                SetUI();
            }
        }
    }

    private void OnContinue()
    {
        StateMachine.ChangeState<PlayerTurnChessGameState>();
    }

    private void OnUndo()
    {
        if (_isMissingPiecePlaced)
        {
            // remove placed defender
            _missingPiece.PieceCaptured();

            // replace pawn
            _spentPawn.gameObject.SetActive(true);
            _spentPawn.inPlay = true;
            _spentPawn.SetChessPiecePosition(_spentPawn.xPos, _spentPawn.zPos, true);

            // reset state
            SetPawnIndicators();
            SetUI();
            DefenderRespawnContinueReady?.Invoke(false);
            _isMissingPiecePlaced = false;
        }
    }

    public override void Exit()
    {
        // unhook from events
        InputController.Current.PressedMouse -= OnMousePressed;
        ChessGameUIController.ContinueButtonPressed -= OnContinue;
        ChessGameUIController.UndoButtonPressed -= OnUndo;

        DefenderRespawnEnded?.Invoke();
        Debug.Log("Defender Respawn: Exiting ...");
    }
}
