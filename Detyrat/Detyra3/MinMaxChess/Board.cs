using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessMinimax
{
    public class Board
    {
        public List<Piece> Pieces = new();

        public List<Move> GenerateMoves(Player player)
        {
            var moves = new List<Move>();
            foreach (var piece in Pieces)
            {
                if (piece.Owner == player)
                    moves.AddRange(GenerateMovesForPiece(piece));
            }
            return moves;
        }

        private List<Move> GenerateMovesForPiece(Piece piece)
        {
            var moves = new List<Move>();
            int direction = piece.Owner == Player.Max ? 1 : -1;

            // ---- PAWN ----
            if (piece.Type == PieceType.Pawn)
            {
                int targetY = piece.Y + direction;
                if (IsInsideBoard(piece.X, targetY))
                {
                    var forward = Pieces.FirstOrDefault(p => p.X == piece.X && p.Y == targetY);
                    if (forward == null)
                    {
                        moves.Add(new Move
                        {
                            Piece = piece,
                            OriginalX = piece.X,
                            OriginalY = piece.Y,
                            TargetX = piece.X,
                            TargetY = targetY
                        });
                    }

                    AddPawnCapture(piece, piece.X - 1, targetY, moves);
                    AddPawnCapture(piece, piece.X + 1, targetY, moves);
                }
            }

            // ---- KNIGHT ----
            if (piece.Type == PieceType.Knight)
            {
                int[][] deltas = {
                    new[] { 1, 2 }, new[] { 2, 1 }, new[] { -1, 2 }, new[] { -2, 1 },
                    new[] { -1, -2 }, new[] { -2, -1 }, new[] { 1, -2 }, new[] { 2, -1 }
                };
                foreach (var d in deltas)
                    TryAddMove(piece, piece.X + d[0], piece.Y + d[1], moves);
            }

            // ---- BISHOP ----
            if (piece.Type == PieceType.Bishop)
                AddSlidingMoves(piece, new[] { (1, 1), (1, -1), (-1, 1), (-1, -1) }, moves);

            // ---- ROOK ----
            if (piece.Type == PieceType.Rook)
                AddSlidingMoves(piece, new[] { (1, 0), (-1, 0), (0, 1), (0, -1) }, moves);

            // ---- QUEEN ----
            if (piece.Type == PieceType.Queen)
                AddSlidingMoves(piece, new[] {
                    (1, 0), (-1, 0), (0, 1), (0, -1),
                    (1, 1), (1, -1), (-1, 1), (-1, -1)
                }, moves);

            // ---- KING ----
            if (piece.Type == PieceType.King)
            {
                int[][] deltas = {
                    new[] { 1, 0 }, new[] { -1, 0 }, new[] { 0, 1 }, new[] { 0, -1 },
                    new[] { 1, 1 }, new[] { 1, -1 }, new[] { -1, 1 }, new[] { -1, -1 }
                };
                foreach (var d in deltas)
                    TryAddMove(piece, piece.X + d[0], piece.Y + d[1], moves);

                AddCastlingMoves(piece, moves);
            }

            return moves;
        }

        private void AddPawnCapture(Piece piece, int x, int y, List<Move> moves)
        {
            if (!IsInsideBoard(x, y)) return;
            var capture = Pieces.FirstOrDefault(p => p.X == x && p.Y == y && p.Owner != piece.Owner);
            if (capture != null)
            {
                moves.Add(new Move
                {
                    Piece = piece,
                    OriginalX = piece.X,
                    OriginalY = piece.Y,
                    TargetX = x,
                    TargetY = y,
                    Captured = capture
                });
            }
        }

        private void TryAddMove(Piece piece, int x, int y, List<Move> moves)
        {
            if (!IsInsideBoard(x, y)) return;

            var target = Pieces.FirstOrDefault(p => p.X == x && p.Y == y);
            if (target == null || target.Owner != piece.Owner)
            {
                moves.Add(new Move
                {
                    Piece = piece,
                    OriginalX = piece.X,
                    OriginalY = piece.Y,
                    TargetX = x,
                    TargetY = y,
                    Captured = target
                });
            }
        }

        private void AddSlidingMoves(Piece piece, (int dx, int dy)[] directions, List<Move> moves)
        {
            foreach (var (dx, dy) in directions)
            {
                int x = piece.X + dx;
                int y = piece.Y + dy;

                while (IsInsideBoard(x, y))
                {
                    var target = Pieces.FirstOrDefault(p => p.X == x && p.Y == y);
                    if (target == null)
                    {
                        moves.Add(new Move
                        {
                            Piece = piece,
                            OriginalX = piece.X,
                            OriginalY = piece.Y,
                            TargetX = x,
                            TargetY = y
                        });
                    }
                    else
                    {
                        if (target.Owner != piece.Owner)
                        {
                            moves.Add(new Move
                            {
                                Piece = piece,
                                OriginalX = piece.X,
                                OriginalY = piece.Y,
                                TargetX = x,
                                TargetY = y,
                                Captured = target
                            });
                        }
                        break;
                    }

                    x += dx;
                    y += dy;
                }
            }
        }

        private void AddCastlingMoves(Piece king, List<Move> moves)
        {
            if (king.Owner == Player.Max && king.X == 4 && king.Y == 0)
            {
                if (!Pieces.Any(p => p.X == 5 && p.Y == 0) &&
                    !Pieces.Any(p => p.X == 6 && p.Y == 0))
                {
                    moves.Add(new Move
                    {
                        Piece = king,
                        OriginalX = king.X,
                        OriginalY = king.Y,
                        TargetX = 6,
                        TargetY = 0
                    });
                }
                if (!Pieces.Any(p => p.X == 3 && p.Y == 0) &&
                    !Pieces.Any(p => p.X == 2 && p.Y == 0) &&
                    !Pieces.Any(p => p.X == 1 && p.Y == 0))
                {
                    moves.Add(new Move
                    {
                        Piece = king,
                        OriginalX = king.X,
                        OriginalY = king.Y,
                        TargetX = 2,
                        TargetY = 0
                    });
                }
            }
        }

        private bool IsInsideBoard(int x, int y) => x >= 0 && x < 8 && y >= 0 && y < 8;

        public void ApplyMove(Move move)
        {
            if (move.Captured != null)
                Pieces.Remove(move.Captured);

            move.Piece.X = move.TargetX;
            move.Piece.Y = move.TargetY;
        }

        public void UndoMove(Move move)
        {
            move.Piece.X = move.OriginalX;
            move.Piece.Y = move.OriginalY;

            if (move.Captured != null)
                Pieces.Add(move.Captured);
        }

        public int Evaluate(Player player)
        {
            int score = 0;
            foreach (var piece in Pieces)
            {
                int value = piece.Type switch
                {
                    PieceType.Pawn => 1,
                    PieceType.Knight => 3,
                    PieceType.Bishop => 3,
                    PieceType.Rook => 5,
                    PieceType.Queen => 9,
                    PieceType.King => 100,
                    _ => 0
                };
                score += piece.Owner == player ? value : -value;
            }

            score += GenerateMoves(player).Count - GenerateMoves(Opponent(player)).Count;
            return score;
        }

        private Player Opponent(Player p) => p == Player.Max ? Player.Min : Player.Max;
    }
}
