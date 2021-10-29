using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupChessGameState : ChessGameState
{
    [SerializeField] int _numberOfObjectives = 3;
    [SerializeField] int _startingEnemyNumber = 3;

    bool _activated = false;

    public override void Enter()
    {
        Debug.Log("Setup: ...Entering");
        Debug.Log("Creating " + _numberOfObjectives + " objectives.");
        Debug.Log("Creating " + _startingEnemyNumber + "enemies.");
        // CANT change state while still in Enter()/Exit() transition!
        // DONT put ChangeState<> here
        _activated = false;
    }

    public override void Tick()
    {
        // a little bit hacky, should ususally use delays or Input
        if(!_activated)
        {
            _activated = true;
            StateMachine.ChangeState<PlayerTurnChessGameState>();
        }
    }

    public override void Exit()
    {
        _activated = false;
        Debug.Log("Setup: Exiting...");
    }
}
