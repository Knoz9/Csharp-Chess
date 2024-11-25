using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GameLogic;

/// <summary>
/// The HighlightManager class is responsible for managing the visual highlights on the chessboard.
/// It is used to highlight valid moves, selected squares, and reset highlights as necessary.
/// It works with the chessboard grid to update button backgrounds and borders to visually indicate:
/// 1. Valid moves for the selected piece (green for empty squares, red for enemy pieces).
/// 2. The currently selected square (highlighted with a red border).
/// 3. Removing all highlights when necessary.
/// </summary>

public class HighlightManager
{
    private readonly Grid chessboardGrid;

    public HighlightManager(Grid chessboardGrid)
    {
        this.chessboardGrid = chessboardGrid;
    }

    /// <summary>
    /// Highlights the valid moves for the selected piece by updating the background color of the respective squares.
    /// Green is used for valid empty squares, red for enemy pieces, and transparent for friendly pieces (invisible).
    /// It also resets the background of squares that are not valid moves.
    /// </summary>
    public void HighlightMoves(List<Tuple<int, int>> validMoves, Tuple<int, int> selectedPosition, Board board)
    {
        foreach (Button button in chessboardGrid.Children)
        {
            var position = button.Tag as Tuple<int, int>;
            if (position == null) continue;

            if (validMoves.Contains(position))
            {
                // Get the piece at the valid move position from the Board object
                Piece pieceAtPosition = board[position.Item1, position.Item2];
                Piece selectedPiece = board[selectedPosition.Item1, selectedPosition.Item2];

                // Highlight enemy pieces in red and valid empty moves in green
                if (pieceAtPosition != null && pieceAtPosition.Color != selectedPiece.Color)
                {
                    button.Background = Brushes.Red; // Enemy piece
                }
                else if (pieceAtPosition == null)
                {
                    button.Background = Brushes.LightGreen; // Valid empty square
                }
                else
                {
                    button.Background = Brushes.Transparent; // Friendly piece, no highlight
                }
            }
            else
            {
                ResetSquareHighlight(button, position);
            }
        }
    }

    /// <summary>
    /// Highlights the selected square with a red border to visually indicate the currently selected piece.
    /// All other squares are reset to their default border state.
    /// </summary>
    public void HighlightSelectedSquare(Button selectedButton)
    {
        foreach (Button button in chessboardGrid.Children)
        {
            button.BorderBrush = Brushes.Transparent;
            button.BorderThickness = new Thickness(0);
        }
        selectedButton.BorderBrush = Brushes.Red;
        selectedButton.BorderThickness = new Thickness(3);
    }

    /// <summary>
    /// Removes all highlights from the chessboard, resetting both the background color 
    /// and the border of each square to its default state.
    /// </summary>
    public void RemoveAllHighlights()
    {
        foreach (Button button in chessboardGrid.Children)
        {
            ResetSquareHighlight(button, button.Tag as Tuple<int, int>);
        }
    }

    /// <summary>
    /// Resets the highlight state for a specific square by setting its background, border color, 
    /// and border thickness to the default (transparent or no border).
    /// </summary>
    private void ResetSquareHighlight(Button button, Tuple<int, int> position)
    {
        button.Background = Brushes.Transparent;
        button.BorderBrush = Brushes.Transparent;
        button.BorderThickness = new Thickness(0);
    }
}
