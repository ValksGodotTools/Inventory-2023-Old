namespace Inventory;

public class ItemStatic : ItemType
{
	public Texture2D Texture { get; set; }

	public override InventoryItem ToInventoryItem(Inventory inventory, Panel panel, Item item) =>
		new InventoryStaticItem(inventory, panel, this, item);
}
