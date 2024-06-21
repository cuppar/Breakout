using Breakout.scenes.brick;
using Godot;

namespace Breakout.scenes.board;

public partial class Board : Node2D
{
    public Vector2 Size
    {
        get
        {
            var x = _right.GlobalPosition.X - _left.GlobalPosition.X;
            var y = _bottom.GlobalPosition.Y - _top.GlobalPosition.Y;

            var xWall = ((RectangleShape2D)_left.Shape).Size.X / 2 + ((RectangleShape2D)_right.Shape).Size.X / 2;
            var yWall = ((RectangleShape2D)_top.Shape).Size.Y / 2 + ((RectangleShape2D)_bottom.Shape).Size.Y / 2;

            return new Vector2(x - xWall, y - yWall);
        }
    }

    public Vector2 Center
    {
        get
        {
            var leftWallRightEdge = _left.GlobalPosition.X + ((RectangleShape2D)_left.Shape).Size.X / 2;
            var rightWallLeftEdge = _right.GlobalPosition.X - ((RectangleShape2D)_right.Shape).Size.X / 2;
            var topWallBottomEdge = _top.GlobalPosition.Y + ((RectangleShape2D)_top.Shape).Size.Y / 2;
            var bottomWallTopEdge = _bottom.GlobalPosition.Y - ((RectangleShape2D)_bottom.Shape).Size.Y / 2;
            
            return new Vector2(leftWallRightEdge + rightWallLeftEdge, topWallBottomEdge + bottomWallTopEdge) / 2;
        }
    }

    public override void _Ready()
    {
        GD.Print($"size: {Size}");
        GD.Print($"center: {Center}");
    }


    #region Child

    [ExportGroup("ChildDontChange")] [Export]
    private CollisionShape2D _left;

    [Export] private CollisionShape2D _right;
    [Export] private CollisionShape2D _top;
    [Export] private CollisionShape2D _bottom;

    #endregion
}