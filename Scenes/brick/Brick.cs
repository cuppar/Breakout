using Godot;

namespace Breakout.scenes.brick;

public partial class Brick : StaticBody2D
{
    public Vector2 Size
    {
        get => ((RectangleShape2D)_collisionShape.Shape).Size;
        set => ((RectangleShape2D)_collisionShape.Shape).Size = value;
    }

    public override void _Ready()
    {
        GD.Print($"size: {Size}");
    }

    #region Child

    [ExportGroup("ChildDontChange")] [Export]
    private CollisionShape2D _collisionShape;

    #endregion
}