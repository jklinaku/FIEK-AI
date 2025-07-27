using System;
using System.Linq;

namespace ChessMinimax
{
    public static class BoardPrinter
    {
        public static void Print(Board board)
        {
            Console.WriteLine("  +-----------------+");
            for (int y = 7; y >= 0; y--)
            {
                Console.Write($"{y} |");
                for (int x = 0; x < 8; x++)
                {
                    var piece = board.Pieces.FirstOrDefault(p => p.X == x && p.Y == y);
                    if (piece == null)
                        Console.Write(" .");
                    else
                        Console.Write(" " + GetPieceSymbol(piece));
                }
                Console.WriteLine(" |");
            }
            Console.WriteLine("  +-----------------+");
            Console.WriteLine("    0 1 2 3 4 5 6 7");
        }

        private static char GetPieceSymbol(Piece piece)
        {
            char symbol = piece.Type switch
            {
                PieceType.Pawn => 'P',
                PieceType.Knight => 'N',
                PieceType.Bishop => 'B',
                PieceType.Rook => 'R',
                PieceType.Queen => 'Q',
                PieceType.King => 'K',
                _ => '?'
            };
            return piece.Owner == Player.Max ? char.ToUpper(symbol) : char.ToLower(symbol);
        }
    }
}
