using Breakout.autoloads;
using Breakout.constants;
using Godot;

namespace Breakout.scenes;

public partial class Board : Node2D
{
    [Export] private float _brickGapH = 5;
    [Export] private float _brickGapV = 5;

    private PackedScene _brickScene = GD.Load<PackedScene>(ScenePaths.Brick);

    [Export] private float _playerPlatformDistanceFromBottom = 50;
    [Export] private float _playerPlatformDistanceFromBricks = 200;

    [Export] public double BallInitMinSpeed = 200;
    [Export] public double BallInitMaxSpeed = 400;

    private int _score;

    private Vector2 Size
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

    private Vector2 Center
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
        InitSignals();
        NewGame();
    }

    private void NewGame()
    {
        InitPlayerPlatformPosition();
        InitBricks();
        InitScore();
        InitBall();
    }

    private void InitBall()
    {
        _ball.Speed = (float)GD.RandRange(BallInitMinSpeed, BallInitMaxSpeed);

        _ball.Direction = new Vector2(GD.RandRange(-1, 1), GD.RandRange(-1, 1)).Normalized();
        if (_ball.Direction == Vector2.Zero)
            _ball.Direction = Vector2.Up;

        _ball.GlobalPosition = new Vector2(_bottom.GlobalPosition.X,
            _playerPlatform.GlobalPosition.Y - _playerPlatformDistanceFromBricks / 2);
    }

    private void InitScore()
    {
        _score = 0;
        _ui.UpdateScore(_score);
    }

    private void InitSignals()
    {
        _ball.HitBrick += OnBallHitBrick;
        _ball.HitBottomWall += OnGameOver;
    }

    private async void OnGameOver()
    {
        _ui.ShowGameOver();
        await ToSignal(GetTree().CreateTimer(2), SceneTreeTimer.SignalName.Timeout);
        AutoloadManager.SceneTranslation.Call(SceneTranslation.MethodName.ChangeSceneToFile, ScenePaths.TitlePage);
    }

    private void OnBallHitBrick()
    {
        _ui.UpdateScore(++_score);
    }

    private void InitPlayerPlatformPosition()
    {
        _playerPlatform.GlobalPosition = _playerPlatform.GlobalPosition with
        {
            X = _bottom.GlobalPosition.X,
            Y = _bottom.GlobalPosition.Y - _playerPlatformDistanceFromBottom
        };
    }

    private void InitBricks()
    {
        // 计算可以容纳多少行多少列砖块
        var tempBrick = (Brick)_brickScene.Instantiate();
        var brickWidthWithGap = tempBrick.Size.X + _brickGapH;
        var brickHeightWithGap = tempBrick.Size.Y + _brickGapV;
        var bricksAreaHeight = Size.Y - _playerPlatformDistanceFromBottom - _playerPlatformDistanceFromBricks +
                               ((RectangleShape2D)_bottom.Shape).Size.Y / 2;
        var bricksAreaWidth = Size.X;
        var rowCount = (int)(bricksAreaHeight / brickHeightWithGap);
        var columnCount = (int)((bricksAreaWidth - _brickGapH) / brickWidthWithGap);

        // 计算生成起始点
        // 计算窗口左上角位置
        var topLeft = Center - Size / 2;
        // x轴起始点需要计算部分空余空间
        var bricksAreaWidthWithoutEdge = columnCount * tempBrick.Size.X + (columnCount - 1) * _brickGapH;
        var xStart = topLeft.X + (Size.X - bricksAreaWidthWithoutEdge) / 2;
        // y轴起始点直接从一个Gap距离开始就行
        var yStart = topLeft.Y + _brickGapV;
        // 起始点从左上角转换到中心
        xStart += tempBrick.Size.X / 2;
        yStart += tempBrick.Size.Y / 2;
        // 销毁临时的砖块
        tempBrick.QueueFree();

        // 生成砖块
        var brickContainer = new Node2D();
        brickContainer.Name = "brickContainer";
        AddChild(brickContainer);
        var yCurPos = yStart;
        for (var row = 0; row < rowCount; row++)
        {
            var xCurPos = xStart;
            for (var column = 0; column < columnCount; column++)
            {
                var brick = (Brick)_brickScene.Instantiate();
                brick.GlobalPosition = new Vector2(xCurPos, yCurPos);
                brickContainer.AddChild(brick);
                xCurPos += brick.Size.X + _brickGapH;
            }

            yCurPos += tempBrick.Size.Y + _brickGapV;
        }
    }

    #region Child

    [ExportGroup("ChildDontChange")] [Export]
    private CollisionShape2D _left;

    [Export] private CollisionShape2D _right;
    [Export] private CollisionShape2D _top;
    [Export] private CollisionShape2D _bottom;
    [Export] private CharacterBody2D _playerPlatform;
    [Export] private Ball _ball;
    [Export] private UI _ui;

    #endregion
}