namespace Inventory;

public class Item
{
    public ItemType Type { get; set; }
    public int Count { get; set; }

    public int Stacklimit { get; set; }

    public Item(ItemType type, int count, int stacklimit)
    {
        Type = type;
        Count = count;
        Stacklimit = stacklimit;
    }

    public Item Clone() => new(Type, Count, Stacklimit);
    public void Hide() => Type.Hide();
    public void Show() => Type.Show();

    public override string ToString() => $"{Count} {Type}{(Count == 1 ? "" : "s")}";
}
