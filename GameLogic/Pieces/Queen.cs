using System;

namespace GameLogic
{

    /// <summary>
    /// The Queen class represents the queen piece in the chess game. It inherits from the `Piece` class 
    /// and implements the logic for the queen's movement. The queen is the most powerful piece, 
    /// capable of moving any number of squares horizontally, vertically, or diagonally, as long as its path is clear.
    /// </summary>
    internal class Queen : Piece
    {
        public override string Color { get; }
        public override PieceType Type => PieceType.Queen;

        public Queen(string color)
        {
            Color = color;
        }

        /// <summary>
        /// Determines if the queen can legally move from the source position to the destination position. 
        /// The queen can move horizontally, vertically, or diagonally, and it checks if the path is clear of pieces.
        /// </summary>
        /// <returns>True if the move is valid (horizontal, vertical, or diagonal and clear path), false otherwise.</returns>

        public override bool CanMove(int fromRow, int fromCol, int toRow, int toCol, Board board)
        {
            // Horizontal move
            if (fromRow == toRow) 
            {
                int minCol = Math.Min(fromCol, toCol);
                int maxCol = Math.Max(fromCol, toCol);
                for (int col = minCol + 1; col < maxCol; col++)
                {
                    if (board[fromRow, col] != null)
                    {
                        return false;
                    }
                }
                return true; 
            }

            // Vertical move
            if (fromCol == toCol) 
            {
                int minRow = Math.Min(fromRow, toRow);
                int maxRow = Math.Max(fromRow, toRow);
                for (int row = minRow + 1; row < maxRow; row++)
                {
                    if (board[row, fromCol] != null) 
                    {
                        return false;
                    }
                }
                return true;
            }

            // Diagonal move
            if (Math.Abs(fromRow - toRow) == Math.Abs(fromCol - toCol))
            {
                int rowDirection = toRow > fromRow ? 1 : -1;
                int colDirection = toCol > fromCol ? 1 : -1;

                int row = fromRow + rowDirection;
                int col = fromCol + colDirection;

                while (row != toRow && col != toCol)
                {
                    if (board[row, col] != null)
                    {
                        return false;
                    }
                    row += rowDirection;
                    col += colDirection;
                }

                return true;
            }

            return false;
        }
    }
}
