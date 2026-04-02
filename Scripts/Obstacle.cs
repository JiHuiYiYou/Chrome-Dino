using Godot;
using System.Linq;

public partial class Obstacle : Area2D
{
	private float _floorSpeed = 0f;
	private Parallax2D _floorParallax; // 缓存地板 Parallax2D 节点



	public override void _Ready()
	{
		var world = GetTree().Root.GetNode("World");
		if (world == null)
		{
			GD.PrintErr("❌ World 节点找不到！");
			return;
		}

		var parallaxRoot = world.GetNode<Node2D>("ParallaxRoot");
		if (parallaxRoot == null)
		{
			GD.PrintErr("❌ ParallaxRoot 节点找不到！");
			return;
		}

		var parallaxChildren = parallaxRoot.GetChildren().OfType<Parallax2D>().ToArray();
		if (parallaxChildren.Length < 3)
		{
			GD.PrintErr("❌ ParallaxRoot 下的 Parallax2D 不足 3 个，找不到地板！");
			return;
		}

		_floorParallax = parallaxChildren[2]; // 缓存起来
		_floorSpeed = _floorParallax.Autoscroll.X;

		GD.Print($"✅ Obstacle _Ready: 获取到地板速度 = {_floorSpeed}");
	}

	public override void _Process(double delta)
	{
		if (_floorParallax == null)
		{
			GD.PrintErr("❌ _floorParallax is null!");
			return;
		}

		// 每帧都重新获取最新的速度（确保同步）
		_floorSpeed = _floorParallax.Autoscroll.X;

		// 移动
		Position += new Vector2(_floorSpeed * (float)delta, 0);

		// 超出屏幕销毁
		if (Position.X < -500)
		{
			QueueFree();
		}

		// 调试（可选）
		// GD.Print($"Obstacle Pos={Position.X:F1}, Speed={_floorSpeed:F1}");
	}
	public void OnPlayerBodyEnter(Node2D body)
	{
		if (body.Name == "Player")
		{
			GD.Print("💥 Player hit an obstacle!");

			// 发射全局死亡信号（先检查实例是否有效）
			if (Signals.Instance != null && IsInstanceValid(Signals.Instance))
			{
				Signals.Instance.EmitSignal(Signals.SignalName.PlayerDied);
				// 或者简写为：
				// Signals.Instance?.EmitSignal("PlayerDied");
				// 但更严谨的是用 IsInstanceValid 检查
			}
			else
			{
				GD.PrintErr("❌ Signals 实例已失效，无法发射 PlayerDied 信号！");
			}
		}
	}
}