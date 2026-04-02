using Godot;

public partial class ObstacleSpawner : Timer
{
    [Export] public PackedScene CherryScene;
    [Export] public float MinCherryInterval = 1.0f;
    [Export] public float MaxCherryInterval = 5.0f;
    [Export] public float PosX = 1593;
    [Export] public float PosY = 432.0f;

    public override void _Ready()
    {
        OneShot = false;
        WaitTime = (float)GD.RandRange(MinCherryInterval, MaxCherryInterval);
        Timeout += OnTimeout;
        Start();
        var signals = Signals.Instance;
        if (signals == null)
        {
            GD.PrintErr("❌ GameManager 找不到 Signals 单例！");
            return;
        }

        signals.PlayerDied += OnPlayerDied;
    }

    private void OnTimeout()
    {
        if (CherryScene == null)
        {
            GD.PrintErr("❌ CherryScene 未设置！");
            return;
        }

        var cherry = CherryScene.Instantiate<Area2D>();
        GetParent().AddChild(cherry);
        cherry.Position = new Vector2(PosX, PosY);

        WaitTime = (float)GD.RandRange(MinCherryInterval, MaxCherryInterval);
    }
    private void OnPlayerDied()
    {
        GD.Print("🎮  计时器收到玩家死亡信号");
        // 停止生成新的障碍物
        Stop();
    }
}