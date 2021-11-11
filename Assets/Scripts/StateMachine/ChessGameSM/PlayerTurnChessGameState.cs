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
        //StateMachine.Input.PressedMouse += OnPressedConfirm;
    }

    public override void Exit()
    {
        // unhook from events
        //StateMachine.Input.PressedMouse -= OnPressedConfirm;
        ChessGameUIController.ContinueButtonPressed -= OnPressedConfirm;

        PlayerTurnEnded?.Invoke();
        Debug.Log("Player Turn: Exiting...");
    }

    void OnPressedConfirm()
    {
        StateMachine.ChangeState<EnemyTurnChessGameState>();
    }
}
