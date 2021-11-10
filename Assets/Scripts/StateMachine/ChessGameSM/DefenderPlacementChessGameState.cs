using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DefenderPlacementChessGameState : ChessGameState
{
    public static event Action DefenderPlacementBegan;
    public static event Action DefenderPlacementEnded;

    [SerializeField] GameObject temp = null;

    public override void Enter()
    {
        Debug.Log("Defender Placement: ... Entering");
        DefenderPlacementBegan?.Invoke();

        GameBoardController.Current.SetIndicators(0, 3, 0, GameBoardController.Current.GameBoard.Width);

        ChessGameUIController.SkipButtonPressed += OnSkip;
    }

    public override void Tick()
    {
        Plane plane = new Plane(Vector3.up, 0);

        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(plane.Raycast(ray, out distance))
        {
            temp.transform.position = ray.GetPoint(distance);
        }
    }

    private void OnSkip()
    {
        StateMachine.ChangeState<EnemyTurnChessGameState>();
    }

    public override void Exit()
    {
        ChessGameUIController.SkipButtonPressed -= OnSkip;

        GameBoardController.Current.DisableAllIndicators();

        DefenderPlacementEnded?.Invoke();
        Debug.Log("Defender Placement: Exiting ...");
    }
}
