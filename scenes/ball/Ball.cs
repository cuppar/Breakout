using System;
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

    private Vector2 _direction = Vector2.Down;

    private float _speed = 200;

    [Export] public double HalfDegChangeAfterCollision = 20;
    [Export] public int HalfSpeedChangeAfterCollision = 50;
    [Export] public float MinSpeed = 400;

    [Export]
    public float Speed
    {
        get => _speed;
        set
        {
            if (Math.Abs(_speed - value) < float.Epsilon)
                return;

            _speed = value;
            UpdateVelocity();
        }
    }

    [Export]
    public Vector2 Direction
    {
        get => _direction;
        set
        {
            if (_direction == value)
                return;

            _direction = value;
            UpdateVelocity();
        }
    }

    private void UpdateVelocity()
    {
        if (Speed < MinSpeed)
            _speed = MinSpeed;

        if (Direction == Vector2.Zero)
            _direction = Vector2.Up;

        Velocity = Direction.Normalized() * _speed;
    }


    public override void _Ready()
    {
        UpdateVelocity();
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


        // 碰撞后调整小球角度和速度
        newVelocity =
            newVelocity.Rotated(
                (float)Mathf.DegToRad(GD.RandRange(-HalfDegChangeAfterCollision, HalfDegChangeAfterCollision)));

        newVelocity += newVelocity.Normalized() *
                       GD.RandRange(-HalfSpeedChangeAfterCollision, HalfSpeedChangeAfterCollision);

        _direction = newVelocity.Normalized();
        _speed = newVelocity.Length();
        UpdateVelocity();


        // 如果是和砖块碰撞，把碰到的砖块销毁，并加分
        if (collider is not Brick brick) return;

        EmitSignal(SignalName.HitBrick);
        brick.QueueFree();
    }
}