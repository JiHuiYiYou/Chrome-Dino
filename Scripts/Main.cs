using Godot;
using System;

public partial class Main : Control
{
	private bool hasCheckedPlayerName = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		GD.Print(">>> Main scene ready, checking player name...");
		GD.Print("玩家\"" + FileAccess.GetFileAsString("user://player_name.cfg") + "\"欢迎回来");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!hasCheckedPlayerName)
		{
			CheckPlayerName();
			hasCheckedPlayerName = true;
		}
	}
	public void _on_Button_pressed()
	{
		GetTree().CreateTimer(1.0f).Connect("timeout", Callable.From(() =>
		{
			GD.Print(">>> 切换场景到 GameOver");
			GetTree().ChangeSceneToFile("res://Scenes/World.tscn");
		}));
	}
	public void OnButtonQuitClick()
	{
		GetTree().CreateTimer(1.0f).Connect("timeout", Callable.From(() =>
		{
			GD.Print(">>> 退出游戏");
			GetTree().Quit();
		}));
	}
	public void OnButtonListClick()
	{
		GetTree().CreateTimer(1.0f).Connect("timeout", Callable.From(() =>
		{
			GD.Print(">>> 显示排行榜");
			// 在这里添加显示排行榜的逻辑
			GetTree().ChangeSceneToFile("res://Scenes/Leaderboard.tscn");
		}));
	}
	private void CheckPlayerName()
	{
		if (!FileAccess.FileExists("user://player_name.cfg"))
		{
			GetTree().ChangeSceneToFile("res://scenes/player_name_input.tscn");
		}
	}
}
