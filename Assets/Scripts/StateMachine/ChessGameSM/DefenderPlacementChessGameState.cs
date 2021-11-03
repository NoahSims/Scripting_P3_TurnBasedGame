using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DefenderPlacementChessGameState : ChessGameState
{
    public static event Action DefenderPlacementBegan;
    public static event Action DefenderPlacementEnded;

    public override void Enter()
    {
        Debug.Log("Defender Placement: ... Entering");
        DefenderPlacementBegan?.Invoke();

        ChessGameUIController.SkipButtonPressed += OnSkip;
    }

    private void OnSkip()
    {
        StateMachine.ChangeState<EnemyTurnChessGameState>();
    }

    public override void Exit()
    {
        ChessGameUIController.SkipButtonPressed -= OnSkip;

        DefenderPlacementEnded?.Invoke();
        Debug.Log("Defender Placement: Exiting ...");
    }
}
