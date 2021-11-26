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

        if(!GameBoardController.Current._whiteKing.GetComponent<ChessPiece>().inPlay)
        {
            Debug.Log("King Captured, ending game");
            StateMachine.ChangeState<LoseChessGameState>();
            yield break;
        }
        else if(StateMachine.RoundNumber >= _finalRoundNumber)
        {
            Debug.Log("Round Max reached, ending game");
            StateMachine.ChangeState<WinChessGameState>();
            yield break;
        }
        else
        {
            foreach (ChessPiece defender in GameBoardController.Current._defenders)
            {
                if (!defender.inPlay)
                {
                    StateMachine.ChangeState<DefenderRespawnChessGameState>();
                    yield break;
                }
            }

            StateMachine.ChangeState<PlayerTurnChessGameState>();
        }
    }

    public override void Exit()
    {
        Debug.Log("End Round: Exiting ...");
    }
}
