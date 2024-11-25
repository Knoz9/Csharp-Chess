using GameLogic;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using System;

namespace GameUI
{

    /// <summary>
    /// The MainWindow class is the core UI controller for the chess game application. 
    /// It handles the game logic interactions with the user interface, including piece selection, 
    /// movement, sound effects, cursor updates, and visual highlights for valid moves, 
    /// check, and checkmate. It initializes the game, manages player turns, and updates 
    /// the chessboard based on the current game state. The class also manages game-over states 
    /// and restarts the game when necessary.
    /// </summary>
    public partial class MainWindow : Window
    {
        private Tuple<int, int>? selectedPosition = null;
        private Game game;
        private BlinkManager blinkManager;
        private SoundManager soundManager;
        private CursorManager cursorManager;
        private HighlightManager highlightManager;
        private AIPlayer aiPlayerB;
        private bool aiPlayermode;


        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
            ChooseGameMode();  // Show the mode selection dialog on startup
        }

        /// <summary>
        /// Initializes the game by setting up game components, such as the game board, 
        /// highlight manager, blink manager, sound manager, and cursor manager, 
        /// and updates the cursor for the current player's turn.
        /// </summary>

        private void InitializeGame()
        {
            game = new Game();
            highlightManager = new HighlightManager(ChessboardGrid);
            blinkManager = new BlinkManager(ChessboardGrid);
            soundManager = new SoundManager();
            cursorManager = new CursorManager();
            aiPlayerB = new AIPlayer("Black");
            DrawChessboard();
            Cursor = cursorManager.GetCursor(game.CurrentTurn);
        }

        /// <summary>
        /// Handles a square click event, determining whether to select a piece or make a move 
        /// based on the current state (whether a piece is selected or not).
        /// </summary>

        private void OnSquareClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            var position = clickedButton?.Tag as Tuple<int, int>;

            if (position == null)
                return;

            int row = position.Item1;
            int col = position.Item2;

            if (selectedPosition == null)
            {
                HandlePieceSelection(clickedButton, row, col);
            }
            else
            {
                HandleMove(row, col);
            }
        }

        /// <summary>
        /// Handles the selection of a piece on the chessboard. Highlights the selected square 
        /// and shows valid moves for that piece. It also updates the selected position.
        /// </summary>
        private void HandlePieceSelection(Button clickedButton, int row, int col)
        {
            Piece selectedPiece = game.Board[row, col];

            if (selectedPiece != null && selectedPiece.Color == game.CurrentTurn)
            {
                highlightManager.RemoveAllHighlights();
                selectedPosition = new Tuple<int, int>(row, col);
                highlightManager.HighlightSelectedSquare(clickedButton);

                List<Tuple<int, int>> validMoves = game.GetValidMoves(row, col);
                highlightManager.HighlightMoves(validMoves, selectedPosition, game.Board);
                Debug.WriteLine($"Selected {selectedPiece.Color} {selectedPiece.Type} at ({row}, {col})");
            }
        }

        /// <summary>
        /// Handles the move action after a piece has been selected. Attempts to move the selected piece 
        /// to the destination square, and if valid, performs necessary actions for the move.
        /// </summary>

        private void HandleMove(int row, int col)
        {
            int fromRow = selectedPosition?.Item1 ?? -1;
            int fromCol = selectedPosition?.Item2 ?? -1;

            if (fromRow >= 0 && fromCol >= 0 && game.MovePiece(fromRow, fromCol, row, col))
            {
                PerformMoveActions();
            }
            else
            {
                Debug.WriteLine("Invalid move");
                highlightManager.RemoveAllHighlights();
                selectedPosition = null;
            }
        }

        /// <summary>
        /// Performs necessary actions after a successful move, such as playing a sound, updating the cursor, 
        /// clearing highlights, stop blinking, and checking if the king is in check or the game is over.
        /// </summary>

        private async Task PerformMoveActions()
        {
            selectedPosition = null;
            soundManager.PlayMoveSound();
            Cursor = cursorManager.GetCursor(game.CurrentTurn);
            highlightManager.RemoveAllHighlights();
            blinkManager.StopBlinking();
            DrawChessboard();

            if (game.CurrentTurn == "Black" && aiPlayermode == true)
            {
                await Task.Delay(1000); // Async delay allows the UI to update before continuing
                aiPlayerB.MakeMove(game);
                Cursor = cursorManager.GetCursor(game.CurrentTurn);
                DrawChessboard(); // Refresh the UI to reflect the AI's move
            }

            if (game.IsKingInCheck(game.CurrentTurn))
            {
                HandleKingInCheck();
            }

            if (game.IsGameOver)
            {
                HandleGameOver();
            }
        }
        /// <summary>
        /// Handles the scenario where a king is in check by playing a capture sound and highlighting the king 
        /// with a blinking yellow effect to indicate it is in danger.
        /// </summary>
        private void HandleKingInCheck()
        {
            soundManager.PlayCaptureSound();
            Tuple<int, int> kingPosition = game.FindKing(game.CurrentTurn);
            blinkManager.StartBlinking(kingPosition, Brushes.Yellow);
        }

        /// <summary>
        /// Handles the end of the game, playing a game-over sound, displaying a message indicating the winner, 
        /// and offering an option to reset the game.
        /// </summary>

        private void HandleGameOver()
        {
            soundManager.PlayGameOverSound();
            string winner = game.CurrentTurn == "White" ? "Black" : "White";
            Tuple<int, int> kingPosition = game.FindKing(winner);
            blinkManager.StartBlinking(kingPosition, Brushes.Red);

            MessageBoxResult result = MessageBox.Show($"{game.CurrentTurn} Wins! Game Over!", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);

            if (result == MessageBoxResult.OK)
            {
                ResetGame();
            }
        }

        /// <summary>
        /// Resets the game by stopping any blinking effects, resetting the game state, 
        /// redrawing the chessboard, and updating the cursor for the new game.
        /// </summary>
        private void ResetGame()
        {
            blinkManager.StopBlinking();
            game.ResetGame();
            DrawChessboard();
            Cursor = cursorManager.GetCursor(game.CurrentTurn);
        }

        /// <summary>
        /// Draws the chessboard by clearing the existing children, setting the background image, 
        /// and creating a button for each square on the board.
        /// </summary>

        private void DrawChessboard()
        {
            ChessboardGrid.Children.Clear();
            SetChessboardBackground();

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    CreateSquareButton(row, col);
                }
            }
        }

        /// <summary>
        /// Sets the background of the chessboard using an image brush with the chessboard's background image.
        /// </summary>

        private void SetChessboardBackground()
        {
            ImageBrush background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/GameUI;component/Assets/Images/board.png"))
            };
            ChessboardGrid.Background = background;
        }

        /// <summary>
        /// Creates a button for a given square on the chessboard, setting the button's background, 
        /// adding any piece images if a piece exists on that square, and binding a click event.
        /// </summary>
        private void CreateSquareButton(int row, int col)
        {
            Button btn = new Button
            {
                Margin = new Thickness(0),
                BorderThickness = new Thickness(0),
                Background = (row + col) % 2 == 0 ? Brushes.Transparent : Brushes.Transparent,
                Tag = new Tuple<int, int>(row, col)
            };

            Piece piece = game.Board[row, col];
            if (piece != null)
            {
                btn.Content = CreatePieceImage(piece);
            }

            btn.Click += OnSquareClick;
            Grid.SetRow(btn, row);
            Grid.SetColumn(btn, col);
            ChessboardGrid.Children.Add(btn);
        }

        /// <summary>
        /// Creates an image for a chess piece based on its color and type, and attempts to load the image from 
        /// a predefined resource path. If the image cannot be loaded, an error message is logged.
        /// </summary>
        private Image CreatePieceImage(Piece piece)
        {
            Image pieceImage = new Image();
            string pieceName = $"{piece.Color.ToLower()}-{piece.Type.ToString().ToLower()}";
            string imagePath = $"pack://application:,,,/GameUI;component/Assets/Images/{pieceName}.png";

            try
            {
                pieceImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading image for {pieceName}: {ex.Message}");
            }

            return pieceImage;
        }

        /// <summary>
        /// Displays a dialog that allows the user to choose between 1-player or 2-player mode.
        /// </summary>
        private void ChooseGameMode()
        {
            MessageBoxResult result = MessageBox.Show("Playing Alone?\nDo you want to play against an AI?", "Game Mode", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                aiPlayermode = true;  // 1 Player (AI opponent)
            }
            else
            {
                aiPlayermode = false;  // 2 Players (Human vs Human)
            }
        }
    }
}
