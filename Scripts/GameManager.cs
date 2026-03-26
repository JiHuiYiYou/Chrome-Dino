// GameManager.cs
using Godot;

public partial class GameManager : Node
{
	public static GameManager Instance;

	public override void _Ready()
	{
		Instance = this;
	}

	// 你可以在这里写全局逻辑，比如游戏结束、分数、速度控制
	public void OnPlayerDied()
	{
		GD.Print("Game Over!");
		// 切换场景到 GameOver
		GetTree().ChangeSceneToFile("res://Scenes/main.tscn");
	}
}