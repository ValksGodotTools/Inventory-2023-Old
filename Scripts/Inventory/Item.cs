namespace Inventory;

public class Item
{
	public ItemType Type  { get; set; }
	public int      Count { get; set; }

	public Item(ItemType type, int count)
	{
		Type = type;
		Count = count;
	}

	public Item Clone() => new(Type, Count);
	public void Hide() => Type.Hide();
	public void Show() => Type.Show();

	public override string ToString() => $"{Count} {Type}{(Count == 1 ? "" : "s")}";
}
