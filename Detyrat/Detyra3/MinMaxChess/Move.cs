using System.IO.Pipelines;

namespace ChessMinimax
{
    public class Move
    {
        public Piece Piece { get; set; }
        public int TargetX { get; set; }
        public int TargetY { get; set; }
        public int OriginalX { get; set; }
        public int OriginalY { get; set; }
        public Piece Captured { get; set; }

        public override string ToString()
        {
            string captured = Captured != null ? $" (captures {Captured.Type})" : "";
            return $"{Piece.Type} from ({OriginalX},{OriginalY}) to ({TargetX},{TargetY}){captured}";
        }
    }
}
