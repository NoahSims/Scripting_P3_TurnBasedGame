using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChessGameUIController : MonoBehaviour
{
    public static Action SkipButtonPressed;
    public static Action ReturnToMenuButtonPressed;

    [SerializeField] Canvas _defenderPlacementScreen = null;
    [SerializeField] Text _enemyThinkingText = null;
    [SerializeField] Text _playerTurnCounter = null;
    [SerializeField] Text _boardGeneratingText = null;
    [SerializeField] Canvas _winScreen = null;
    [SerializeField] Button _skipTurnButton = null;

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
        _winScreen.gameObject.SetActive(false);
        _skipTurnButton.gameObject.SetActive(false);
    }

    public void SkipButton()
    {
        SkipButtonPressed?.Invoke();
    }

    public void ReturnToMenuButton()
    {
        ReturnToMenuButtonPressed?.Invoke();
    }

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
    #endregion
}
