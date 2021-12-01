using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessGameSM : StateMachine
{
    [SerializeField] InputController _input;
    public InputController Input => _input;

    private int _roundNumber = 0;
    public int RoundNumber { get => _roundNumber; set => _roundNumber = value; }

    [SerializeField] private int _maxRounds = 15;
    public int MaxRounds { get => _maxRounds; }

    private void Start()
    {
        ChangeState<SetupChessGameState>();
    }
}
