using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyTurnChessGameState : ChessGameState
{
    public static event Action EnemyTurnBegan;
    public static event Action EnemyTurnEnded;

    [SerializeField] float _pauseDuration = 1.5f;

    public override void Enter()
    {
        Debug.Log("Enemy turn: ...Enter");
        EnemyTurnBegan?.Invoke();

        StartCoroutine(EnemyThinkingRoutine(_pauseDuration));
    }

    public override void Exit()
    {
        Debug.Log("Enemy Turn: Exit...");
    }

    IEnumerator EnemyThinkingRoutine(float pauseDuration)
    {
        Debug.Log("Enemy thinking...");
        yield return new WaitForSeconds(pauseDuration);

        Debug.Log("Enemy performs action");
        EnemyTurnEnded?.Invoke();
        // turn over. Go back to Player.
        StateMachine.ChangeState<EndRoundChessGameState>();
    }
}
