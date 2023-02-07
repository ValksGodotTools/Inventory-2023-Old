namespace Inventory;

public static class Items
{
	// animated
	public static ItemAnimated Coin { get; } = LoadAnimated("sprite_frames_coin");

	// static
	public static ItemStatic CoinSnowy { get; } = LoadStatic("coin_snowy");
	public static ItemStatic CoinPink { get; } = LoadStatic("coin_pink");
	public static ItemStatic CoinRed { get; } = LoadStatic("coin_red");

	private static ItemAnimated LoadAnimated(string path) =>
		new() { SpriteFrames = GD.Load<SpriteFrames>($"res://{path}.tres") };
	private static ItemStatic LoadStatic(string path) =>
		new() { Texture = GD.Load<Texture2D>($"res://sprites/{path}.png") };
}
