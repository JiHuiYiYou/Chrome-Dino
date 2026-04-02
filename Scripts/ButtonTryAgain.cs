using Godot;
using System;

public partial class ButtonTryAgain : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void _on_Button_pressed()
	{
		GetTree().CreateTimer(1.0f).Connect("timeout", Callable.From(() =>
        {
            GD.Print(">>> 切换场景到 GameOver");
            GetTree().ChangeSceneToFile("res://Scenes/World.tscn");
        }));
	}
}
