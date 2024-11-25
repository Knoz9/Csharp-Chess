namespace GameLogic
{
    public abstract class Piece
    {

        /// <summary>
        /// Gets the color of the piece (either "White" or "Black").
        /// </summary>
        public abstract String Color { get; }

        /// <summary>
        /// Gets the type of the piece (e.g., Pawn, Rook, Knight, etc.).
        /// </summary>
        public abstract PieceType Type { get; }

        /// <summary>
        /// Determines if the piece can legally move from the source position to the destination position, 
        /// based on the rules of the piece's movement and the state of the board.
        /// </summary>
        /// <param name="fromRow">The row of the piece's current position.</param>
        /// <param name="fromCol">The column of the piece's current position.</param>
        /// <param name="toRow">The row of the destination square.</param>
        /// <param name="toCol">The column of the destination square.</param>
        /// <param name="board">The current state of the game board.</param>
        /// <returns>True if the move is valid, false otherwise.</returns>
        public abstract bool CanMove(int fromRow, int fromCol, int toRow, int toCol, Board board);
    }
}
