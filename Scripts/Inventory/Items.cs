namespace Inventory;

public class Items
{
    #region Texture Loading methods

    private static SpriteFrames LoadSpriteFrames(string path) =>
    GD.Load<SpriteFrames>($"res://SpriteFrames/{path}.tres");

    private static Texture2D LoadTexture(string path) =>
        GD.Load<Texture2D>($"res://Sprites/{path}.png");
    #endregion
    #region Currencies

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
    #endregion
    #region Consumables

    public static ItemStatic PotionRed { get; } = new()
    {
        Name = "Red Potion",
        Description = "A potion with a red tint",
        ItemCategory = ItemCategory.Consumable,
        Texture = LoadTexture("potion_red")
    };

    public static ItemStatic PotionBlue { get; } = new()
    {
        Name = "Blue Potion",
        Description = "A potion with a blue tint",
        ItemCategory = ItemCategory.Consumable,
        Texture = LoadTexture("potion_blue")
    };
    #endregion
}