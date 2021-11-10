using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DefenderPlacementChessGameState : ChessGameState
{
    public static event Action DefenderPlacementBegan;
    public static event Action DefenderPlacementEnded;
    public static event Action<int> DefenderPlacementCancelHolding;

    [SerializeField] GameObject _mouseTracker = null;
    [SerializeField] GameObject _defenderKnight = null;
    [SerializeField] GameObject _defenderBishop = null;
    [SerializeField] GameObject _defenderRook = null;

    private int _holding = 0;

    public override void Enter()
    {
        Debug.Log("Defender Placement: ... Entering");
        DefenderPlacementBegan?.Invoke();

        GameBoardController.Current.SetIndicators(0, 3, 0, GameBoardController.Current.GameBoard.Width);

        InputController.Current.PressedMouse += OnMousePressed;
        ChessGameUIController.DefenderPlacementKnightButtonPressed += OnDefenderKnightButton;
        ChessGameUIController.DefenderPlacementBishopButtonPressed += OnDefenderBishopButton;
        ChessGameUIController.DefenderPlacementRookButtonPressed += OnDefenderRookButton;
        ChessGameUIController.SkipButtonPressed += OnSkip;
    }

    public override void Tick()
    {
        _mouseTracker.transform.position = InputController.Current.GetMouseWorldPosition();
    }

    private void OnMousePressed(int buttonNum)
    {
        if(_holding != 0 && buttonNum == 1)
        {
            OnDefenderPlacementCancelHolding(_holding);
        }
    }

    private void OnSkip()
    {
        StateMachine.ChangeState<EnemyTurnChessGameState>();
    }

    private void OnDefenderKnightButton()
    {
        if (_holding != 0)
            OnDefenderPlacementCancelHolding(_holding);

        _holding = 1;

        _defenderKnight.SetActive(true);
        _defenderKnight.transform.parent = _mouseTracker.transform;
        _defenderKnight.transform.localPosition = new Vector3(0, _defenderKnight.transform.localPosition.y, 0);
    }

    private void OnDefenderBishopButton()
    {
        if (_holding != 0)
            OnDefenderPlacementCancelHolding(_holding);

        _holding = 2;

        _defenderBishop.SetActive(true);
        _defenderBishop.transform.parent = _mouseTracker.transform;
        _defenderBishop.transform.localPosition = new Vector3(0, _defenderBishop.transform.localPosition.y, 0);
    }

    private void OnDefenderRookButton()
    {
        if (_holding != 0)
            OnDefenderPlacementCancelHolding(_holding);

        _holding = 3;

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
    }

    public override void Exit()
    {
        InputController.Current.PressedMouse -= OnMousePressed;
        ChessGameUIController.SkipButtonPressed -= OnSkip;
        ChessGameUIController.DefenderPlacementKnightButtonPressed -= OnDefenderKnightButton;
        ChessGameUIController.DefenderPlacementBishopButtonPressed -= OnDefenderBishopButton;
        ChessGameUIController.DefenderPlacementRookButtonPressed -= OnDefenderRookButton;

        GameBoardController.Current.DisableAllIndicators();

        DefenderPlacementEnded?.Invoke();
        Debug.Log("Defender Placement: Exiting ...");
    }
}
