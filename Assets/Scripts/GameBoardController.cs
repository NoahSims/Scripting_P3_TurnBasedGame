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
    [SerializeField] private GameObject _tileIndicatorRedPrefab;
    private Vector3 _pieceOffset;

    [Header("Game Pieces")]
    [SerializeField] public List<ChessPiece> _defenders;
    [SerializeField] public List<ChessPiece> _blackTeam;
    [SerializeField] public List<ChessPiece> _whitePawns;
    [SerializeField] public GameObject _whiteKing = null;
    [SerializeField] private GameObject _whitePawnPrefab = null;
    [SerializeField] private GameObject[] _blackPiecePrefabs;
    public bool PiecesAllowedToAttack = true;

    [SerializeField] private bool _debuging = true;

    public TileGrid GameBoard = null;

    //---------------------------------------------------------------------------------------------------------------
    #region Board Instantiation
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

                GameBoard.GridArray[col, row].TileIndicatorRed = Instantiate(_tileIndicatorRedPrefab, _tileIndicatorHolder.transform);
                GameBoard.GridArray[col, row].TileIndicatorRed.transform.position += (new Vector3(col, 0, row) * GameBoard.CellSize) + (new Vector3(GameBoard.CellSize, 0, GameBoard.CellSize) * 0.5f);
                GameBoard.GridArray[col, row].TileIndicatorRed.SetActive(false);
            }
        }
    }
    #endregion
    //---------------------------------------------------------------------------------------------------------------
    #region Debugging
    private void FixedUpdate()
    {
        if (_debuging)
        {
            DebugLines();
        }
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
    #endregion
    //---------------------------------------------------------------------------------------------------------------
    #region Indicators
    public void DisableAllIndicators()
    {
        for (int col = 0; col < GameBoard.Width; col++)
        {
            for (int row = 0; row < GameBoard.Height; row++)
            {
                GameBoard.GridArray[col, row].TileIndicator.SetActive(false);
                GameBoard.GridArray[col, row].TileIndicatorRed.SetActive(false);
            }
        }
    }

    public void SetIndicators(int minRow, int maxRow, int minCol, int maxCol)
    {
        for(int col = minCol; col < maxCol; col++)
        {
            for(int row = minRow; row < maxRow; row++)
            {
                if (GameBoard.GridArray[col, row].TilePiece == null)
                {
                    GameBoard.GridArray[col, row].TileIndicator.SetActive(true);
                }
            }
        }
    }

    private void DisableAllPieces()
    {
        _whiteKing.SetActive(false);
    }
    #endregion
    //---------------------------------------------------------------------------------------------------------------
    #region Piece Spawning
    public void SpawnWhiteKing()
    {
        _whiteKing.SetActive(true);
        _whiteKing.GetComponent<ChessPiece>().SetChessPiecePosition(4, 0, true);
    }

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
            if (GameBoard.GridArray[x, z].TilePiece == null)
                posFound = true;
        }
        
        // Spawn piece and place
        GameObject newPawn = Instantiate(_whitePawnPrefab, Vector3.zero, Quaternion.identity);
        newPawn.GetComponent<ChessPiece>().SetChessPiecePosition(x, z, true);
        _whitePawns.Add(newPawn.GetComponent<ChessPiece>());
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
            if (GameBoard.GridArray[x, z].TilePiece == null)
                posFound = true;
        }
        
        GameObject newPiece = Instantiate(_blackPiecePrefabs[Mathf.FloorToInt(Random.Range(0, 2.999f))], Vector3.zero, Quaternion.identity);
        newPiece.GetComponent<ChessPiece>().SetChessPiecePosition(x, z, true);
        _blackTeam.Add(newPiece.GetComponent<ChessPiece>());
    }

    public void SpawnBlackPieceAt(int xPos, int zPos)
    {
        GameObject newPiece = Instantiate(_blackPiecePrefabs[Mathf.FloorToInt(Random.Range(0, 2.999f))], Vector3.zero, Quaternion.identity);
        newPiece.GetComponent<ChessPiece>().SetChessPiecePosition(xPos, zPos, true);
        _blackTeam.Add(newPiece.GetComponent<ChessPiece>());
    }
    #endregion
    //---------------------------------------------------------------------------------------------------------------
    #region Board Manipulation & Helper Functions
    public Vector2 GetTileFromWorldSpace(Vector3 worldPos)
    {
        worldPos -= _boardOrigin.transform.position;
        worldPos = worldPos / GameBoard.CellSize;

        return new Vector2(Mathf.Floor(worldPos.x), Mathf.Floor(worldPos.z));
    }

    public Vector3 GetChessWorldSpaceFromTile(int x, int z)
    {
        return (new Vector3(x, 0, z) * GameBoard.CellSize) + _boardOrigin.transform.position + _pieceOffset;
    }

    public int CheckTileContents(int x, int z)
    {
        if (x >= 0 && x < GameBoard.Width && z >= 0 && z < GameBoard.Height)
        {
            if (GameBoard.GridArray[x, z].TilePiece == null)
                return 0;
            else
                return ((int)GameBoard.GridArray[x, z].TilePiece.ChessPieceTeam);
        }

        return -1;
    }

    public ChessPiece GetPieceFromTile(int x, int z)
    {
        if (x >= 0 && x < GameBoard.Width && z >= 0 && z < GameBoard.Height)
        {
            return GameBoard.GridArray[x, z].TilePiece;
        }
        else
            return null;
    }

    public bool AttemptPlacePiece(int x, int z, ChessPiece piece)
    {
        if (x >= 0 && x < GameBoard.Width && z >= 0 && z < GameBoard.Height)
        {
            if (GameBoard.GridArray[x, z].TilePiece == null && GameBoard.GridArray[x, z].TileIndicator.activeInHierarchy)
            {
                piece.SetChessPiecePosition(x, z, true);
                return true;
            }
        } 
        
        return false;
    }
    #endregion
    //---------------------------------------------------------------------------------------------------------------
}
