using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{

    /// <summary>
    /// The Rook class represents the rook piece in the chess game. It inherits from the `Piece` class 
    /// and implements the logic for the rook's movement. The rook moves in straight lines, either horizontally 
    /// or vertically, and can move any number of squares along these lines, as long as there are no obstacles in its path.
    /// </summary>
    internal class Rook : Piece
    {
        public override String Color { get; }
        public override PieceType Type => PieceType.Rook;
        public Rook(String color)
        {
            Color = color;
        }

        /// <summary>
        /// Determines if the rook can legally move from the source position to the destination position. 
        /// The rook can move any number of squares horizontally or vertically, but it checks if there are any pieces in the way.
        /// </summary>
        /// <returns>True if the move is valid (horizontal or vertical with no obstacles), false otherwise.</returns>

        public override bool CanMove(int fromRow, int fromCol, int toRow, int toCol, Board board)
        {
            // Horizontal move
            if (fromRow == toRow)
            {
                int step = (toCol > fromCol) ? 1 : -1;
                for (int col = fromCol + step; col != toCol; col += step)
                {
                    // Ensure we stay within bounds
                    if (col < 0 || col >= 8)
                    {
                        return false;
                    }

                    if (board[fromRow, col] != null)
                    {
                        return false;
                    }
                }
                return true;
            }
            // Vertical move
            else if (fromCol == toCol)
            {
                int step = (toRow > fromRow) ? 1 : -1;
                for (int row = fromRow + step; row != toRow; row += step)
                {
                    // Ensure we stay within bounds
                    if (row < 0 || row >= 8)
                    {
                        return false; 
                    }

                    if (board[row, fromCol] != null)
                    {
                        return false;
                    }
                }
                return true;
            }

            return false;
        }

    }
}