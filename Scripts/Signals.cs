using Godot;

public partial class Signals : Node
{
    [Signal]
    public delegate void PlayerDiedEventHandler();

    public static Signals Instance { get; private set; }

    public override void _EnterTree()
    {
        // if (Instance != null)
        // {
        //     QueueFree();
        //     return;
        // }

        Instance = this;
        Name = nameof(Signals);
        GD.Print("signal_enter_tree");
    }
}