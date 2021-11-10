using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardController : MonoBehaviour
{
    public static GameBoardController Current { get; private set; }

    [Header("Board")]
    [SerializeField] private GameObject _boardOrigin = null;
    [SerializeField] private float _pieceYOffest = 0;
    [SerializeField] private float _tileSize = 1.5f;
    [SerializeField] private GameObject _tileIndicatorHolder;
    [SerializeField] private GameObject _tileIndicatorPrefab;
    private Vector3 _pieceOffset;

    [Header("Game Pieces")]
    [SerializeField] private GameObject _whiteKing = null;
    [SerializeField] private GameObject _whitePawn = null;
    [SerializeField] private GameObject[] _whitePawns;
    [SerializeField] private GameObject[] _blackPiecePrefabs;

    [SerializeField] private bool _debuging = true;

    public TileGrid GameBoard = null;

    private void Awake()
    {
        Current = this;
        
    }

    private void Start()
    {
        _tileIndicatorHolder.transform.position = _boardOrigin.transform.position;
        InstantiateBoard();
        _pieceOffset = new Vector3(GameBoard.CellSize * 0.5f, _pieceYOffest, GameBoard.CellSize * 0.5f);

        DisableAllPieces();
    }

    private void FixedUpdate()
    {
        if(_debuging)
        {
            DebugLines();
        }
    }

    public void InstantiateBoard()
    {
        GameBoard = new TileGrid(8, 8, _tileSize);
        
        for (int col = 0; col < GameBoard.Width; col++)
        {
            for (int row = 0; row < GameBoard.Height; row++)
            {
                GameBoard.GridArray[col, row].TileIndicator = Instantiate(_tileIndicatorPrefab, _tileIndicatorHolder.transform);
                GameBoard.GridArray[col, row].TileIndicator.transform.position += (new Vector3(col, 0, row) * GameBoard.CellSize) + (new Vector3(GameBoard.CellSize, 0, GameBoard.CellSize) * 0.5f);
                GameBoard.GridArray[col, row].TileIndicator.SetActive(false);
            }
        }
    }

    public void DisableAllIndicators()
    {
        for (int col = 0; col < GameBoard.Width; col++)
        {
            for (int row = 0; row < GameBoard.Height; row++)
            {
                GameBoard.GridArray[col, row].TileIndicator.SetActive(false);
            }
        }
    }

    public void SetIndicators(int minRow, int maxRow, int minCol, int maxCol)
    {
        for(int col = minCol; col < maxCol; col++)
        {
            for(int row = minRow; row < maxRow; row++)
            {
                if (GameBoard.GridArray[col, row].TileContents == ((int)ChessPieceEnum.EMPTY))
                {
                    GameBoard.GridArray[col, row].TileIndicator.SetActive(true);
                }
            }
        }
    }

    private void DisableAllPieces()
    {
        _whiteKing.SetActive(false);
        for(int i = 0; i < _whitePawns.Length; i++)
        {
            _whitePawns[i].SetActive(false);
        }
    }

    public void SpawnWhiteKing()
    {
        GameBoard.GridArray[4, 0].TileContents = ((int)ChessPieceEnum.W_KING);
        _whiteKing.SetActive(true);
    }

    /*
    public void SpawnWhitePawns()
    {
        for(int i = 0; i < _whitePawns.Length; i++)
        {
            bool posFound = false;
            int x = 0;
            int z = 0;

            while(!posFound)
            {
                x = Mathf.FloorToInt(Random.Range(0, 7.999f));
                z = Mathf.FloorToInt(Random.Range(0, 3.999f));

                if((x > 1) && (x < 6))
                {
                    x = Mathf.FloorToInt(Random.Range(0, 7.999f));
                }

                if (GameBoard.GridArray[x, z] == ((int)ChessPieceEnum.EMPTY))
                    posFound = true;
            }

            GameBoard.GridArray[x, z] = ((int)ChessPieceEnum.W_PAWN);
            _whitePawns[i].SetActive(true);
            _whitePawns[i].transform.position = _boardOrigin.transform.position + new Vector3(x * GameBoard.CellSize, 0, z * GameBoard.CellSize) + _pieceOffset;
        }
    }
    */

    public void SpawnWhitePawn()
    {
        bool posFound = false;
        int x = 0;
        int z = 0;

        while (!posFound)
        {
            // get random position
            x = Mathf.FloorToInt(Random.Range(0, 7.999f));
            z = Mathf.FloorToInt(Random.Range(0, 3.999f));

            // if not on the edge of the board, reroll. makes edge pieces more likely, but not guaranteed
            if ((x > 1) && (x < 6))
            {
                x = Mathf.FloorToInt(Random.Range(0, 7.999f));
            }

            // make sure position is not occupied
            if (GameBoard.GridArray[x, z].TileContents == ((int)ChessPieceEnum.EMPTY))
                posFound = true;
        }

        GameBoard.GridArray[x, z].TileContents = ((int)ChessPieceEnum.W_PAWN);
        
        // TODO: need to keep track of pieces that are created
        Vector3 pos = _boardOrigin.transform.position + new Vector3(x * GameBoard.CellSize, 0, z * GameBoard.CellSize) + _pieceOffset;
        GameObject newPawn = Instantiate(_whitePawn, pos, Quaternion.identity);
    }

    public void SpawnBlackPiece()
    {
        bool posFound = false;
        int x = 0;
        int z = 0;

        while (!posFound)
        {
            // get random position
            x = Mathf.FloorToInt(Random.Range(0, 7.999f));
            z = Mathf.FloorToInt(Random.Range(5, 7.999f));

            // make sure position is not occupied
            if (GameBoard.GridArray[x, z].TileContents == ((int)ChessPieceEnum.EMPTY))
                posFound = true;
        }

        GameBoard.GridArray[x, z].TileContents = ((int)ChessPieceEnum.B_ENEMY);

        // TODO: need to keep track of pieces that are created
        Vector3 pos = _boardOrigin.transform.position + new Vector3(x * GameBoard.CellSize, 0, z * GameBoard.CellSize) + _pieceOffset;
        GameObject newPawn = Instantiate(_blackPiecePrefabs[Mathf.FloorToInt(Random.Range(0, 2.999f))], pos, Quaternion.identity);
    }

    private void DebugLines()
    {
        for (int col = 0; col < GameBoard.Width; col++)
        {
            for (int row = 0; row < GameBoard.Height; row++)
            {
                Vector3 tilePos = _boardOrigin.transform.position + new Vector3(col * GameBoard.CellSize, 0.01f, row * GameBoard.CellSize);
                //Gizmos.DrawLine(tilePos, tilePos + new Vector3(GameBoard.CellSize, 0, 0));
                Debug.DrawLine(tilePos, tilePos + new Vector3(GameBoard.CellSize, 0, 0), Color.red);
                Debug.DrawLine(tilePos, tilePos + new Vector3(0, 0, GameBoard.CellSize), Color.red);
            }
        }
    }
}

// TODO: rework piece numbering system
public enum ChessPieceEnum { EMPTY = 0, W_KING, W_PAWN, B_ENEMY};
