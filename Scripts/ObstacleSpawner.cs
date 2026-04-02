using Godot;

public partial class ObstacleSpawner : Timer
{
    // ✅ 拖入你想随机生成的障碍物场景
    [Export] public PackedScene[] ObstacleScenes;

    [Export] public float MinInterval = 1.0f;
    [Export] public float MaxInterval = 5.0f;

    // ✅ 统一的生成位置（直接填）
    [Export] public Vector2 SpawnPosition = new Vector2(1500,456);

    public override void _Ready()
    {
        OneShot = false;
        WaitTime = GD.RandRange(MinInterval, MaxInterval);
        Timeout += OnTimeout;
        Start();

        var signals = Signals.Instance;
        if (signals == null)
        {
            GD.PrintErr("❌ Signals 单例未找到！");
            return;
        }

        signals.PlayerDied += OnPlayerDied;
    }

    private void OnTimeout()
    {
        if (ObstacleScenes == null || ObstacleScenes.Length == 0)
        {
            GD.PrintErr("❌ 没有设置任何障碍物场景！");
            return;
        }

        // ✅ 随机选一个障碍物
        if (ObstacleScenes.Length == 0) return;
        var index = GD.RandRange(0, ObstacleScenes.Length - 1);
        var scene = ObstacleScenes[(int)index];
        var obstacle = scene.Instantiate<Area2D>();

        // ✅ 使用统一指定的生成位置
        GetParent().AddChild(obstacle);
        obstacle.GlobalPosition = SpawnPosition;

        // ✅ 下一次随机时间
        WaitTime = GD.RandRange(MinInterval, MaxInterval);
    }

    private void OnPlayerDied()
    {
        GD.Print("🎮 Spawner 收到玩家死亡信号，停止生成");
        Stop();
    }
}