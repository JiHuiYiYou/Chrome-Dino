using Godot;
using System;

public partial class PlayerNameInput : Control
{
    [Export] public LineEdit NameInput { get; set; }
	private string name;

    public override void _Ready()
    {
        GD.Print("进入输入昵称界面");
    }

    private void OnConfirmPressed()
    {
		name =  NameInput.Text.Trim();
		GD.Print(">>> 确认按钮被按下，输入的昵称: " + name);
        if (string.IsNullOrEmpty(name))
		{
			GD.Print("昵称不能为空，使用默认值 \"Player\"");
			name = "Player";
		}
        var config = new ConfigFile();
        config.SetValue("Player", "Name", name);
        config.Save("user://player_name.cfg");

        GD.Print("昵称已保存并跳转主场景: " + name);
        GetTree().CreateTimer(1.0f).Connect("timeout", Callable.From(() =>
		{
			GD.Print("Triangle Button Pressed!");
			// 在这里添加显示排行榜的逻辑
			GetTree().ChangeSceneToFile("res://Scenes/main.tscn");
		}));
    }
    private void OnQuitPressed()
    {
        GD.Print(">>> 退出按钮被按下，退出游戏");
        GetTree().CreateTimer(1.0f).Connect("timeout", Callable.From(() =>
		{
			GD.Print("Triangle Button Pressed!");
			// 在这里添加显示排行榜的逻辑
			GetTree().ChangeSceneToFile("res://Scenes/main.tscn");
		}));
    }
}