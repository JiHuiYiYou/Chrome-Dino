using Godot;
using System.Linq;

public partial class ParallaxController : Node2D
{
    [Export] private float _accelerationFactor = 0.5f;  // 加速度系数（原始速度的倍数）
    [Export] private float _maxSpeedFactor = 3.0f;       // 速度上限系数（原始速度的倍数）

    private Parallax2D[] _layers;
    private bool _isPlayerDead = false;

    public override void _Ready()
    {
        _layers = GetChildren().OfType<Parallax2D>().ToArray();
        GD.Print($"=== ParallaxController: Found {_layers.Length} layers ===");

        foreach (var layer in _layers)
        {
            // 保存原始速度（只读，不修改）
            float originalSpeed = layer.Autoscroll.X;

            // 自动计算加速度和上限
            float acceleration = originalSpeed * _accelerationFactor;
            float maxSpeed = originalSpeed * _maxSpeedFactor;

            // 保存这些参数到层的 UserData（或用字典缓存）
            layer.SetMeta("OriginalSpeed", originalSpeed);
            layer.SetMeta("Acceleration", acceleration);
            layer.SetMeta("MaxSpeed", maxSpeed);

            GD.Print($"Layer: {layer.Name}, Original={originalSpeed:F1}, Accel={acceleration:F1}, Max={maxSpeed:F1}");
        }
        var signals = Signals.Instance;
        if (signals == null)
        {
            GD.PrintErr("❌ GameManager 找不到 Signals 单例！");
            return;
        }

        signals.PlayerDied += OnPlayerDied;
    }

    public override void _Process(double delta)
    {
        if (_isPlayerDead)
            return;
        foreach (var layer in _layers)
        {
            // 读取之前保存的参数
            float originalSpeed = (float)layer.GetMeta("OriginalSpeed");
            float acceleration = (float)layer.GetMeta("Acceleration");
            float maxSpeed = (float)layer.GetMeta("MaxSpeed");

            // 当前速度（首次运行时从原始速度开始）
            float currentSpeed = layer.Autoscroll.X;
            if (float.IsNaN(currentSpeed) || float.IsInfinity(currentSpeed))
                currentSpeed = originalSpeed;

            // 计算新速度：加速直到上限
            float newSpeed = currentSpeed + acceleration * (float)delta;
            // GD.Print($"Layer: {layer.Name}, Current={currentSpeed:F1}, New={newSpeed:F1}, Accel={acceleration:F1}, Delta={delta:F3}");
            if (newSpeed < maxSpeed)
                newSpeed = maxSpeed;
            // GD.Print("抑制新速度到上限: " + newSpeed+" Max: " + maxSpeed );

            // 应用新速度
            layer.Autoscroll = new Vector2(newSpeed, layer.Autoscroll.Y);
        }
    }
    private void OnPlayerDied()
    {
        GD.Print("🎮 GameManager: 玩家死亡,画面停止");
        _isPlayerDead = true;

        // 停止所有层的滚动
        foreach (var layer in _layers)
        {
            layer.Autoscroll = Vector2.Zero;
        }
    }
}