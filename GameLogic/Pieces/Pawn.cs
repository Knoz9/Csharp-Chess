using System;

namespace GameLogic
{

    /// <summary>
    /// The Pawn class represents the pawn piece in the chess game. It inherits from the `Piece` class 
    /// and implements the logic for the pawn's movement. Pawns move forward one square at a time, but 
    /// on their first move, they can move two squares. They can also capture diagonally. The pawn also has 
    /// promotion logic when it reaches the opponent's back rank.
    /// </summary>
    internal class Pawn : Piece
    {
        public override string Color { get; }
        public override PieceType Type => PieceType.Pawn;

        public bool hasMoved = false;

        public Pawn(string color)
        {
            Color = color;
        }

        /// <summary>
        /// Determines if the pawn can legally move from the source position to the destination position. 
        /// The pawn can move one square forward, two squares on its first move, or capture diagonally.
        /// </summary>
        /// <returns>True if the move is valid (forward or diagonal capture), false otherwise.</returns>

        public override bool CanMove(int fromRow, int fromCol, int toRow, int toCol, Board board)
        {
            int direction = this.Color == "White" ? -1 : 1; // White goes up (row decreases), Black goes down (row increases)
            if ((this.Color == "White" && toRow >= fromRow) || (this.Color == "Black" && toRow <= fromRow))
            {
                return false; // Invalid move: pawns can't move backwards
            }

            // Forward movement
            if (fromCol == toCol)
            {
                // First move: allow moving two squares forward
                if (!hasMoved && Math.Abs(fromRow - toRow) == 2)
                {
                    int middleRow = fromRow + direction;
                    if (board[middleRow, fromCol] == null && board[toRow, toCol] == null)
                    {
                        return true; 
                    }
                }

                // Regular one-square forward move
                if (Math.Abs(fromRow - toRow) == 1 && board[toRow, toCol] == null)
                {
                    return true; 
                }
            }

            // Diagonal capture: pawn can move diagonally if there is an opponent's piece
            if (Math.Abs(fromCol - toCol) == 1 && toRow == fromRow + direction)
            {
                Piece pieceAtDestination = board[toRow, toCol];
                if (pieceAtDestination != null && pieceAtDestination.Color != this.Color)
                {
                    return true; 
                }
            }

            return false; 
        }
    }
}
