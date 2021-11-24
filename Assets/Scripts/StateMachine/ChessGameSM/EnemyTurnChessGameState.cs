using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyTurnChessGameState : ChessGameState
{
    public static event Action EnemyTurnBegan;
    public static event Action EnemyTurnEnded;

    [SerializeField] float _pauseDuration = 1.5f;

    private bool hasClicked = false;

    public override void Enter()
    {
        Debug.Log("Enemy turn: ...Enter");
        EnemyTurnBegan?.Invoke();

        // hook into events
        InputController.Current.PressedMouse += OnMousePressed;

        hasClicked = false;

        StartCoroutine(EnemyThinkingRoutine(_pauseDuration));
    }

    private void OnMousePressed(int button)
    {
        hasClicked = true;
    }

    public override void Exit()
    {
        GameBoardController.Current.DisableAllIndicators();

        // unhook from events
        InputController.Current.PressedMouse -= OnMousePressed;

        Debug.Log("Enemy Turn: Exit...");
    }

    IEnumerator EnemyThinkingRoutine(float pauseDuration)
    {
        Debug.Log("Enemy thinking...");

        MinMaxTree tree = new MinMaxTree(3);
        tree.DetermineMove();

        yield return new WaitUntil(() => hasClicked);
        //yield return new WaitForSeconds(pauseDuration);

        Debug.Log("Enemy performs action");
        EnemyTurnEnded?.Invoke();
        // turn over. Go back to Player.
        StateMachine.ChangeState<EndRoundChessGameState>();
    }
}
