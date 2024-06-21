using Breakout.scenes.brick;
using Godot;

namespace Breakout.scenes.test;

public partial class Test : Node2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }


    #region Child

    [ExportGroup("ChildDontChange")] [Export]
    private Brick _brick;

    #endregion
}