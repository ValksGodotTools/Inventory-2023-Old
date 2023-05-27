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
        Stacklimit = 10,
        ItemCategory = ItemCategory.Currency,
        Texture = LoadTexture("coin_red")
    };

    public static ItemStatic CoinPink { get; } = new()
    {
        Name = "Pink Coin",
        Description = "A coin with a pink tint",
        Stacklimit = 10,
        ItemCategory = ItemCategory.Currency,
        Texture = LoadTexture("coin_pink")
    };

    public static ItemAnimated Coin { get; } = new()
    {
        Name = "Golden Coin",
        Description = "A dancing coin",
        Stacklimit = 10,
        ItemCategory = ItemCategory.Currency,
        SpriteFrames = LoadSpriteFrames("coin")
    };
    #endregion
    #region Consumables

    public static ItemStatic PotionRed { get; } = new()
    {
        Name = "Red Potion",
        Description = "A potion with a red tint",
        Stacklimit = 100,
        ItemCategory = ItemCategory.Consumable,
        Texture = LoadTexture("potion_red")
    };

    public static ItemStatic PotionBlue { get; } = new()
    {
        Name = "Blue Potion",
        Description = "A potion with a blue tint",
        Stacklimit = 100,
        ItemCategory = ItemCategory.Consumable,
        Texture = LoadTexture("potion_blue")
    };
    #endregion
    #region Weapons
    public static ItemStatic SwordIron { get; } = new()
    {
        Name = "Iron Sword",
        Description = "For slaying dragons or die trying",
        Stacklimit = 1,
        ItemCategory = ItemCategory.Weapon,
        Texture = LoadTexture("Weapons/sword_iron")
    };

    public static ItemStatic SwordWooden { get; } = new()
    {
        Name = "Wooden Sword",
        Description = "Perfect for tickling foes gently",
        Stacklimit = 1,
        ItemCategory = ItemCategory.Weapon,
        Texture = LoadTexture("Weapons/sword_wood")
    };
    #endregion
}