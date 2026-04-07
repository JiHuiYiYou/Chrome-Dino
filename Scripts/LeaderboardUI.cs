using Godot;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public partial class LeaderboardUI : Control
{
    private static readonly System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
    
    public override async void _Ready()
    {
        // 创建UI
        CreateUI();
        
        // 加载数据
        await LoadAndShowData();
    }
    
    private void CreateUI()
    {
        // 垂直容器
        var vbox = new VBoxContainer();
        vbox.AnchorLeft = 0.5f;
        vbox.AnchorTop = 0.5f;
        vbox.AnchorRight = 0.5f;
        vbox.AnchorBottom = 0.5f;
        vbox.OffsetLeft = -150;
        vbox.OffsetTop = -200;
        vbox.OffsetRight = 150;
        vbox.OffsetBottom = 200;
        AddChild(vbox);
        
        // 标题
        var title = new Label();
        title.Text = "🏆 TOP 10";
        title.HorizontalAlignment = HorizontalAlignment.Center;
        title.AddThemeFontSizeOverride("font_size", 28);
        vbox.AddChild(title);
        
        // 创建10行
        for (int i = 0; i < 10; i++)
        {
            var hbox = new HBoxContainer();
            
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
            
            vbox.AddChild(hbox);
        }
    }
    
    private async Task LoadAndShowData()
    {
        string url = "http://dreamlo.com/lb/69cdfe068f40bc2f60cfd46e/pipe";
        
        try
        {
            string rawData = await client.GetStringAsync(url);
            var list = Parse(rawData);
            Show(list);
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
            var hbox = GetChild<VBoxContainer>(0).GetChild<HBoxContainer>(i + 1);
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
}