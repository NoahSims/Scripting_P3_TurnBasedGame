using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMaxTree
{
    int depthLimit;

   public MinMaxTree(int newDepthLimit)
    {
        depthLimit = newDepthLimit;
    }

    public void DetermineMove()
    {
        int maxScore = 0;
        List<PieceMove> bestMoveList = new List<PieceMove>();

        foreach (ChessPiece piece in GameBoardController.Current._blackTeam)
        {
            if(piece.inPlay)
            {
                List<Vector2> moves = piece.GetPossibleMoves();
                foreach (Vector2 move in moves)
                {
                    MinMaxNode child = new MinMaxNode(true, 1, depthLimit, piece, move);
                    int childScore = child.CalculateScore();

                    Debug.Log(piece.name + ": " + childScore);

                    if (childScore > maxScore)
                    {
                        bestMoveList.Clear();
                        bestMoveList.Add(new PieceMove(piece, move));
                        maxScore = childScore;
                    }
                    else if (childScore == maxScore)
                    {
                        bestMoveList.Add(new PieceMove(piece, move));
                        maxScore = childScore;
                    }

                    child = null;
                }
            }
        }

        foreach (PieceMove move in bestMoveList)
        {
            move.piece.SetTileIndicator(true);
            GameBoardController.Current.GameBoard.GridArray[((int)move.move.x), ((int)move.move.y)].TileIndicator.SetActive(true);
        }
        Debug.Log("Max Score = " + maxScore);
        //MinMaxNode debug = new MinMaxNode(true, 1, 3, bestMoveList[0].piece, bestMoveList[0].move);
        //debug.Debugging();
    }
}

public struct PieceMove
{
    public ChessPiece piece;
    public Vector2 move;

    public PieceMove(ChessPiece newPiece, Vector2 newMove)
    {
        piece = newPiece;
        move = newMove;
    }
}

public class MinMaxNode
{
    bool isMaximizer;
    int depth;
    int depthLimit;
    int score;
    ChessPiece piece;
    Vector2 pieceOrigPos;
    Vector2 moveTo;
    ChessPiece attackTarget;
    MinMaxNode childNode = null;

    public MinMaxNode(bool newIsMaximizer, int newDepth, int newDepthLimit, ChessPiece newPiece, Vector2 newMove)
    {
        isMaximizer = newIsMaximizer;
        depth = newDepth;
        depthLimit = newDepthLimit;
        score = 0;
        piece = newPiece;
        moveTo = newMove;
    }

    public int CalculateScore()
    {
        //Debug.Log("Depth = " + this.depth);
        // try move
        score += TestMove();

        // check next nodes based on current move
        if(depth < depthLimit)
        {
            // this is counter-intuitive, but it's right. I did these in a weird order, just don't worry about it
            if (isMaximizer)
            {
                score += GetMinScoreFromChildren();
            }
            else
            {
                score += GetMaxScoreFromChildren();
            }
        }

        // undo move
        UndoTestMove();

        return this.score;
    }

    private int TestMove()
    {
        // get captured piece
        attackTarget = GameBoardController.Current.GameBoard.GridArray[((int)moveTo.x), ((int)moveTo.y)].TilePiece;
        if (attackTarget != null)
            attackTarget.inPlay = false;
        // save original position
        pieceOrigPos = new Vector2(piece.xPos, piece.zPos);

        // move to new position
        GameBoardController.Current.GameBoard.GridArray[((int)moveTo.x), ((int)moveTo.y)].TilePiece = piece;
        piece.xPos = ((int)moveTo.x);
        piece.zPos = ((int)moveTo.y);

        // get score based on captured piece
        if (isMaximizer)    // maximizing player gets score if capturing a piece
        {
            if (attackTarget == null)
                return 0;
            else if (attackTarget.ChessPieceType == ChessPieceEnum.KING)
                return 10;
            else
                return 1;
        }
        else     // maximizing player loses score if minimizing player captures a piece
        {
            if (attackTarget == null)
                return 0;
            else
                return -1;
        }
    }

    private void UndoTestMove()
    {
        // move piece to original position
        GameBoardController.Current.GameBoard.GridArray[((int)pieceOrigPos.x), ((int)pieceOrigPos.y)].TilePiece = piece;
        piece.xPos = ((int)pieceOrigPos.x);
        piece.zPos = ((int)pieceOrigPos.y);

        // restore original tile
        GameBoardController.Current.GameBoard.GridArray[((int)moveTo.x), ((int)moveTo.y)].TilePiece = attackTarget;
        if (attackTarget != null)
            attackTarget.inPlay = true;
    }

    private int GetMinScoreFromChildren()
    {
        int minScore = 0;

        foreach (ChessPiece oponent in GameBoardController.Current._defenders)
        {
            if (oponent.inPlay)
            {
                //Debug.Log("Considering piece: " + oponent.gameObject.name);
                List<Vector2> moves = oponent.GetPossibleMoves();
                foreach (Vector2 move in moves)
                {
                    MinMaxNode currentChild = new MinMaxNode(!this.isMaximizer, this.depth + 1, this.depthLimit, oponent, move);
                    currentChild.CalculateScore();

                    //Debug.Log("Child score = " + currentChild.score);

                    if (currentChild.score < minScore)
                    {
                        Debug.Log("Replacing minScore " + minScore + " with " + currentChild.score);
                        minScore = currentChild.score;
                        childNode = currentChild;
                    }
                    else
                        currentChild = null;
                }
            }
        }

        Debug.Log("Depth: " + this.depth + "; Returning: " + minScore);

        return minScore;
    }

    private int GetMaxScoreFromChildren()
    {
        int maxScore = 0;

        foreach (ChessPiece oponent in GameBoardController.Current._blackTeam)
        {
            if (oponent.inPlay)
            {
                //Debug.Log("Considering piece: " + oponent.gameObject.name);
                List<Vector2> moves = oponent.GetPossibleMoves();
                foreach (Vector2 move in moves)
                {
                    MinMaxNode currentChild = new MinMaxNode(!this.isMaximizer, this.depth + 1, this.depthLimit, oponent, move);
                    currentChild.CalculateScore();

                    //Debug.Log("Child score = " + currentChild.score);

                    if (currentChild.score > maxScore)
                    {
                        Debug.Log("Replacing maxScore " + maxScore + " with " + currentChild.score);
                        maxScore = currentChild.score;
                        childNode = currentChild;
                    }
                    else
                        currentChild = null;
                }
            }
        }

        Debug.Log("Depth: " + this.depth + "; Returning: " + maxScore);

        return maxScore;
    }

    public void Debugging()
    {
        CalculateScore();
        Debug.Log(piece.gameObject.name + " move to " + moveTo + "; score = " + score);
        if (attackTarget != null)
            Debug.Log("Captured " + attackTarget.gameObject.name);

        Debug.Log("Best child move - " + childNode?.piece + "; " + childNode?.moveTo);

        if(depth < depthLimit)
        {
            childNode?.Debugging();
        }
            
    }
}
