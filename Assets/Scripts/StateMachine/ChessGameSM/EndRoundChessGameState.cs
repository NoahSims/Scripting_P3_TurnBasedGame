using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EndRoundChessGameState : ChessGameState
{
    public static event Action<int> EndRound;

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
        else if(StateMachine.RoundNumber > StateMachine.MaxRounds)
        {
            Debug.Log("Round Max reached, ending game");
            StateMachine.ChangeState<WinChessGameState>();
            yield break;
        }
        else
        {
            EndRound?.Invoke(StateMachine.MaxRounds - StateMachine.RoundNumber);

            foreach (ChessPiece defender in GameBoardController.Current._defenders)
            {
                // If a defender needs to respawn and there are live pawns available, go to respawn
                if (!defender.inPlay && checkForLivePieces(GameBoardController.Current._whitePawns))
                {
                    StateMachine.ChangeState<DefenderRespawnChessGameState>();
                    yield break;
                }
            }

            if (checkForLivePieces(GameBoardController.Current._defenders))
                StateMachine.ChangeState<PlayerTurnChessGameState>();
            else
                StateMachine.ChangeState<EnemyTurnChessGameState>();
        }
    }

    private bool checkForLivePieces(List<ChessPiece> pieces)
    {
        foreach (ChessPiece piece in pieces)
        {
            if (piece.inPlay)
                return true;
        }
        return false;
    }

    public override void Exit()
    {
        Debug.Log("End Round: Exiting ...");
    }
}
