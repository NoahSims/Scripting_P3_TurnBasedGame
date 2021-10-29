using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChessGameSM))]
public class ChessGameState : State
{
    protected ChessGameSM StateMachine { get; private set; }

    private void Awake()
    {
        StateMachine = GetComponent<ChessGameSM>();
    }
}
