using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

/// <summary>
/// The BlinkManager class is responsible for managing the blinking effect on a chessboard square, 
/// typically used for highlighting the King’s square when it is in check or checkmate.
/// It uses a DispatcherTimer to toggle the background color of the specified square at regular intervals, 
/// creating a blinking effect.
/// </summary>

public class BlinkManager
{
    private DispatcherTimer blinkTimer;
    private bool blinkState;
    private Grid chessboardGrid;

    public BlinkManager(Grid chessboardGrid)
    {
        this.chessboardGrid = chessboardGrid;
    }

    /// <summary>
    /// Starts the blinking effect on a specified square by toggling the background color at regular intervals.
    /// The square at the given position will alternate between the specified color and transparent.
    /// The blinking effect is controlled by a DispatcherTimer, and the effect is stopped if another blinking effect is already running.
    /// </summary>
    public void StartBlinking(Tuple<int, int> position, Brush color)
    {
        StopBlinking(); // Ensure no multiple timers are running

        blinkTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(500)
        };

        blinkTimer.Tick += (s, e) =>
        {
            ToggleBlink(position, color);
        };

        blinkTimer.Start();
    }

    /// <summary>
    /// Stops any ongoing blinking effect and resets the state, ensuring no timers are running.
    /// </summary>
    public void StopBlinking()
    {
        if (blinkTimer != null)
        {
            blinkTimer.Stop();
            blinkTimer = null;
        }
    }

    /// <summary>
    /// Toggles the background color of the specified square between the given color and transparent, 
    /// creating the blinking effect. The state alternates every time the timer ticks.
    /// </summary>
    private void ToggleBlink(Tuple<int, int> position, Brush color)
    {
        foreach (Button button in chessboardGrid.Children)
        {
            var pos = button.Tag as Tuple<int, int>;
            if (pos != null && pos.Equals(position))
            {
                button.Background = blinkState ? color : Brushes.Transparent;
                break;
            }
        }

        blinkState = !blinkState;
    }
}
