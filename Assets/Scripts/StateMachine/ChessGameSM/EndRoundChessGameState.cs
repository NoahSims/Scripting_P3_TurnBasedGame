using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoundChessGameState : ChessGameState
{
    [SerializeField] private int _finalRoundNumber = 15;

    public override void Enter()
    {
        Debug.Log("End Round: ... Enter");

        StateMachine.RoundNumber++;
        StartCoroutine(CheckGameState());
    }

    IEnumerator CheckGameState()
    {
        yield return new WaitForEndOfFrame();

        if(StateMachine.RoundNumber >= _finalRoundNumber)
        {
            Debug.Log("Round Max reached, ending game");
            StateMachine.ChangeState<WinChessGameState>();
        }
        else
        {
            StateMachine.ChangeState<PlayerTurnChessGameState>();
        }
    }

    public override void Exit()
    {
        Debug.Log("End Round: Exiting ...");
    }
}
