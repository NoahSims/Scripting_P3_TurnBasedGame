using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMaxTree
{
    int depthLimit;

   public MiniMaxTree(int newDepthLimit)
    {
        depthLimit = newDepthLimit;
    }

    public void DetermineMove()
    {
        float maxScore = -999f;
        List<PieceMove> bestMoveList = new List<PieceMove>();

        // this bool disables the AI's ability to attack for the first turn, but only for the first turn
        bool isFirstRound = !GameBoardController.Current.PiecesAllowedToAttack;

        // get list of best moves from children nodes
        foreach (ChessPiece piece in GameBoardController.Current._blackTeam)
        {
            if(piece.inPlay)
            {
                if (isFirstRound)
                    GameBoardController.Current.PiecesAllowedToAttack = false;

                List<Vector2> moves = piece.GetPossibleMoves();

                if (isFirstRound)
                    GameBoardController.Current.PiecesAllowedToAttack = true;

                foreach (Vector2 move in moves)
                {
                    MiniMaxNode child = new MiniMaxNode(true, 1, depthLimit, -999f, 999f, piece, move);
                    float childScore = child.CalculateScore();

                    //Debug.Log(piece.name + ": " + childScore);

                    if (childScore > maxScore)
                    {
                        bestMoveList.Clear();
                        bestMoveList.Add(new PieceMove(piece, move));
                        maxScore = childScore;
                    }
                    else if (childScore == maxScore || bestMoveList.Count == 0)
                    {
                        bestMoveList.Add(new PieceMove(piece, move));
                        maxScore = childScore;
                    }

                    child = null;
                }
            }
        }

        /*
        foreach (PieceMove move in bestMoveList)
        {
            move.piece.SetTileIndicator(true);
            GameBoardController.Current.GameBoard.GridArray[((int)move.move.x), ((int)move.move.y)].TileIndicator.SetActive(true);
        }
        */

        Debug.Log("MiniMax Max Score = " + maxScore);

        // select random move from best moves
        if (bestMoveList.Count > 0)
        {
            int randNum = Random.Range(0, bestMoveList.Count);
            //Debug.Log("random value = " + randNum + "; count = " + bestMoveList.Count);
            bestMoveList[randNum].piece.SetTileIndicatorRed(true);
            bestMoveList[randNum].piece.MoveChessPiece(((int)bestMoveList[randNum].move.x), ((int)bestMoveList[randNum].move.y));
            bestMoveList[randNum].piece.SetTileIndicatorRed(true);
        }
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

public class MiniMaxNode
{
    bool isMaximizer;
    int depth;
    int depthLimit;
    float score;
    float alpha, beta;
    ChessPiece piece;
    Vector2 pieceOrigPos;
    Vector2 moveTo;
    ChessPiece attackTarget;

    public MiniMaxNode(bool newIsMaximizer, int newDepth, int newDepthLimit, float newAlpha, float newBeta, ChessPiece newPiece, Vector2 newMove)
    {
        isMaximizer = newIsMaximizer;
        depth = newDepth;
        depthLimit = newDepthLimit;
        score = 0;
        alpha = newAlpha;
        beta = newBeta;
        piece = newPiece;
        moveTo = newMove;
    }

    public float CalculateScore()
    {
        //Debug.Log("Depth = " + this.depth);

        // try move
        score += TestMove();

        // check next nodes based on current move
        // don't check children if reached depth limit or piece has captured the king (ending the game)
        if(depth < depthLimit && score < 1000f)
        {
            // if this node is the maximizer, its child node will be the minimizer, so get the min score from child
            if (isMaximizer)
            {
                score += GetMinScoreFromChildren();
                alpha = score;
            }
            else
            {
                score += GetMaxScoreFromChildren();
                beta = score;
            }
        }

        // undo move
        UndoTestMove();

        return this.score;
    }

    private float TestMove()
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
                return 1000f;
            else
                return 1f * (1f - (0.1f * depth));
        }
        else     // maximizing player loses score if minimizing player captures a piece
        {
            if (attackTarget == null)
                return 0;
            else
                return -1f * (1f - (0.1f * depth));
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

    private float GetMinScoreFromChildren()
    {
        float minScore = 999f;

        foreach (ChessPiece oponent in GameBoardController.Current._defenders)
        {
            if (oponent.inPlay)
            {
                //Debug.Log("Considering piece: " + oponent.gameObject.name);
                List<Vector2> moves = oponent.GetPossibleMoves();
                foreach (Vector2 move in moves)
                {
                    MiniMaxNode currentChild = new MiniMaxNode(!this.isMaximizer, this.depth + 1, this.depthLimit, -999f, 999f, oponent, move);
                    currentChild.CalculateScore();

                    //Debug.Log("Child score = " + currentChild.score);

                    if (currentChild.beta < alpha)
                    {
                        Debug.Log("AlphaBeta pruned a min value of " + currentChild.beta);
                        return currentChild.beta;
                    }


                    if (currentChild.score < minScore)
                    {
                        //Debug.Log("Replacing minScore " + minScore + " with " + currentChild.score);
                        minScore = currentChild.score;
                    }
                }
            }
        }

        //Debug.Log("Depth: " + this.depth + "; Returning: " + minScore);

        return minScore;
    }

    private float GetMaxScoreFromChildren()
    {
        float maxScore = -999f;

        foreach (ChessPiece oponent in GameBoardController.Current._blackTeam)
        {
            if (oponent.inPlay)
            {
                //Debug.Log("Considering piece: " + oponent.gameObject.name);
                List<Vector2> moves = oponent.GetPossibleMoves();
                foreach (Vector2 move in moves)
                {
                    MiniMaxNode currentChild = new MiniMaxNode(!this.isMaximizer, this.depth + 1, this.depthLimit, -999f, 999f, oponent, move);
                    currentChild.CalculateScore();

                    //Debug.Log("Child score = " + currentChild.score);

                    if (currentChild.alpha > beta)
                    {
                        Debug.Log("AlphaBeta pruned a max value of " + currentChild.alpha);
                        return currentChild.alpha;
                    }

                    if (currentChild.score > maxScore)
                    {
                        //Debug.Log("Replacing maxScore " + maxScore + " with " + currentChild.score);
                        maxScore = currentChild.score;
                    }
                }
            }
        }

        //Debug.Log("Depth: " + this.depth + "; Returning: " + maxScore);

        return maxScore;
    }
}
