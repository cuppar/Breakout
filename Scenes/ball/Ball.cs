using Godot;

namespace Breakout.scenes;

public partial class Ball : CharacterBody2D
{
    #region Delegates

    [Signal]
    public delegate void HitBottomWallEventHandler();

    [Signal]
    public delegate void HitBrickEventHandler();

    #endregion

    [Export] public float Speed { get; set; }

    [Export] public Vector2 InitDirection { get; set; }

    public override void _Ready()
    {
        Velocity = InitDirection.Normalized() * Speed;
    }

    public override void _PhysicsProcess(double delta)
    {
        var collision = MoveAndCollide(Velocity * (float)delta);
        if (collision == null) return;
        var collider = collision.GetCollider();

        // 如果和地面碰撞，游戏结束
        if (collider is StaticBody2D wall && wall.Name == "BottomWall")
        {
            EmitSignal(SignalName.HitBottomWall);
            QueueFree();
        }


        // 镜面反射
        var normal = collision.GetNormal();
        var velocityParallel = normal * normal.Dot(Velocity);
        var velocityPerpendicular = Velocity - velocityParallel;
        var newVelocity = velocityPerpendicular - velocityParallel;
        Velocity = newVelocity;

        // 如果是和砖块碰撞，把碰到的砖块销毁，并加分
        if (collider is not Brick brick) return;

        EmitSignal(SignalName.HitBrick);
        brick.QueueFree();
    }
}