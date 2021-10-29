using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessGameSM : StateMachine
{
    [SerializeField] InputController _input;
    public InputController Input => _input;

    private void Start()
    {
        ChangeState<SetupChessGameState>();
    }
}
