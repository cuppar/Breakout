using Godot;

namespace Breakout.Scenes.PlayerPlatform;

public partial class PlayerPlatform : CharacterBody2D
{
    private Vector2 _direction = Vector2.Zero;
    
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("move_left"))
        {
            GD.Print("move left");
        }
        if (@event.IsActionPressed("move_right"))
        {
            GD.Print("move right");
        }
    }
}