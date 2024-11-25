using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{

    /// <summary>
    /// The Bishop class represents a bishop piece in the chess game. It inherits from the `Piece` class 
    /// and implements the logic for the bishop's movement. A bishop moves diagonally on the board and 
    /// is able to traverse any number of squares in its path as long as there are no obstacles. 
    /// The color of the bishop is specified during its creation.
    /// </summary>
    internal class Bishop : Piece
    {
        public override string Color { get; }
        public override PieceType Type => PieceType.Bishop;

        public Bishop(string color)
        {
            Color = color;
        }

        /// <summary>
        /// Determines if the bishop can legally move from the source position to the destination position.
        /// The bishop moves diagonally, and the move is valid if the row and column differences are equal.
        /// It also checks if there are any pieces blocking its path along the diagonal.
        /// </summary>
        /// <returns>True if the move is valid (diagonal and unobstructed), false otherwise.</returns>
        public override bool CanMove(int fromRow, int fromCol, int toRow, int toCol, Board board)
        {
            // A Bishop moves diagonally: abs(row difference) == abs(col difference)
            int rowDiff = Math.Abs(fromRow - toRow);
            int colDiff = Math.Abs(fromCol - toCol);

            if (rowDiff == colDiff)
            {
                int rowDirection = toRow > fromRow ? 1 : -1;
                int colDirection = toCol > fromCol ? 1 : -1;

                int currentRow = fromRow + rowDirection;
                int currentCol = fromCol + colDirection;

                while (currentRow != toRow && currentCol != toCol)
                {
                    if (currentRow < 0 || currentRow >= 8 || currentCol < 0 || currentCol >= 8)
                    {
                        return false;
                    }

                    Piece pieceAtCurrentSquare = board[currentRow, currentCol];
                    if (pieceAtCurrentSquare != null)
                    {
                        return false;
                    }

                    currentRow += rowDirection;
                    currentCol += colDirection;
                }

                return true;
            }

            return false;
        }
    }

}
