using Godot;
using System;

public partial class World : Node2D
{
	[Export] public CanvasLayer MainHUD;
	[Export] public CanvasLayer GameOverPanel;
	[Export] public ScoreLabel ScoreLabel;
    [Export] public float BaseScorePerSecond = 10;

    private float _elapsed;
    private int _currentScore;
	private bool _isGameOver = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameOverPanel.Visible = false;
		MainHUD.Visible = true;
		var signals = Signals.Instance;
		if (signals == null)
		{
			GD.PrintErr("❌ GameManager 找不到 Signals 单例！");
			return;
		}

		signals.PlayerDied += OnPlayerDied;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
		if (_isGameOver)
		{
			return;
		}
        _elapsed += (float)delta;

        // ✅ 线性加分（Dino 核心）
        int newScore = (int)(_elapsed * BaseScorePerSecond);
        if (newScore != _currentScore)
        {
            _currentScore = newScore;
            ScoreLabel.SetCurrent(_currentScore);
        }
    }
	private async void OnPlayerDied()
	{
		GD.Print("🎮 World: 玩家死亡信号执行");
		if (GameOverPanel != null)
		{
			GameOverPanel.Visible = true;
			GameOverPanel.ProcessMode = ProcessModeEnum.Always;
		}
		await ScoreLabel.SetBest(_currentScore);
		_isGameOver = true;
	}
	public void _on_Button_pressed()
	{
		GetTree().CreateTimer(1.0f).Connect("timeout", Callable.From(() =>
        {
            GD.Print(">>> 切换场景到 GameOver");
            GetTree().ChangeSceneToFile("res://Scenes/main.tscn");
        }));
	}
}
