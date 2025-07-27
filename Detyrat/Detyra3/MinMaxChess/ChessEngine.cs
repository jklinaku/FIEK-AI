using System;
using System.Collections.Generic;

namespace ChessMinimax
{
    public static class ChessEngine
    {
        private const int NEG_INF = -10000;
        private const int POS_INF = 10000;

        public static Move GetBestMove(Board board, int depth)
        {
            Move bestMove = null;
            int bestEval = NEG_INF;

            var moves = board.GenerateMoves(Player.Max);

            foreach (var move in moves)
            {
                board.ApplyMove(move);
                int eval = Run(board, depth - 1, NEG_INF, POS_INF, false);
                board.UndoMove(move);

                if (eval > bestEval)
                {
                    bestEval = eval;
                    bestMove = move;
                }
            }

            return bestMove;
        }

        private static int Run(Board board, int depth, int alpha, int beta, bool isMaximizingPlayer)
        {
            Player current = isMaximizingPlayer ? Player.Max : Player.Min;

            if (depth == 0)
                return board.Evaluate(Player.Max);

            var moves = board.GenerateMoves(current);
            if (moves.Count == 0)
                return board.Evaluate(Player.Max);

            if (isMaximizingPlayer)
            {
                int maxEval = NEG_INF;
                foreach (var move in moves)
                {
                    if (IsBadMove(move)) continue;

                    board.ApplyMove(move);
                    int eval = Run(board, depth - 1, alpha, beta, false);
                    board.UndoMove(move);

                    maxEval = Math.Max(maxEval, eval);
                    alpha = Math.Max(alpha, eval);
                    if (beta <= alpha) break; 
                }
                return maxEval;
            }
            else
            {
                int minEval = POS_INF;
                foreach (var move in moves)
                {
                    if (IsBadMove(move)) continue;

                    board.ApplyMove(move);
                    int eval = Run(board, depth - 1, alpha, beta, true);
                    board.UndoMove(move);

                    minEval = Math.Min(minEval, eval);
                    beta = Math.Min(beta, eval);
                    if (beta <= alpha) break; 
                }
                return minEval;
            }
        }
        private static bool IsBadMove(Move move)
        {
            return move.Piece.Type == PieceType.Pawn && move.Captured?.Type == PieceType.Queen;
        }
    }
}
