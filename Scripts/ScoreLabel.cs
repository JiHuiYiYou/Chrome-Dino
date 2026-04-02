using Godot;

public partial class ScoreLabel : Label
{
    private const string SAVE_PATH = "user://best_score.cfg";
    
    [Export] public int BestScore = 0;
    [Export] public int CurrentScore = 0;

    public override void _Ready()
    {
        // 启动时加载最佳分
        BestScore = LoadBestScore();
        UpdateText();
    }

    // 设置当前分数（由 World 调用）
    public void SetCurrent(int value)
    {
        CurrentScore = value;
        UpdateText();
    }

    // 设置最佳分数（由 World 在玩家死亡时调用）
    public void SetBest(int value)
    {
        if (value > BestScore)
        {
            BestScore = value;
            SaveBestScore();
        }
    }

    // 保存最佳分到配置文件
    private void SaveBestScore()
    {
        var config = new ConfigFile();
        config.SetValue("Player", "BestScore", BestScore);
        Error err = config.Save(SAVE_PATH);
        if (err != Error.Ok)
        {
            GD.PrintErr("保存失败: " + err);
        }
        else
        {
            GD.Print("最佳分数已保存: " + BestScore);
        }
    }

    // 从配置文件加载最佳分
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

    // 更新显示文本
    private void UpdateText()
    {
        // 即使当前分 > 最佳分，也不立即更新最佳分（因为还没保存！）
        Text = $"HI  {BestScore:D5}  {CurrentScore:D5}";
    }
}