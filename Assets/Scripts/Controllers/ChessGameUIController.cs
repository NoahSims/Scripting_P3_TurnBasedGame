using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChessGameUIController : MonoBehaviour
{
    [SerializeField] Text _enemyThinkingTextUI = null;

    private void OnEnable()
    {
        EnemyTurnChessGameState.EnemyTurnBegan += OnEnemyTurnBegan;
        EnemyTurnChessGameState.EnemyTurnEnded += OnEnemyTurnEnded;
    }

    private void OnDisable()
    {
        EnemyTurnChessGameState.EnemyTurnBegan -= OnEnemyTurnBegan;
        EnemyTurnChessGameState.EnemyTurnEnded -= OnEnemyTurnEnded;
    }

    private void Start()
    {
        // make sure text is disabled on start
        _enemyThinkingTextUI.gameObject.SetActive(false);
    }

    void OnEnemyTurnBegan()
    {
        _enemyThinkingTextUI.gameObject.SetActive(true);
    }

    void OnEnemyTurnEnded()
    {
        _enemyThinkingTextUI.gameObject.SetActive(false);
    }
}
