namespace Inventory;

public abstract class InventoryItem
{
	public Item Item { get; set; }

	public abstract void QueueFree();
}
