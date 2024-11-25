using System.Diagnostics;

namespace GameLogic
{
    /// <summary>
    /// The Game class manages the logic and rules of the chess game. It handles the state of the chessboard, 
    /// the players' turns, and validates moves. It includes functionality for resetting the game, moving pieces, 
    /// checking for check or checkmate conditions, and managing special pawn promotions. The game alternates turns 
    /// between two players, ensuring that moves are valid and that the king is not in check after each move.
    /// </summary>

    public class Game
    {
        public Board Board { get; private set; }
        public string CurrentTurn { get; private set; }
        public bool IsGameOver { get; private set; }

        /// <summary>
        /// Initializes a new game with an empty board, sets the current turn to "White", 
        /// and sets the game over flag to false. It also resets the game to its initial state.
        /// </summary>
        public Game()
        {
            Board = new Board();
            CurrentTurn = "White";
            IsGameOver = false;
            ResetGame();
        }

        /// <summary>
        /// Resets the game by setting up the board, changing the current turn to "White", 
        /// and marking the game as not over.
        /// </summary>
        public void ResetGame()
        {
            Board = Board.resetBoard();
            CurrentTurn = "White";
            IsGameOver = false;
        }

        /// <summary>
        /// Moves a piece from one square to another on the board, ensuring the move is valid, 
        /// does not result in the player's king being in check, and handles pawn promotion and checkmate detection.
        /// </summary>
        /// <param name="fromRow">Row of the piece to move.</param>
        /// <param name="fromCol">Column of the piece to move.</param>
        /// <param name="toRow">Row to move the piece to.</param>
        /// <param name="toCol">Column to move the piece to.</param>
        /// <returns>True if the move was valid and completed, false otherwise.</returns>
        public bool MovePiece(int fromRow, int fromCol, int toRow, int toCol)
        {
            if (IsGameOver) return false;

            Piece piece = Board[fromRow, fromCol];

            // Ensure piece belongs to the current player
            if (piece == null || piece.Color != CurrentTurn) return false;

            // Perform the move temporarily
            Piece tempPiece = Board[toRow, toCol]; // Save target position's piece
            Board[toRow, toCol] = piece;
            Board[fromRow, fromCol] = null;

            bool isKingInCheck = IsKingInCheck(CurrentTurn);

            Board[fromRow, fromCol] = piece;
            Board[toRow, toCol] = tempPiece;

            if (isKingInCheck)
            {
                Debug.WriteLine("Move rejected: King would be in check.");
                return false;
            }

            // Perform the actual move
            if (Board.MovePiece(fromRow, fromCol, toRow, toCol, Board))
            {
                // Check for pawn promotion
                if (piece.Type == PieceType.Pawn && (toRow == 0 || toRow == 7))
                {
                    PromotePawn(toRow, toCol, piece.Color);
                }

                // After a valid move, check for checkmate
                string opponentColor = CurrentTurn == "White" ? "Black" : "White";
                if (CheckMate(opponentColor))
                {
                    IsGameOver = true;
                    Debug.WriteLine($"Game Over: {opponentColor} is in checkmate.");
                    return true;
                }

                // Switch turn
                CurrentTurn = (CurrentTurn == "White") ? "Black" : "White";
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a list of valid moves for a piece at a given position on the board. 
        /// The function checks each possible move and ensures the king is not left in check after the move.
        /// </summary>
        /// <param name="row">Row of the piece.</param>
        /// <param name="col">Column of the piece.</param>
        /// <returns>A list of valid move positions for the piece.</returns>
        public List<Tuple<int, int>> GetValidMoves(int row, int col)
        {
            Debug.WriteLine($"GetValidMoves called with row: {row}, col: {col}");
            List<Tuple<int, int>> validMoves = new List<Tuple<int, int>>();

            if (row < 0 || row >= 8 || col < 0 || col >= 8)
            {
                Debug.WriteLine($"Invalid position: row = {row}, col = {col}");
                return validMoves;
            }

            Piece piece = Board[row, col];
            if (piece == null) return validMoves;

            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    if (piece.CanMove(row, col, r, c, Board))
                    {
                        Piece targetPiece = Board[r, c];
                        if (targetPiece != null && targetPiece.Color == piece.Color)
                        {
                            continue;
                        }

                        // Temporarily perform the move
                        Piece tempPiece = Board[r, c];
                        Board[r, c] = piece;
                        Board[row, col] = null;

                        // Check if the move leaves the king in check
                        if (!IsKingInCheck(piece.Color))
                        {
                            validMoves.Add(new Tuple<int, int>(r, c));
                        }

                        // Undo the move
                        Board[row, col] = piece;
                        Board[r, c] = tempPiece;
                    }
                }
            }

            return validMoves;
        }

        /// <summary>
        /// Checks if the king of a given color is in check. 
        /// It evaluates if any enemy piece can attack the king's position.
        /// </summary>
        /// <param name="kingColor">Color of the king to check for check.</param>
        /// <returns>True if the king is in check, false otherwise.</returns>
        public bool IsKingInCheck(string kingColor)
        {
            Tuple<int, int>? kingPosition = FindKing(kingColor);
            if (kingPosition == null) return false;

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Piece piece = Board[row, col];
                    if (piece != null && piece.Color != kingColor)
                    {
                        if (piece.CanMove(row, col, kingPosition.Item1, kingPosition.Item2, Board))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Finds the position of the king for a given color on the board.
        /// </summary>
        /// <param name="kingColor">Color of the king to find.</param>
        /// <returns>The position of the king as a tuple (row, col), or null if the king is not found.</returns>
        public Tuple<int, int>? FindKing(string kingColor)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Piece piece = Board[row, col];
                    if (piece != null && piece.Color == kingColor && piece.Type == PieceType.King)
                    {
                        return new Tuple<int, int>(row, col);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Checks if the given color's king is in checkmate, 
        /// which happens if the king is in check and there are no valid moves to escape check.
        /// </summary>
        /// <param name="kingColor">Color of the king to check for checkmate.</param>
        /// <returns>True if the king is in checkmate, false otherwise.</returns>
        private bool CheckMate(string kingColor)
        {
            // If the king is not in check, no checkmate
            if (!IsKingInCheck(kingColor)) return false;

            // Check if any valid move is possible for the player to get out of check
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Piece piece = Board[row, col];
                    if (piece != null && piece.Color == kingColor)
                    {
                        List<Tuple<int, int>> validMoves = GetValidMoves(row, col);

                        foreach (var move in validMoves)
                        {
                            // Temporarily perform the move to see if it resolves the check
                            Piece tempPiece = Board[move.Item1, move.Item2];
                            Board[move.Item1, move.Item2] = piece;
                            Board[row, col] = null;

                            // If moving this piece leaves the king safe, return false (not checkmate)
                            if (!IsKingInCheck(kingColor))
                            {
                                Board[row, col] = piece;
                                Board[move.Item1, move.Item2] = tempPiece;

                                return false;
                            }
                            Board[row, col] = piece;
                            Board[move.Item1, move.Item2] = tempPiece;
                        }
                    }
                }
            }

            // No valid moves that escape the check => Checkmate
            return true;
        }

        /// <summary>
        /// Promotes a pawn to a queen if it reaches the opposite side of the board.
        /// </summary>
        /// <param name="row">Row where the pawn is located.</param>
        /// <param name="col">Column where the pawn is located.</param>
        /// <param name="color">Color of the pawn being promoted.</param>
        private void PromotePawn(int row, int col, string color)
        {
            Debug.WriteLine($"Pawn at ({row}, {col}) promoted to Queen!");

            Board[row, col] = new Queen(color);
        }


    }
}
