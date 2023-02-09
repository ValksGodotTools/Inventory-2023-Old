namespace Inventory;

public partial class Player : CharacterBody2D
{
	public static PlayerInventory Inventory { get; set; }

	// The speed of the player
	[Export] public float Speed { get; set; } = 500;

	// A friction of 1.0 will bring the player to a complete halt
	[Export(PropertyHint.Range, "0, 0.2")] public float Friction { get; set; } = 0.01f;

	private AnimatedSprite2D AnimatedSprite2D { get; set; }

	private Vector2 MoveVec { get; set; }

	public override void _Ready()
	{
		AnimatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		Inventory = new(this);

		Inventory.SetItem(1, new Item(Items.CoinRed));

		for (int i = 18; i < 27; i++)
			Inventory.SetItem(i, new Item(Items.CoinPink));
	}

	public override void _PhysicsProcess(double delta)
	{
		Animate();
		Move(delta);
	}

	public override void _Input(InputEvent @event)
	{
		UtilsInventory.HandleInput(@event);
	}

	private void Animate()
	{
		var rawInputVec = Utils.GetMovementInputRaw("player");

		if (rawInputVec == Vector2.Zero)
			AnimatedSprite2D.InstantPlay("idle");
		else if (rawInputVec.X > 0)
			AnimatedSprite2D.InstantPlay("walk_right");
		else if (rawInputVec.X < 0)
			AnimatedSprite2D.InstantPlay("walk_left");
		else if (rawInputVec.Y > 0)
			AnimatedSprite2D.InstantPlay("walk_down");
		else if (rawInputVec.Y < 0)
			AnimatedSprite2D.InstantPlay("walk_up");
	}

	private void Move(double delta)
	{
		MoveVec *= 1 - Friction;
		MoveVec += Utils.GetMovementInput("player") * Speed * (float)delta;
		Velocity = MoveVec;

		MoveAndSlide();
	}
}