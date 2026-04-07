using Godot;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public partial class LeaderboardUI : Control
{
    private string myPlayerName = FileAccess.GetFileAsString("user://player_name.cfg");
    
    private static readonly System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
    private Label myRankLabel;  // 自己的排名Label
    
    public override async void _Ready()
    {
        CreateUI();
        await LoadAndShowData();
    }
    
    private void CreateUI()
    {
        // 主容器
        var mainVBox = new VBoxContainer();
        mainVBox.AnchorLeft = 0.5f;
        mainVBox.AnchorTop = 0.5f;
        mainVBox.AnchorRight = 0.5f;
        mainVBox.AnchorBottom = 0.5f;
        mainVBox.OffsetLeft = -150;
        mainVBox.OffsetTop = -250;
        mainVBox.OffsetRight = 150;
        mainVBox.OffsetBottom = 250;
        AddChild(mainVBox);
        
        // 标题
        var title = new Label();
        title.Text = "🏆 TOP 10";
        title.HorizontalAlignment = HorizontalAlignment.Center;
        title.AddThemeFontSizeOverride("font_size", 28);
        mainVBox.AddChild(title);
        
        // 排行榜列表
        var listVBox = new VBoxContainer();
        listVBox.AddThemeConstantOverride("separation", 3);
        mainVBox.AddChild(listVBox);
        
        // 创建10行
        for (int i = 0; i < 10; i++)
        {
            var hbox = new HBoxContainer();
            hbox.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            
            var rank = new Label();
            rank.Text = $"{i + 1}.";
            rank.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
            rank.AddThemeFontSizeOverride("font_size", 18);
            hbox.AddChild(rank);
            
            var name = new Label();
            name.Name = $"Name{i}";
            name.Text = "-";
            name.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            name.AddThemeFontSizeOverride("font_size", 18);
            hbox.AddChild(name);
            
            var score = new Label();
            score.Name = $"Score{i}";
            score.Text = "-";
            score.SizeFlagsHorizontal = SizeFlags.ShrinkEnd;
            score.AddThemeFontSizeOverride("font_size", 18);
            hbox.AddChild(score);
            
            listVBox.AddChild(hbox);
        }
        
        // ========== 新增：自己的排名显示 ==========
        var selfBox = new HBoxContainer();
        selfBox.AddThemeConstantOverride("separation", 10);
        mainVBox.AddChild(selfBox);
        
        var selfTitle = new Label();
        selfTitle.Text = "你的排名:";
        selfTitle.AddThemeFontSizeOverride("font_size", 20);
        selfBox.AddChild(selfTitle);
        
        myRankLabel = new Label();
        myRankLabel.Name = "MyRank";
        myRankLabel.Text = "未上榜";
        myRankLabel.AddThemeFontSizeOverride("font_size", 20);
        selfBox.AddChild(myRankLabel);
        // ========================================
    }
    
    private async Task LoadAndShowData()
    {
        string url = "http://dreamlo.com/lb/69cdfe068f40bc2f60cfd46e/pipe";
        
        try
        {
            string rawData = await client.GetStringAsync(url);
            var list = Parse(rawData);
            Show(list);
            FindMyRank(list);  // 新增：查找自己的排名
        }
        catch (Exception e)
        {
            GD.PrintErr($"加载失败: {e.Message}");
        }
    }
    
    private List<(string name, int score)> Parse(string data)
    {
        var result = new List<(string, int)>();
        string[] lines = data.Split('\n');
        
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            
            string[] parts = line.Split('|');
            if (parts.Length >= 2 && int.TryParse(parts[1], out int score))
            {
                result.Add((parts[0].Trim(), score));
            }
        }
        
        result.Sort(((string, int) a, (string, int) b) => b.Item2.CompareTo(a.Item2));
        return result.Count > 10 ? result.GetRange(0, 10) : result;
    }
    
    private void Show(List<(string name, int score)> list)
    {
        for (int i = 0; i < 10; i++)
        {
            var hbox = GetChild<VBoxContainer>(0).GetChild<VBoxContainer>(1).GetChild<HBoxContainer>(i);
            var nameLabel = hbox.GetNode<Label>($"Name{i}");
            var scoreLabel = hbox.GetNode<Label>($"Score{i}");
            
            if (i < list.Count)
            {
                nameLabel.Text = list[i].name;
                scoreLabel.Text = list[i].score.ToString();
            }
            else
            {
                nameLabel.Text = "";
                scoreLabel.Text = "";
            }
        }
    }
    
    // ========== 新增：查找自己的排名 ==========
    private void FindMyRank(List<(string name, int score)> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].name == myPlayerName)
            {
                myRankLabel.Text = $"第 {i + 1} 名 (分数: {list[i].score})";
                return;
            }
        }
        myRankLabel.Text = "未上榜";
    }
}