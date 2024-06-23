using Breakout.autoloads;
using Breakout.constants;
using Godot;

namespace Breakout.scenes;

public partial class TitlePage : Control
{
    public override void _Ready()
    {
        _startButton.Pressed += OnStartButtonPressed;
        _quitButton.Pressed += OnQuitButtonPressed;
    }

    private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }

    private static void OnStartButtonPressed()
    {
        AutoloadManager.SceneTranslation.Call(SceneTranslation.MethodName.ChangeSceneToFile, ScenePaths.Board);
    }


    #region Child

    [ExportGroup("ChildDontChange")] [Export]
    private Button _startButton;

    [Export] private Button _quitButton;

    #endregion
}