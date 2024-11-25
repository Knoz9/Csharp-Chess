using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{

    /// <summary>
    /// The Board class represents the chessboard and handles the arrangement and management of pieces. 
    /// It provides methods for setting up the initial state of the chessboard, moving pieces, and accessing the board's positions.
    /// The board is represented as an 8x8 grid, where each square contains a `Piece` or is empty.
    /// </summary>
    public class Board
    {
        private readonly Piece[,] boardPieces = new Piece[8, 8];

        // Indexer to access board pieces by row and column
        public Piece this[int row, int column]
        {
            get { return boardPieces[row, column]; }
            set { boardPieces[row, column] = value; }
        }

        /// <summary>
        /// Initializes the chessboard by placing the pieces in their starting positions. 
        /// The pieces are arranged according to standard chess rules: black pieces are placed on the first two rows 
        /// and white pieces on the last two rows. Pawns are placed in front of the other pieces.
        /// </summary>
        private void setupPieces()  
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    this[row, col] = null;
                }
            }

            Piece[] blackPieces = new Piece[]
            {
                new Rook("Black"),
                new Knight("Black"),
                new Bishop("Black"),
                new Queen("Black"),
                new King("Black"),
                new Bishop("Black"),
                new Knight("Black"),
                new Rook("Black")
            };

            for (int col = 0; col < 8; col++)
            {
                this[0, col] = blackPieces[col];
                this[1, col] = new Pawn("Black");
            }

            Piece[] whitePieces = new Piece[]
            {
                new Rook("White"),
                new Knight("White"),
                new Bishop("White"),
                new Queen("White"),
                new King("White"),
                new Bishop("White"),
                new Knight("White"),
                new Rook("White")
            };

            for (int col = 0; col < 8; col++)
            {
                this[7, col] = whitePieces[col];
                this[6, col] = new Pawn("White");
            }
        }

        public Board resetBoard()
        {
            Board board = new Board();
            board.setupPieces();
            return board;
        }

        /// <summary>
        /// Resets the chessboard to its initial state with pieces in their starting positions.
        /// This method creates a new `Board` object, sets up the pieces, and returns the new board.
        /// </summary>
        /// <returns>A new `Board` object with the pieces set up in their initial positions.</returns>
        public bool MovePiece(int fromRow, int fromCol, int toRow, int toCol, Board board)
        {
            // Get the piece at the source position
            Piece pieceToMove = this[fromRow, fromCol];

            if (pieceToMove == null)
            {
                Debug.WriteLine("No piece at the source position.");
                return false; // No piece to move
            }

            Piece pieceAtDestination = this[toRow, toCol];

            if (pieceAtDestination != null && pieceAtDestination.Color == pieceToMove.Color)
            {
                Debug.WriteLine("Cannot move to a square occupied by a friendly piece.");
                return false;
            }

            // Use the CanMove method specific to each piece to check if the move is valid
            if (!pieceToMove.CanMove(fromRow, fromCol, toRow, toCol, board))
            {
                Debug.WriteLine($"Invalid move for {pieceToMove.Type}.");
                return false;
            }

            if (pieceToMove is Pawn pawn)
            {
                pawn.hasMoved = true;
            }

            // At this point, it's a valid move, either to an empty square or to capture an opponent's piece

            this[toRow, toCol] = pieceToMove;
            this[fromRow, fromCol] = null;

            Debug.WriteLine($"Moved {pieceToMove.Color} {pieceToMove.Type} from ({fromRow}, {fromCol}) to ({toRow}, {toCol})");

            return true;
        }


    }

}
