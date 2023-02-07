namespace Inventory;

public class Item
{
	public ItemType Type { get; set; }
	public int Count { get; set; }

	public Item(ItemType type, int count = 1)
	{
		Type = type;
		Count = count;
	}
}
