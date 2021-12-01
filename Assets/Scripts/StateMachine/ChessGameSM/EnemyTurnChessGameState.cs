using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class EnemyTurnChessGameState : ChessGameState
{
    public static event Action EnemyTurnBegan;
    public static event Action EnemyTurnEnded;

    [SerializeField] float _pauseDuration = 1.5f;
    [SerializeField] Canvas _enemySpawnCounterCanvas = null;
    [SerializeField] Text _enemySpawnCounterText = null;
    [SerializeField] AudioClip _enemyCounterSound = null;
    private int _enemySpawnCounterXPos = 0;
    private int _enemySpawnCounterZPos = 0;

    private bool hasClicked = false;

    public override void Enter()
    {
        Debug.Log("Enemy turn: ...Enter");
        EnemyTurnBegan?.Invoke();

        // hook into events
        InputController.Current.PressedMouse += OnMousePressed;

        Debug.Log("Round Number = " + StateMachine.RoundNumber);

        hasClicked = false;

        StartCoroutine(EnemyThinkingRoutine(_pauseDuration));
    }

    private void OnMousePressed(int button)
    {
        hasClicked = true;
    }

    IEnumerator EnemyThinkingRoutine(float pauseDuration)
    {
        Debug.Log("Enemy thinking...");

        yield return new WaitForSeconds(2f); // neccesary so that Minimax doesn't freeze everything before ui can update

        MiniMaxTree tree = new MiniMaxTree(5);
        tree.DetermineMove();

        //yield return new WaitUntil(() => hasClicked);
        yield return new WaitForSeconds(pauseDuration);

        // go to spawn enemy
        StartCoroutine(EnemySpawn());
    }

    IEnumerator EnemySpawn()
    {
        int counter = 3 - (StateMachine.RoundNumber % 3);
        if (StateMachine.MaxRounds - StateMachine.RoundNumber > 3)
            _enemySpawnCounterCanvas.gameObject.SetActive(true);
        _enemySpawnCounterText.text = counter.ToString();

        if (StateMachine.RoundNumber % 3 == 0)
        {
            if(StateMachine.RoundNumber != 0)
            {
                // if tile is occupied, capture ocupying piece
                if (GameBoardController.Current.CheckTileContents(_enemySpawnCounterXPos, _enemySpawnCounterZPos) != 0)
                    GameBoardController.Current.GameBoard.GridArray[_enemySpawnCounterXPos, _enemySpawnCounterZPos].TilePiece.PieceCaptured();
                else // else spawn new black piece
                    GameBoardController.Current.SpawnBlackPieceAt(_enemySpawnCounterXPos, _enemySpawnCounterZPos);

                yield return new WaitForSeconds(_pauseDuration);
            }

            // move counter
            if (StateMachine.MaxRounds - StateMachine.RoundNumber > 3)
            {
                _enemySpawnCounterXPos = Mathf.FloorToInt(UnityEngine.Random.Range(0, 7.999f));
                _enemySpawnCounterZPos = Mathf.FloorToInt(UnityEngine.Random.Range(5, 7.999f));

                _enemySpawnCounterCanvas.gameObject.transform.position = GameBoardController.Current.GetChessWorldSpaceFromTile(_enemySpawnCounterXPos, _enemySpawnCounterZPos) - new Vector3(0, 0.17f, 0);

                if (_enemyCounterSound != null)
                {
                    AudioHelper.PlayClip2D(_enemyCounterSound, 1f);
                }
            }
            else
                _enemySpawnCounterCanvas.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.5f);
        }

        // end turn
        StateMachine.ChangeState<EndRoundChessGameState>();
        yield break;
    }

    public override void Exit()
    {
        //GameBoardController.Current.DisableAllIndicators();

        // unhook from events
        InputController.Current.PressedMouse -= OnMousePressed;

        EnemyTurnEnded?.Invoke();
        Debug.Log("Enemy Turn: Exit...");
    }
}
