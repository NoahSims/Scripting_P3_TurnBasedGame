using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SetupChessGameState : ChessGameState
{
    public static event Action SetupBegan;
    public static event Action SetupEnded;

    [SerializeField] int _numberOfObjectives = 3;
    [SerializeField] int _startingEnemyNumber = 3;

    public override void Enter()
    {
        Debug.Log("Setup: ...Entering");
        SetupBegan?.Invoke();

        StartCoroutine(SpawnPieces());
    }

    IEnumerator SpawnPieces()
    {
        yield return new WaitForSeconds(0.25f);
        Debug.Log("Spawning White King");
        GameBoardController.Current.SpawnWhiteKing();

        Debug.Log("Creating " + _numberOfObjectives + " objectives.");
        for(int i = 0; i < _numberOfObjectives; i++)
        {
            yield return new WaitForSeconds(0.25f);
            GameBoardController.Current.SpawnWhitePawn();
        }

        Debug.Log("Creating " + _startingEnemyNumber + "enemies.");
        for (int i = 0; i < _startingEnemyNumber; i++)
        {
            yield return new WaitForSeconds(0.25f);
            GameBoardController.Current.SpawnBlackPiece();
        }

        StateMachine.ChangeState<DefenderPlacementChessGameState>();
    }

    public override void Exit()
    {
        SetupEnded?.Invoke();
        Debug.Log("Setup: Exiting...");
    }
}
