using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DefenderRespawnChessGameState : ChessGameState
{
    public static event Action DefenderRespawnBegan;
    public static event Action DefenderRespawnEnded;
    public static event Action<int> DefenderMenuReset;

    private ChessPiece missingPiece = null;

    public override void Enter()
    {
        Debug.Log("Defender Respawn: ... Entering");
        DefenderRespawnBegan?.Invoke();

        DetermineMissingPiece();
        SetUI();
    }

    private void DetermineMissingPiece()
    {
        foreach (ChessPiece defender in GameBoardController.Current._defenders)
        {
            if (!defender.inPlay)
                missingPiece = defender;
        }
    }

    private void SetUI()
    {
        DefenderMenuReset?.Invoke(((int)missingPiece.ChessPieceType));
    }

    public override void Exit()
    {
        DefenderRespawnEnded?.Invoke();
        Debug.Log("Defender Respawn: Exiting ...");
    }
}
