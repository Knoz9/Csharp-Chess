using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

/// <summary>
/// The SoundManager class is responsible for loading and playing sound effects during the game.
/// It manages sounds for various events such as piece movement, capture, and game over.
/// The class initializes the sound files during construction and provides methods to play 
/// the respective sounds for move, capture, and game over events.
/// </summary>

public class SoundManager
{
    private SoundPlayer moveSound;
    private SoundPlayer captureSound;
    private SoundPlayer gameOverSound;

    public SoundManager()
    {
        moveSound = LoadSound("pack://application:,,,/Assets/Sounds/move.wav");
        captureSound = LoadSound("pack://application:,,,/Assets/Sounds/check.wav");
        gameOverSound = LoadSound("pack://application:,,,/Assets/Sounds/gameover.wav");
    }

    public void PlayMoveSound() => moveSound?.Play();
    public void PlayCaptureSound() => captureSound?.Play();
    public void PlayGameOverSound() => gameOverSound?.Play();

    private SoundPlayer LoadSound(string path)
    {
        return new SoundPlayer(Application.GetResourceStream(new Uri(path)).Stream);
    }
}
