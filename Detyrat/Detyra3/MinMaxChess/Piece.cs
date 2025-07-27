namespace ChessMinimax
{
    public enum PieceType { 
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King
    }
    public enum Player {
        Max,
        Min 
    }

    public class Piece
    {
        public PieceType Type { get; set; }
        public Player Owner { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
