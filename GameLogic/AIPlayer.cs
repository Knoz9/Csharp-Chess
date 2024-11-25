using System;
using System.Collections.Generic;

namespace GameLogic
{

    /// <summary>
    /// Represents an AI player that can make moves in the game based on its color and the current state of the game.
    /// This is a very basic AI that only cares about capture moves.
    /// This AI also evaluates each capture by score / piece value, so it will always capture the queen if it can.
    /// </summary>
    public class AIPlayer
    {
        private readonly string aiColor;
        private readonly Random random = new Random();

        public AIPlayer(string color)
        {
            aiColor = color;
        }

        /// <summary>
        /// Makes a move for the AI player. It checks if it is the AI's turn, then calculates valid moves.
        /// Prioritizes capture moves, and selects the best move based on the game state.
        /// Since it cannot calculate losing pieces, it is extremely agressive.
        /// </summary>
        /// <param name="game">The current game instance.</param>
        public void MakeMove(Game game)
        {
            if (game.CurrentTurn != aiColor)
                return;

            var allMoves = GetAllValidMoves(game);
            if (allMoves.Count == 0)
                return;

            var captureMoves = GetCaptureMoves(game, allMoves);
            var selectedMove = captureMoves.Count > 0 ? SelectBestMove(captureMoves, game) : SelectBestMove(allMoves, game);

            game.MovePiece(selectedMove.fromRow, selectedMove.fromCol, selectedMove.toRow, selectedMove.toCol);
        }

        /// <summary>
        /// Retrieves all valid moves for the AI player’s pieces on the board. It iterates through all the squares
        /// and checks for AI-controlled pieces, then collects all the possible valid moves for each piece.
        /// </summary>
        /// <param name="game">The current game instance.</param>
        /// <returns>A list of valid moves in the format (fromRow, fromCol, toRow, toCol).</returns>
        private List<(int fromRow, int fromCol, int toRow, int toCol)> GetAllValidMoves(Game game)
        {
            var validMoves = new List<(int fromRow, int fromCol, int toRow, int toCol)>();

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var piece = game.Board[row, col];
                    if (piece != null && piece.Color == aiColor)
                    {
                        var pieceMoves = game.GetValidMoves(row, col);
                        foreach (var move in pieceMoves)
                        {
                            validMoves.Add((row, col, move.Item1, move.Item2));
                        }
                    }
                }
            }

            // Shuffle the list of moves randomly
            ShuffleMoves(validMoves);

            return validMoves;
        }

        /// <summary>
        /// Shuffles a list of moves randomly. This introduces variability in the AI's move selection (where scores are the same)
        /// to avoid repetitive behavior.
        /// </summary>
        /// <param name="moves">The list of moves to be shuffled.</param>
        private void ShuffleMoves(List<(int fromRow, int fromCol, int toRow, int toCol)> moves)
        {
            int n = moves.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                var value = moves[k];
                moves[k] = moves[n];
                moves[n] = value;
            }
        }

        /// <summary>
        /// Filters and returns a list of moves that capture an opponent's piece. It checks each move to see if the target
        /// square contains an opponent's piece, and if so, includes it as a capture move.
        /// </summary>
        /// <param name="game">The current game instance.</param>
        /// <param name="moves">A list of moves to be evaluated for capturing.</param>
        /// <returns>A list of capture moves in the format (fromRow, fromCol, toRow, toCol).</returns>
        private List<(int fromRow, int fromCol, int toRow, int toCol)> GetCaptureMoves(Game game, List<(int fromRow, int fromCol, int toRow, int toCol)> moves)
        {
            var captureMoves = new List<(int fromRow, int fromCol, int toRow, int toCol)>();

            foreach (var move in moves)
            {
                var targetPiece = game.Board[move.toRow, move.toCol];
                if (targetPiece != null && targetPiece.Color != aiColor)
                {
                    captureMoves.Add(move);
                }
            }

            return captureMoves;
        }


        /// <summary>
        /// Selects the best move from a list of possible moves based on a scoring system. The method evaluates each move
        /// using the `GetMoveScore` method and selects the one with the highest score.
        /// </summary>
        /// <param name="moves">The list of moves to be evaluated.</param>
        /// <param name="game">The current game instance.</param>
        /// <returns>The best move based on the score, in the format (fromRow, fromCol, toRow, toCol).</returns>
        private (int fromRow, int fromCol, int toRow, int toCol) SelectBestMove(List<(int fromRow, int fromCol, int toRow, int toCol)> moves, Game game)
        {
            var bestMove = moves[0];
            int bestScore = int.MinValue;

            foreach (var move in moves)
            {
                int moveScore = GetMoveScore(move, game);
                if (moveScore > bestScore)
                {
                    bestScore = moveScore;
                    bestMove = move;
                }
            }

            return bestMove;
        }

        // <summary>
        /// Calculates the score for a move by evaluating the target piece. If the move captures an opponent's piece,
        /// it assigns a score based on the piece's value.
        /// </summary>
        /// <param name="move">The move to be scored, in the format (fromRow, fromCol, toRow, toCol).</param>
        /// <param name="game">The current game instance.</param>
        /// <returns>The score of the move, where a higher score is better.</returns>
        private int GetMoveScore((int fromRow, int fromCol, int toRow, int toCol) move, Game game)
        {
            var targetPiece = game.Board[move.toRow, move.toCol];
            int score = 0;

            if (targetPiece != null && targetPiece.Color != aiColor)
            {
                score += GetPieceValue(targetPiece.Type);
            }

            return score;
        }


        /// <summary>
        /// Returns the point value of a piece based on its type.
        /// </summary>
        /// <param name="type">The type of the piece.</param>
        /// <returns>The point value of the piece.</returns>
        private int GetPieceValue(PieceType type)
        {
            switch (type)
            {
                case PieceType.Pawn: return 1;
                case PieceType.Knight: return 3;
                case PieceType.Bishop: return 3;
                case PieceType.Rook: return 5;
                case PieceType.Queen: return 9;
                case PieceType.King: return 0; // King cannot be "captured" only checked, so thats why its 0.
                default: return 0;
            }
        }
    }
}
