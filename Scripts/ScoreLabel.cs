using Godot;
using System.Text;
using System;
using System.Threading.Tasks;

public partial class ScoreLabel : Label
{
    private const string SAVE_PATH = "user://best_score.cfg";
    // 使用你的 Private Key 提交分数
    private const string DREAMLO_PRIVATE_KEY = "QBYmoAxc3EqRPZuh1X2QlQq-BQ35nN60uWEF48o_unww";

    [Export] public int BestScore = 0;
    [Export] public int CurrentScore = 0;
    // 文档1（ScoreLabel.cs）中修改myPlayerName的初始化
    private string myPlayerName;
    public override void _Ready()
    {
        // 先检查文件是否存在且内容有效
        if (FileAccess.FileExists("user://player_name.cfg"))
        {
            string fileContent = FileAccess.GetFileAsString("user://player_name.cfg").Trim();
            // 若文件内容是结构化配置（如含[Player]段），需提取名字（此处以你提供的错误内容为例）
            if (fileContent.Contains("Name=\""))
            {
                int start = fileContent.IndexOf("Name=\"") + 6;
                int end = fileContent.IndexOf("\"", start);
                myPlayerName = fileContent.Substring(start, end - start);
            }
            else
            {
                myPlayerName = fileContent; // 若文件是纯名字，直接使用
            }
        }
        else
        {
            myPlayerName = "Anonymous"; // 文件不存在时用默认名
        }
        GD.Print("玩家\"" + myPlayerName + "\"欢迎回来");
        // 后续逻辑不变...
        BestScore = LoadBestScore();
        UpdateText();
    }

    public void SetCurrent(int value)
    {
        CurrentScore = value;
        UpdateText();
    }

    public async Task SetBest(int value)
    {
        if (value > BestScore)
        {
            BestScore = value;
            SaveBestScore();
            await ScoreSender.Instance.AddScore(myPlayerName, BestScore);
        }
    }


    private void SaveBestScore()
    {
        var config = new ConfigFile();
        config.SetValue("Player", "BestScore", BestScore);
        Error err = config.Save(SAVE_PATH);
        if (err != Error.Ok)
            GD.PrintErr("保存失败: " + err);
        else
            GD.Print("最佳分数已保存: " + BestScore);
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

    private void UpdateText()
    {
        Text = $"HI  {BestScore:D5}  {CurrentScore:D5}";
    }
    private void CallDeferred(Action value)
    {
        throw new NotImplementedException();
    }

}