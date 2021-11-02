using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChessGameUIController : MonoBehaviour
{
    [SerializeField] Text _enemyThinkingTextUI = null;
    [SerializeField] Text _playerTurnCounter = null;
    [SerializeField] Text _winScreen = null;

    private void OnEnable()
    {
        PlayerTurnChessGameState.PlayerTurnBegan += OnPlayerTurnBegan;
        PlayerTurnChessGameState.PlayerTurnEnded += OnPlayerTurnEnded;
        EnemyTurnChessGameState.EnemyTurnBegan += OnEnemyTurnBegan;
        EnemyTurnChessGameState.EnemyTurnEnded += OnEnemyTurnEnded;
        WinChessGameState.PlayerWon += OnPlayerWon;
    }

    private void OnDisable()
    {
        EnemyTurnChessGameState.EnemyTurnBegan -= OnEnemyTurnBegan;
        EnemyTurnChessGameState.EnemyTurnEnded -= OnEnemyTurnEnded;
        EnemyTurnChessGameState.EnemyTurnBegan -= OnEnemyTurnBegan;
        EnemyTurnChessGameState.EnemyTurnEnded -= OnEnemyTurnEnded;
        WinChessGameState.PlayerWon -= OnPlayerWon;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        // make sure text is disabled on start
        _playerTurnCounter.gameObject.SetActive(false);
        _enemyThinkingTextUI.gameObject.SetActive(false);
        _winScreen.gameObject.SetActive(false);
    }

    void OnPlayerTurnBegan(int turnNumber)
    {
        _playerTurnCounter.gameObject.SetActive(true);
        _playerTurnCounter.text = "Player turn: " + turnNumber.ToString();
    }

    void OnPlayerTurnEnded()
    {
        _playerTurnCounter.gameObject.SetActive(false);
    }

    void OnEnemyTurnBegan()
    {
        _enemyThinkingTextUI.gameObject.SetActive(true);
    }

    void OnEnemyTurnEnded()
    {
        _enemyThinkingTextUI.gameObject.SetActive(false);
    }

    void OnPlayerWon()
    {
        _winScreen.gameObject.SetActive(true);
    }
}
