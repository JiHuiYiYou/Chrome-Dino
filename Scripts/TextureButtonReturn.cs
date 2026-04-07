using Godot;
using System;

public partial class TextureButtonReturn : TextureButton
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void OnButtonPressed()
	{
		GetTree().CreateTimer(1.0f).Connect("timeout", Callable.From(() =>
		{
			GD.Print("Triangle Button Pressed!");
			// 在这里添加显示排行榜的逻辑
			GetTree().ChangeSceneToFile("res://Scenes/main.tscn");
		}));
	}
}
