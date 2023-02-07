namespace Inventory;

public abstract class Item
{
	public abstract InventoryItem ToInventoryItem(Inventory inventory, Panel panel);
}
