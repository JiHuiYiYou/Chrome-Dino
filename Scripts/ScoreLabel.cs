using Godot;
using System.Text;
using System;

public partial class ScoreLabel : Label
{
    private const string SAVE_PATH = "user://best_score.cfg";
    // 使用你的 Private Key 提交分数
    private const string DREAMLO_PRIVATE_KEY = "QBYmoAxc3EqRPZuh1X2QlQq-BQ35nN60uWEF48o_unww";

    [Export] public int BestScore = 0;
    [Export] public int CurrentScore = 0;

    public override void _Ready()
    {
        BestScore = LoadBestScore();
        UpdateText();
    }

    public void SetCurrent(int value)
    {
        CurrentScore = value;
        UpdateText();
    }

    public void SetBest(int value)
    {
        if (value > BestScore)
        {
            BestScore = value;
            SaveBestScore();
            UploadScore(GetPlayerName(), BestScore);
        }
    }

    private string GetPlayerName()
    {
        // 从配置加载玩家名称
        var config = new ConfigFile();
        Error err = config.Load("user://player_name.cfg");
        if (err == Error.Ok)
        {
            return (string)config.GetValue("Player", "Name", "Player");
        }
        return "Player";
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

    private void UploadScore(string playerName, int score)
    {
        var http = new HttpRequest();
        AddChild(http);

        http.RequestCompleted += (request, response_code, headers, body) =>
        {
            if (response_code == 200)
            {
                string response = Encoding.UTF8.GetString(body);
                GD.Print("上传成功! 服务器响应: " + response);
            }
            else
            {
                GD.PrintErr($"上传失败! 响应码: {response_code}");
                if (body != null)
                    GD.PrintErr("错误详情: " + Encoding.UTF8.GetString(body));
            }
            this.CallDeferred(() => http.QueueFree());
        };

        string encodedName = Uri.EscapeDataString(playerName);
        string url = $"http://dreamlo.com/lb/{DREAMLO_PRIVATE_KEY}/add/{encodedName}/{score}";
        
        GD.Print($"上传分数: {playerName} - {score}");
        GD.Print($"URL: {url}");
        
        http.Get(url);
    }

    private void CallDeferred(Action value)
    {
        throw new NotImplementedException();
    }

}