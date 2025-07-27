using System;

namespace ChessMinimax
{
    class Program
    {
        static void Main(string[] args)
        {
            var board = new Board();
            board.Pieces.AddRange(new[]
            {
                // White pieces (Player.Max)
                new Piece { Type = PieceType.Rook, Owner = Player.Max, X = 0, Y = 0 },
                new Piece { Type = PieceType.Knight, Owner = Player.Max, X = 2, Y = 2 },
                new Piece { Type = PieceType.Bishop, Owner = Player.Max, X = 2, Y = 0 },
                new Piece { Type = PieceType.Queen, Owner = Player.Max, X = 3, Y = 0 },
                new Piece { Type = PieceType.Rook, Owner = Player.Max, X = 4, Y = 0 },
                new Piece { Type = PieceType.Knight, Owner = Player.Max, X = 5, Y = 2 },
                new Piece { Type = PieceType.Bishop, Owner = Player.Max, X = 6, Y = 1 },
                new Piece { Type = PieceType.King, Owner = Player.Max, X = 6, Y = 0 },

                new Piece { Type = PieceType.Pawn, Owner = Player.Max, X = 0, Y = 1 },
                new Piece { Type = PieceType.Pawn, Owner = Player.Max, X = 1, Y = 1 },
                new Piece { Type = PieceType.Pawn, Owner = Player.Max, X = 2, Y = 3 },
                new Piece { Type = PieceType.Pawn, Owner = Player.Max, X = 3, Y = 3 },
                new Piece { Type = PieceType.Pawn, Owner = Player.Max, X = 4, Y = 3 },
                new Piece { Type = PieceType.Pawn, Owner = Player.Max, X = 5, Y = 1 },
                new Piece { Type = PieceType.Pawn, Owner = Player.Max, X = 6, Y = 2 },
                new Piece { Type = PieceType.Pawn, Owner = Player.Max, X = 7, Y = 1 },

                // Black pieces (Player.Min)
                new Piece { Type = PieceType.Rook, Owner = Player.Min, X = 0, Y = 7 },
                new Piece { Type = PieceType.Knight, Owner = Player.Min, X = 1, Y = 7 },
                new Piece { Type = PieceType.Bishop, Owner = Player.Min, X = 2, Y = 7 },
                new Piece { Type = PieceType.Queen, Owner = Player.Min, X = 3, Y = 7 },
                new Piece { Type = PieceType.Rook, Owner = Player.Min, X = 7, Y = 7 },
                new Piece { Type = PieceType.Knight, Owner = Player.Min, X = 6, Y = 5 },
                new Piece { Type = PieceType.Bishop, Owner = Player.Min, X = 5, Y = 7 },
                new Piece { Type = PieceType.King, Owner = Player.Min, X = 6, Y = 7 },

                new Piece { Type = PieceType.Pawn, Owner = Player.Min, X = 0, Y = 6 },
                new Piece { Type = PieceType.Pawn, Owner = Player.Min, X = 1, Y = 5 },
                new Piece { Type = PieceType.Pawn, Owner = Player.Min, X = 2, Y = 4 },
                new Piece { Type = PieceType.Pawn, Owner = Player.Min, X = 3, Y = 5 },
                new Piece { Type = PieceType.Pawn, Owner = Player.Min, X = 4, Y = 4 },
                new Piece { Type = PieceType.Pawn, Owner = Player.Min, X = 5, Y = 6 },
                new Piece { Type = PieceType.Pawn, Owner = Player.Min, X = 6, Y = 6 },
                new Piece { Type = PieceType.Pawn, Owner = Player.Min, X = 7, Y = 6 }
            });

            Console.WriteLine("Initial Board:");
            BoardPrinter.Print(board);
            Console.WriteLine($"Initial Evaluation: {board.Evaluate(Player.Max)}");

            Move bestMove = ChessEngine.GetBestMove(board, 3);
            if (bestMove != null)
            {
                Console.WriteLine($"\nBest Move: {bestMove}");
                board.ApplyMove(bestMove);
                Console.WriteLine("\nBoard After Best Move:");
                BoardPrinter.Print(board);
                Console.WriteLine($"Evaluation After Move: {board.Evaluate(Player.Max)}");
            }
            else
            {
                Console.WriteLine("No moves available.");
            }
        }
    }
}
