using Godot;

public partial class Player : CharacterBody2D
{
    [Export]
    public float JumpVelocity = -400f;

    [Export]
    public float Gravity = 980f;

    public override void _PhysicsProcess(double delta)
    {
        Velocity = new Vector2(0, Velocity.Y + Gravity * (float)delta);

        if (Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            Velocity = new Vector2(0, JumpVelocity);
        }

        MoveAndSlide();
    }
}