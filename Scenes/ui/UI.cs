using Godot;

namespace Breakout.scenes;

public partial class UI : CanvasLayer
{
    #region Child

    [ExportGroup("ChildDontChange")] [Export]
    private Label _score;

    #endregion

    public void UpdateScore(int score)
    {
        _score.Text = score.ToString();
    }
}