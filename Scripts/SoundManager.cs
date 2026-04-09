using Godot;

public partial class SoundManager : Node
{
    public static SoundManager Instance { get; private set; }
    private AudioStream ButtonClickSound { get; set; } = ResourceLoader.Load<AudioStream>("res://Music/dragon-studio-button-press-382713.mp3");

    private AudioStreamPlayer _player;

    public override void _Ready()
    {
        if (Instance != null && Instance != this)
        {
            QueueFree();
            return;
        }
        Instance = this;

        // 关键：让它在场景切换时依然存活
        ProcessMode = ProcessModeEnum.Always;

        _player = new AudioStreamPlayer();
        AddChild(_player);
        _player.Bus = "Master";

        // ✅ 正确做法：监听“场景树根节点改变”事件
        GetTree().Root.ChildExitingTree += OnRootChildExiting;
        GetTree().Root.ChildEnteredTree += OnRootChildEntered;

        // 初始连接
        ConnectAllButtonsInScene(GetTree().Root);
    }

    // 当旧场景退出时（清理，防止鬼魂信号）
    private void OnRootChildExiting(Node node)
    {
        if (node is BaseButton button)
        {
            button.Pressed -= PlayButtonSound;
        }
    }

    // 当新场景进入时（连接新按钮）
    private void OnRootChildEntered(Node node)
    {
        CallDeferred(nameof(ConnectAllButtonsInScene), node);
    }

    private void ConnectAllButtonsInScene(Node root)
    {
        if (root == null) return;

        if (root is BaseButton button)
        {
            // 保险起见，先移除再添加（防止极端情况）
            button.Pressed += PlayButtonSound;
        }

        foreach (Node child in root.GetChildren())
        {
            ConnectAllButtonsInScene(child);
        }
    }

    private void PlayButtonSound()
    {
        GD.Print("🎵 SoundManager: 播放按钮点击音效");
        if (ButtonClickSound == null) return;
        GD.Print("🎵 SoundManager: ButtonClickSound 已设置，正在播放...");
        _player.Stop();
        _player.Stream = ButtonClickSound;
        _player.Play();
    }
}