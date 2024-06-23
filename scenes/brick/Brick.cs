using Godot;

namespace Breakout.scenes;

public partial class Brick : StaticBody2D
{
    #region Child

    [ExportGroup("ChildDontChange")] [Export]
    private CollisionShape2D _collisionShape;

    #endregion


    public Vector2 Size
    {
        get => ((RectangleShape2D)_collisionShape.Shape).Size;
        set => ((RectangleShape2D)_collisionShape.Shape).Size = value;
    }
}