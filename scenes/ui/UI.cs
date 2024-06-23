using Godot;

namespace Breakout.scenes;

public partial class UI : CanvasLayer
{
    public override void _Ready()
    {
        _gameOverLabel.Hide();
    }


    public void UpdateScore(int score)
    {
        _score.Text = score.ToString();
    }

    public void ShowGameOver()
    {
        _gameOverLabel.Show();
    }

    public void HideGameOver()
    {
        _gameOverLabel.Hide();
    }

    #region Child

    [ExportGroup("ChildDontChange")] [Export]
    private Label _score;

    [Export] private Label _gameOverLabel;

    #endregion
}