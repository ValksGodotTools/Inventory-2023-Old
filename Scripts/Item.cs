namespace Inventory;

public abstract class Item
{
	public string Name { get; set; }
	public string Description { get; set; }

	public abstract InventoryItem ToInventoryItem(Inventory inventory, Panel panel);
}
