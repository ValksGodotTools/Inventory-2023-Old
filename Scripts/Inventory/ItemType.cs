namespace Inventory;

public enum ItemCategory
{
    Weapon,
    Consumable,
    Currency,
}

public abstract class ItemType
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Node2D Node2D { get; set; }

    public ItemCategory ItemCategory { get; set; }
    public abstract Node2D GenerateGraphic();

    public void Hide() => Node2D.Hide();
    public void Show() => Node2D.Show();

    public override string ToString() => Name;
}
