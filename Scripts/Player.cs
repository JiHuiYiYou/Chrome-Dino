using Godot;

public partial class Player : CharacterBody2D
{
    public bool isAlive=true;
    private AnimatedSprite2D animatedSprite;
    [Export]
    public float JumpVelocity = -400f;

    [Export]
    public float Gravity = 980f;


    public override void _Ready()
    {
        animatedSprite=GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        var signals = Signals.Instance;
        if (signals == null)
        {
            GD.PrintErr("❌ GameManager 找不到 Signals 单例！");
            return;
        }

        signals.PlayerDied += OnPlayerDied;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!isAlive)
        {
            Velocity = new Vector2(0, 0);
            MoveAndSlide();
            AnimationControl();
            return;
        }
        if (IsOnFloor())
        {
            Gravity=2500;
        }
        Velocity = new Vector2(0, Velocity.Y + Gravity * (float)delta);

        if (Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            Velocity = new Vector2(0, JumpVelocity);
        }
        else if (Input.IsActionJustPressed("crouch"))
        {
            Gravity=99999;
            GetNode<AnimatedSprite2D>("Boom").Play("default");
        }
        

        MoveAndSlide();
        AnimationControl();
    }
    private async void AnimationControl()
	{
        if (!isAlive)
		{
			return;
		}
		//jump crunch run
        if (!IsOnFloor())
		{
			animatedSprite.Play("Jump");
		}
        else if (Input.IsActionPressed("crouch"))
        {
            animatedSprite.Play("Crouch");
        }
        else
		{
			animatedSprite.Play("Run");
		}
    }
    private void OnPlayerDied()
    {
        animatedSprite.Play("Death");
        GD.Print("🎮 GameManager: 玩家死亡信号执行");
        isAlive=false;
    }
}