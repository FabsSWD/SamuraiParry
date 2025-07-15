using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
  public float Speed = 200.0f;
  private AnimatedSprite2D animatedSprite;
  private bool isAttacking = false;
  public float gravity = (float)ProjectSettings.GetSetting("physics/2d/default_gravity");
	
  public override void _Ready()
  {
	  animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	  animatedSprite.AnimationFinished += OnAnimationFinished;
  }

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Apply gravity
		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta;
		}

		// Attack logic
		if (Input.IsActionJustPressed("attack") && !isAttacking)
		{
			isAttacking = true;
			animatedSprite.Play("Attack");
		}

		// Movement logic
		if (!isAttacking)
		{
			float inputX = Input.GetAxis("move_left", "move_right");
			Vector2 direction = new Vector2(inputX, 0);

			if (direction != Vector2.Zero)
			{
				velocity.X = direction.X * Speed;
				animatedSprite.Play("Run");
				animatedSprite.FlipH = direction.X < 0;
			}
			else
			{
				velocity.X = Mathf.MoveToward(velocity.X, 0, Speed);
				animatedSprite.Play("Idle");
			}
		}
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, Speed * 2);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private void OnAnimationFinished()
	{
		if (animatedSprite.Animation == "Attack")
		{
			isAttacking = false;
		}
	}
}
