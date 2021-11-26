using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LoseChessGameState : ChessGameState
{
    public static event Action PlayerLost;

    public override void Enter()
    {
        Debug.Log("Lose: ... Entering");
        PlayerLost?.Invoke();

        ChessGameUIController.ReturnToMenuButtonPressed += OnPressedConfirm;
        //StateMachine.Input.PressedMouse += OnPressedConfirm;
    }

    public override void Exit()
    {
        Debug.Log("Lost: Exiting ...");
        ChessGameUIController.ReturnToMenuButtonPressed -= OnPressedConfirm;
        //StateMachine.Input.PressedMouse -= OnPressedConfirm;
    }

    void OnPressedConfirm()
    {
        Exit();
        SceneManager.LoadScene("MainMenu");
    }
}