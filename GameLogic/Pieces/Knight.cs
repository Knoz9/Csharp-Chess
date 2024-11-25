using System;

namespace GameLogic
{

    /// <summary>
    /// The Knight class represents the knight piece in the chess game. It inherits from the `Piece` class 
    /// and implements the logic for the knight's movement. The knight moves in an "L" shape: 
    /// two squares in one direction and then one square perpendicular to that, or vice versa.
    /// </summary>

    internal class Knight : Piece
    {
        public override string Color { get; }
        public override PieceType Type => PieceType.Knight;

        public Knight(string color)
        {
            Color = color;
        }

        /// <summary>
        /// Determines if the knight can legally move from the source position to the destination position. 
        /// The knight moves in an "L" shape: two squares in one direction and then one square perpendicular to that, or vice versa.
        /// </summary>
        /// <returns>True if the move is valid (an "L" shape), false otherwise.</returns>
        public override bool CanMove(int fromRow, int fromCol, int toRow, int toCol, Board board)
        {
            int rowDiff = Math.Abs(fromRow - toRow);
            int colDiff = Math.Abs(fromCol - toCol);

            // Check for the L-shaped move
            if ((rowDiff == 2 && colDiff == 1) || (rowDiff == 1 && colDiff == 2))
            {
                return true; 
            }

            return false;
        }
    }
}
