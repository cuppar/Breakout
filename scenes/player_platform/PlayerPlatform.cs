using Godot;

namespace Breakout.scenes;

public partial class PlayerPlatform : CharacterBody2D
{
    private Vector2 _direction = Vector2.Zero;

    [Export] public float Speed { get; set; } = 200;


    public override void _Process(double delta)
    {
        _direction = Vector2.Zero;
        if (Input.IsActionPressed("move_left")) _direction += Vector2.Left;

        if (Input.IsActionPressed("move_right")) _direction += Vector2.Right;

        Velocity = _direction * Speed;
        MoveAndSlide();
    }
}