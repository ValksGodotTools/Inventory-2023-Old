namespace Inventory;

public static class Items
{
	// animated
	public static ItemAnimated Coin { get; } = new()
	{
		SpriteFrames = LoadSpriteFrames("sprite_frames_coin"),
		Name = "Coin",
		Description = "A shiny coin"
	};

	// static
	public static ItemStatic CoinSnowy { get; } = new()
	{
		Texture = LoadTexture("coin_snowy"),
		Name = "Snowy Coin",
		Description = "A frozen coin"
	};

	public static ItemStatic CoinPink { get; } = new()
	{
		Texture = LoadTexture("coin_pink"),
		Name = "Pink Coin",
		Description = "A coin with a pink tint to it"
	};

	public static ItemStatic CoinRed { get; } = new()
	{
		Texture = LoadTexture("coin_red"),
		Name = "Red Coin",
		Description = "A coin with a red tint to it"
	};

	private static SpriteFrames LoadSpriteFrames(string path) =>
		GD.Load<SpriteFrames>($"res://{path}.tres");

	private static Texture2D LoadTexture(string path) =>
		GD.Load<Texture2D>($"res://sprites/{path}.png");
}
