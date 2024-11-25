using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

/// <summary>
/// The CursorManager class is responsible for managing and changing the cursor based on the player's turn.
/// It loads different cursors for the White and Black players and provides a method to retrieve the correct
/// cursor for the current turn.
/// </summary>


public class CursorManager
{
    private readonly Cursor whiteCursor;
    private readonly Cursor blackCursor;

    public CursorManager()
    {
        whiteCursor = LoadCursor("pack://application:,,,/Assets/Cursor/cursor1.cur");
        blackCursor = LoadCursor("pack://application:,,,/Assets/Cursor/cursor2b.cur");
    }

    public Cursor GetCursor(string turn) => turn == "White" ? whiteCursor : blackCursor;

    private Cursor LoadCursor(string path)
    {
        return new Cursor(Application.GetResourceStream(new Uri(path)).Stream);
    }
}
