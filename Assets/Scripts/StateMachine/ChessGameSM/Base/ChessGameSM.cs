using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessGameSM : StateMachine
{
    [SerializeField] InputController _input;
    public InputController Input => _input;

    private int _roundNumber = 0;
    public int RoundNumber => _roundNumber;

    private void Start()
    {
        ChangeState<SetupChessGameState>();
    }
}
