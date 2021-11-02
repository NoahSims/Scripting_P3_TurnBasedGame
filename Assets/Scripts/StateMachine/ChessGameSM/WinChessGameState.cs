using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class WinChessGameState : ChessGameState
{
    public static event Action PlayerWon;

    public override void Enter()
    {
        Debug.Log("Win: ... Entering");
        PlayerWon?.Invoke();

        StateMachine.Input.PressedMouse += OnPressedConfirm;
    }

    public override void Exit()
    {
        Debug.Log("Win: Exiting ...");
        StateMachine.Input.PressedMouse -= OnPressedConfirm;
    }

    void OnPressedConfirm()
    {
        Exit();
        SceneManager.LoadScene("MainMenu");
    }
}
