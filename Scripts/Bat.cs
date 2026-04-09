using Godot;
using System;

public partial class Bat : Area2D
{
    // 速度范围（都是负值，表示向左移动）
    private readonly float MaxSpeed = -800f;
    private readonly float MinSpeed = -400f;
    private AnimatedSprite2D animatedSprite;

    // 每个 Bat 实例自己的固定速度
    private float _fixedSpeed;

    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        // 只在初始化时随机一次！
        _fixedSpeed = (float)GD.RandRange(MinSpeed, MaxSpeed);
        
        // （可选）调试输出，可以看到每个 Bat 的速度是多少
        GD.Print($"🦇 Bat {_fixedSpeed:F1} 初始化，速度固定");
    }

    public override void _Process(double delta)
    {
        // 每帧用固定的速度移动
        Position += new Vector2(_fixedSpeed * (float)delta, 0);

        // 超出屏幕左边界就销毁
        if (Position.X < -500)
        {
            QueueFree();
        }
    }

    public void OnPlayerBodyEnter(Node2D body)
    {
        if (body.Name == "Player")
        {
            GD.Print("💥 Player hit a bat!");
            _fixedSpeed=0; // 碰到玩家后停止移动
            animatedSprite.Stop(); // 停止动画

            // 发射全局死亡信号
            Signals.Instance?.EmitSignal("PlayerDied");
        }
    }
}