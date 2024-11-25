using System;

namespace GameLogic
{

    /// <summary>
    /// The King class represents the king piece in the chess game. It inherits from the `Piece` class 
    /// and implements the logic for the king's movement. The king can move exactly one square in any direction: 
    /// horizontally, vertically, or diagonally.
    /// </summary>
    internal class King : Piece
    {
        public override string Color { get; }
        public override PieceType Type => PieceType.King;

        public King(string color)
        {
            Color = color;
        }

        /// <summary>
        /// Determines if the king can legally move from the source position to the destination position. 
        /// The king can move one square in any direction: horizontally, vertically, or diagonally.
        /// </summary>
        /// <returns>True if the move is valid (one square in any direction), false otherwise.</returns>
        public override bool CanMove(int fromRow, int fromCol, int toRow, int toCol, Board board)
        {
            if (Math.Abs(fromRow - toRow) <= 1 && Math.Abs(fromCol - toCol) <= 1)
            {
                return true;
            }

            return false;
        }
    }
}


