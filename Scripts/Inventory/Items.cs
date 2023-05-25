namespace Inventory;

public class Items
{
    public static ItemStatic CoinRed { get; } = new()
    {
        Name = "Red Coin",
        Description = "A coin with a red tint",
        ItemCategory = ItemCategory.Currency,
        Texture = LoadTexture("coin_red")
    };

    public static ItemStatic CoinPink { get; } = new()
    {
        Name = "Pink Coin",
        Description = "A coin with a pink tint",
        ItemCategory = ItemCategory.Currency,
        Texture = LoadTexture("coin_pink")
    };

    public static ItemAnimated Coin { get; } = new()
    {
        Name = "Coin",
        Description = "A dancing coin",
        ItemCategory = ItemCategory.Currency,
        SpriteFrames = LoadSpriteFrames("coin")
    };

    private static SpriteFrames LoadSpriteFrames(string path) =>
        GD.Load<SpriteFrames>($"res://SpriteFrames/{path}.tres");

    private static Texture2D LoadTexture(string path) =>
        GD.Load<Texture2D>($"res://Sprites/{path}.png");
}
