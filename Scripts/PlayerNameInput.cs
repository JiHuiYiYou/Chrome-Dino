using Godot;
using System;
using System.Threading.Tasks;

public partial class PlayerNameInput : Control
{
    [Export] public LineEdit NameInput { get; set; }
    private string name;
    private string myPlayerName;
    private const string SAVE_PATH = "user://best_score.cfg";
    [Export] private int BestScore = 0;

    public override void _Ready()
    {
        if (FileAccess.FileExists("user://player_name.cfg"))
        {
            string fileContent = FileAccess.GetFileAsString("user://player_name.cfg").Trim();
            if (fileContent.Contains("Name=\""))
            {
                int start = fileContent.IndexOf("Name=\"") + 6;
                int end = fileContent.IndexOf("\"", start);
                myPlayerName = fileContent.Substring(start, end - start); // 正确提取名字
            }
            else
            {
                myPlayerName = fileContent; // 纯名字直接使用
            }
        }
        else
        {
            myPlayerName = "Anonymous"; // 默认名
        }
        GD.Print("玩家\"" + myPlayerName + "\"欢迎回来");
        BestScore = LoadBestScore(); // 移除多余的打印与覆盖
    }
    private int LoadBestScore()
    {
        var config = new ConfigFile();
        Error err = config.Load(SAVE_PATH);
        if (err != Error.Ok)
        {
            GD.Print("没有找到存档，返回 0");
            return 0;
        }
        return (int)config.GetValue("Player", "BestScore", 0);
    }

    private async void OnConfirmPressed()
    {
        name = NameInput.Text.Trim();
        GD.Print(">>> 确认按钮被按下，输入的昵称: " + name);
        if (string.IsNullOrEmpty(name))
        {
            GD.Print("昵称不能为空，使用默认值 \"Player\"");
            name = "Player";
        }
        var config = new ConfigFile();
        config.SetValue("Player", "Name", name);
        config.Save("user://player_name.cfg");

        await ScoreSender.Instance.ChangeName(myPlayerName, name, BestScore);

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