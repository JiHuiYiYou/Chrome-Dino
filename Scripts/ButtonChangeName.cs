using Godot;
using System;

public partial class ButtonChangeName : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Text = FileAccess.GetFileAsString("user://player_name.cfg");
	}
	public void OnButtonChangeNameClick()
	{
		GetTree().CreateTimer(1.0f).Connect("timeout", Callable.From(() =>
		{
			GD.Print(">>> 显示排行榜");
			// 在这里添加显示排行榜的逻辑
			GetTree().ChangeSceneToFile("res://scenes/player_name_input.tscn");
		}));
	}
}
