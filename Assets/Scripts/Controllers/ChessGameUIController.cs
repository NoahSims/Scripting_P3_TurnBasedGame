using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChessGameUIController : MonoBehaviour
{
    public static Action ContinueButtonPressed;
    public static Action ReturnToMenuButtonPressed;
    public static Action DefenderPlacementKnightButtonPressed;
    public static Action DefenderPlacementBishopButtonPressed;
    public static Action DefenderPlacementRookButtonPressed;

    [SerializeField] Text _enemyThinkingText = null;
    [SerializeField] Text _playerTurnCounter = null;
    [SerializeField] Text _boardGeneratingText = null;
    [SerializeField] Canvas _winScreen = null;
    [SerializeField] Button _skipTurnButton = null;

    [Header("Defender Placement State")]
    [SerializeField] Canvas _defenderPlacementScreen = null;
    [SerializeField] Button defenderPlacementKnightButton = null;
    [SerializeField] GameObject defenderPlacementKnightObject = null;
    [SerializeField] Button defenderPlacementBishopButton = null;
    [SerializeField] GameObject defenderPlacementBishopObject = null;
    [SerializeField] Button defenderPlacementRookButton = null;
    [SerializeField] GameObject defenderPlacementRookObject = null;
    [SerializeField] Button defenderPlacementContinueButton = null;

    private void OnEnable()
    {
        PlayerTurnChessGameState.PlayerTurnBegan += OnPlayerTurnBegan;
        PlayerTurnChessGameState.PlayerTurnEnded += OnPlayerTurnEnded;
        EnemyTurnChessGameState.EnemyTurnBegan += OnEnemyTurnBegan;
        EnemyTurnChessGameState.EnemyTurnEnded += OnEnemyTurnEnded;
        SetupChessGameState.SetupBegan += OnSetupBegan;
        SetupChessGameState.SetupEnded += OnSetupEnded;
        DefenderPlacementChessGameState.DefenderPlacementBegan += OnDefenderPlacementBegan;
        DefenderPlacementChessGameState.DefenderPlacementEnded += OnDefenderPlacementEnded;
        DefenderPlacementChessGameState.DefenderPlacementCancelHolding += OnDefenderPlacementCancelHolding;
        DefenderPlacementChessGameState.DefenderPlacementContinueReady += OnDefenderPlacementContinueReady;
        WinChessGameState.PlayerWon += OnPlayerWon;
    }

    private void OnDisable()
    {
        EnemyTurnChessGameState.EnemyTurnBegan -= OnEnemyTurnBegan;
        EnemyTurnChessGameState.EnemyTurnEnded -= OnEnemyTurnEnded;
        EnemyTurnChessGameState.EnemyTurnBegan -= OnEnemyTurnBegan;
        EnemyTurnChessGameState.EnemyTurnEnded -= OnEnemyTurnEnded;
        SetupChessGameState.SetupBegan -= OnSetupBegan;
        SetupChessGameState.SetupEnded -= OnSetupEnded;
        DefenderPlacementChessGameState.DefenderPlacementBegan -= OnDefenderPlacementBegan;
        DefenderPlacementChessGameState.DefenderPlacementEnded -= OnDefenderPlacementEnded;
        DefenderPlacementChessGameState.DefenderPlacementCancelHolding -= OnDefenderPlacementCancelHolding;
        DefenderPlacementChessGameState.DefenderPlacementContinueReady -= OnDefenderPlacementContinueReady;
        WinChessGameState.PlayerWon -= OnPlayerWon;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        // make sure text is disabled on start
        _playerTurnCounter.gameObject.SetActive(false);
        _enemyThinkingText.gameObject.SetActive(false);
        _boardGeneratingText.gameObject.SetActive(false);
        _defenderPlacementScreen.gameObject.SetActive(false);
        defenderPlacementContinueButton.gameObject.SetActive(false);
        _winScreen.gameObject.SetActive(false);
        _skipTurnButton.gameObject.SetActive(false);
    }

    #region Buttons
    public void ContinueButton()
    {
        ContinueButtonPressed?.Invoke();
    }

    public void ReturnToMenuButton()
    {
        ReturnToMenuButtonPressed?.Invoke();
    }

    public void DefenderPlacementButton(int pieceNum, GameObject chessPiece, Button button)
    {
        chessPiece.SetActive(false);
        button.gameObject.SetActive(false);

        switch(pieceNum)
        {
            case (1):
                DefenderPlacementKnightButtonPressed?.Invoke();
                break;
            case (2):
                DefenderPlacementBishopButtonPressed?.Invoke();
                break;
            case (3):
                DefenderPlacementRookButtonPressed?.Invoke();
                break;
        }
    }

    public void DefenderPlacementKnightButton()
    {
        defenderPlacementKnightButton.interactable = false;
        defenderPlacementKnightObject.SetActive(false);
        DefenderPlacementKnightButtonPressed?.Invoke();
    }

    public void DefenderPlacementBishopButton()
    {
        defenderPlacementBishopButton.interactable = false;
        defenderPlacementBishopObject.SetActive(false);
        DefenderPlacementBishopButtonPressed?.Invoke();
    }

    public void DefenderPlacementRookButton()
    {
        defenderPlacementRookButton.interactable = false;
        defenderPlacementRookObject.SetActive(false);
        DefenderPlacementRookButtonPressed?.Invoke();
    }

    public void OnDefenderPlacementCancelHolding(int pieceNum)
    {
        switch(pieceNum)
        {
            case (1):
                defenderPlacementKnightButton.interactable = true;
                defenderPlacementKnightObject.SetActive(true);
                break;
            case (2):
                defenderPlacementBishopButton.interactable = true;
                defenderPlacementBishopObject.SetActive(true);
                break;
            case (3):
                defenderPlacementRookButton.interactable = true;
                defenderPlacementRookObject.SetActive(true);
                break;
        }
    }
    #endregion

    #region Events
    void OnPlayerTurnBegan(int turnNumber)
    {
        _playerTurnCounter.gameObject.SetActive(true);
        _playerTurnCounter.text = "Player turn: " + turnNumber.ToString();
        _skipTurnButton.gameObject.SetActive(true);
    }

    void OnPlayerTurnEnded()
    {
        _playerTurnCounter.gameObject.SetActive(false);
        _skipTurnButton.gameObject.SetActive(false);
    }

    void OnEnemyTurnBegan()
    {
        _enemyThinkingText.gameObject.SetActive(true);
    }

    void OnEnemyTurnEnded()
    {
        _enemyThinkingText.gameObject.SetActive(false);
    }

    void OnSetupBegan()
    {
        _boardGeneratingText.gameObject.SetActive(true);
    }

    void OnSetupEnded()
    {
        _boardGeneratingText.gameObject.SetActive(false);
    }

    void OnPlayerWon()
    {
        _winScreen.gameObject.SetActive(true);
    }

    void OnDefenderPlacementBegan()
    {
        _defenderPlacementScreen.gameObject.SetActive(true);
    }

    void OnDefenderPlacementEnded()
    {
        _defenderPlacementScreen.gameObject.SetActive(false);
    }

    void OnDefenderPlacementContinueReady(bool status)
    {
        defenderPlacementContinueButton.gameObject.SetActive(status);
    }
    #endregion
}
