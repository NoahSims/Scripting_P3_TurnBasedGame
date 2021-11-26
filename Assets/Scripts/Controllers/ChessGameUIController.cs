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

    [Header("Turn Screens")]
    [SerializeField] Text _enemyThinkingText = null;
    [SerializeField] Text _playerTurnCounter = null;

    [Header("Other Game States")]
    [SerializeField] Text _boardGeneratingText = null;
    [SerializeField] Canvas _winScreen = null;
    [SerializeField] Text _winLoseText = null;

    [Header("Defender Placement State")]
    [SerializeField] Canvas _defenderPlacementScreen = null;
    [SerializeField] Text _defenderPlacementText = null;
    [SerializeField] Button defenderPlacementKnightButton = null;
    [SerializeField] GameObject defenderPlacementKnightObject = null;
    [SerializeField] Button defenderPlacementBishopButton = null;
    [SerializeField] GameObject defenderPlacementBishopObject = null;
    [SerializeField] Button defenderPlacementRookButton = null;
    [SerializeField] GameObject defenderPlacementRookObject = null;
    [SerializeField] Button defenderPlacementContinueButton = null;

    [Header("Defender Respawn State")]
    [SerializeField] Text _defenderRespawnText = null;

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
        DefenderRespawnChessGameState.DefenderRespawnBegan += OnDefenderRespawnBegan;
        DefenderRespawnChessGameState.DefenderRespawnEnded += OnDefenderRespawnEnded;
        DefenderRespawnChessGameState.DefenderMenuReset += OnDefenderRespawnMenuReset;
        DefenderRespawnChessGameState.DefenderRespawnContinueReady += OnDefenderPlacementContinueReady;
        WinChessGameState.PlayerWon += OnPlayerWon;
        LoseChessGameState.PlayerLost += OnPlayerLost;
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
        DefenderRespawnChessGameState.DefenderRespawnBegan -= OnDefenderRespawnBegan;
        DefenderRespawnChessGameState.DefenderRespawnEnded -= OnDefenderRespawnEnded;
        DefenderRespawnChessGameState.DefenderMenuReset -= OnDefenderRespawnMenuReset;
        DefenderRespawnChessGameState.DefenderRespawnContinueReady -= OnDefenderPlacementContinueReady;
        WinChessGameState.PlayerWon -= OnPlayerWon;
        LoseChessGameState.PlayerLost -= OnPlayerLost;
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
        //_skipTurnButton.gameObject.SetActive(false);
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
            case (((int)ChessPieceEnum.KNIGHT)):
                defenderPlacementKnightButton.interactable = true;
                defenderPlacementKnightObject.SetActive(true);
                break;
            case (((int)ChessPieceEnum.BISHOP)):
                defenderPlacementBishopButton.interactable = true;
                defenderPlacementBishopObject.SetActive(true);
                break;
            case (((int)ChessPieceEnum.ROOK)):
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
    }

    void OnPlayerTurnEnded()
    {
        _playerTurnCounter.gameObject.SetActive(false);
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
        _winLoseText.text = "YOU WIN";
    }

    void OnPlayerLost()
    {
        _winScreen.gameObject.SetActive(true);
        _winLoseText.text = "YOU LOSE";
    }

    void OnDefenderPlacementBegan()
    {
        _defenderPlacementScreen.gameObject.SetActive(true);
        _defenderPlacementText.gameObject.SetActive(true);
    }

    void OnDefenderPlacementEnded()
    {
        _defenderPlacementText.gameObject.SetActive(false);
        _defenderPlacementScreen.gameObject.SetActive(false);
    }

    void OnDefenderPlacementContinueReady(bool status)
    {
        defenderPlacementContinueButton.gameObject.SetActive(status);
    }

    void OnDefenderRespawnBegan()
    {
        _defenderPlacementScreen.gameObject.SetActive(true);
        _defenderRespawnText.gameObject.SetActive(true);
        OnDefenderPlacementContinueReady(false);
    }

    void OnDefenderRespawnEnded()
    {
        _defenderRespawnText.gameObject.SetActive(false);
        _defenderPlacementScreen.gameObject.SetActive(false);
    }

    void OnDefenderRespawnMenuReset(int pieceNum)
    {
        switch (pieceNum)
        {
            case (((int)ChessPieceEnum.KNIGHT)):
                defenderPlacementKnightButton.interactable = false;
                defenderPlacementKnightObject.SetActive(true);
                break;
            case (((int)ChessPieceEnum.BISHOP)):
                defenderPlacementBishopButton.interactable = false;
                defenderPlacementBishopObject.SetActive(true);
                break;
            case (((int)ChessPieceEnum.ROOK)):
                defenderPlacementRookButton.interactable = false;
                defenderPlacementRookObject.SetActive(true);
                break;
        }
    }
    #endregion
}
