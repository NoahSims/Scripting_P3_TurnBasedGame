using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChessGameUIController : MonoBehaviour
{
    public static Action ContinueButtonPressed;
    public static Action UndoButtonPressed;
    public static Action ReturnToMenuButtonPressed;
    public static Action DefenderPlacementKnightButtonPressed;
    public static Action DefenderPlacementBishopButtonPressed;
    public static Action DefenderPlacementRookButtonPressed;

    [Header("Turn Screens")]
    [SerializeField] Text _enemyThinkingText = null;
    [SerializeField] Text _playerTurnCounter = null;
    [SerializeField] Button _playerTurnContinueButton = null;
    [SerializeField] Button _playerTurnUndoButton = null;

    [Header("Other Game States")]
    [SerializeField] Text _boardGeneratingText = null;
    [SerializeField] Canvas _winScreen = null;
    [SerializeField] Text _winLoseText = null;

    [Header("Defender Placement State")]
    [SerializeField] Canvas _defenderPlacementScreen = null;
    [SerializeField] Text _defenderPlacementText = null;
    [SerializeField] Button _defenderPlacementKnightButton = null;
    [SerializeField] GameObject _defenderPlacementKnightObject = null;
    [SerializeField] Button _defenderPlacementBishopButton = null;
    [SerializeField] GameObject _defenderPlacementBishopObject = null;
    [SerializeField] Button _defenderPlacementRookButton = null;
    [SerializeField] GameObject _defenderPlacementRookObject = null;
    [SerializeField] Button _defenderPlacementContinueButton = null;

    [Header("Defender Respawn State")]
    [SerializeField] Text _defenderRespawnText = null;
    [SerializeField] Button _defenderRespawnUndoButton = null;

    private void OnEnable()
    {
        PlayerTurnChessGameState.PlayerTurnBegan += OnPlayerTurnBegan;
        PlayerTurnChessGameState.PlayerTurnEnded += OnPlayerTurnEnded;
        PlayerTurnChessGameState.PlayerTurnContinueReady += OnPlayerTurnContinueReady;
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
        DefenderRespawnChessGameState.DefenderRespawnContinueReady += OnDefenderRespawnContinueReady;

        WinChessGameState.PlayerWon += OnPlayerWon;
        LoseChessGameState.PlayerLost += OnPlayerLost;
    }

    private void OnDisable()
    {
        PlayerTurnChessGameState.PlayerTurnBegan -= OnPlayerTurnBegan;
        PlayerTurnChessGameState.PlayerTurnEnded -= OnPlayerTurnEnded;
        PlayerTurnChessGameState.PlayerTurnContinueReady -= OnPlayerTurnContinueReady;
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
        _defenderPlacementContinueButton.gameObject.SetActive(false);
        _defenderRespawnUndoButton.gameObject.SetActive(false);
        _winScreen.gameObject.SetActive(false);
        _playerTurnContinueButton.gameObject.SetActive(false);
        _playerTurnUndoButton.gameObject.SetActive(false);
    }

    #region Buttons
    public void ContinueButton()
    {
        ContinueButtonPressed?.Invoke();
    }

    public void UndoButton()
    {
        UndoButtonPressed?.Invoke();
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
        _defenderPlacementKnightButton.interactable = false;
        _defenderPlacementKnightObject.SetActive(false);
        DefenderPlacementKnightButtonPressed?.Invoke();
    }

    public void DefenderPlacementBishopButton()
    {
        _defenderPlacementBishopButton.interactable = false;
        _defenderPlacementBishopObject.SetActive(false);
        DefenderPlacementBishopButtonPressed?.Invoke();
    }

    public void DefenderPlacementRookButton()
    {
        _defenderPlacementRookButton.interactable = false;
        _defenderPlacementRookObject.SetActive(false);
        DefenderPlacementRookButtonPressed?.Invoke();
    }

    public void OnDefenderPlacementCancelHolding(int pieceNum)
    {
        switch(pieceNum)
        {
            case (((int)ChessPieceEnum.KNIGHT)):
                _defenderPlacementKnightButton.interactable = true;
                _defenderPlacementKnightObject.SetActive(true);
                break;
            case (((int)ChessPieceEnum.BISHOP)):
                _defenderPlacementBishopButton.interactable = true;
                _defenderPlacementBishopObject.SetActive(true);
                break;
            case (((int)ChessPieceEnum.ROOK)):
                _defenderPlacementRookButton.interactable = true;
                _defenderPlacementRookObject.SetActive(true);
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
        OnPlayerTurnContinueReady(false);
    }

    void OnPlayerTurnContinueReady(bool status)
    {
        _playerTurnContinueButton.gameObject.SetActive(status);
        _playerTurnUndoButton.gameObject.SetActive(status);
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
        _defenderPlacementContinueButton.gameObject.SetActive(status);
    }

    void OnDefenderRespawnBegan()
    {
        _defenderPlacementScreen.gameObject.SetActive(true);
        _defenderRespawnText.gameObject.SetActive(true);
        OnDefenderRespawnContinueReady(false);
    }

    void OnDefenderRespawnContinueReady(bool status)
    {
        _defenderPlacementContinueButton.gameObject.SetActive(status);
        _defenderRespawnUndoButton.gameObject.SetActive(status);
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
                _defenderPlacementKnightButton.interactable = false;
                _defenderPlacementKnightObject.SetActive(!_defenderPlacementKnightObject.activeSelf);
                break;
            case (((int)ChessPieceEnum.BISHOP)):
                _defenderPlacementBishopButton.interactable = false;
                _defenderPlacementBishopObject.SetActive(!_defenderPlacementBishopObject.activeSelf);
                break;
            case (((int)ChessPieceEnum.ROOK)):
                _defenderPlacementRookButton.interactable = false;
                _defenderPlacementRookObject.SetActive(!_defenderPlacementRookObject.activeSelf);
                break;
        }
    }
    #endregion
}
