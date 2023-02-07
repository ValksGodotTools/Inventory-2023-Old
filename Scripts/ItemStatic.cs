namespace Inventory;

public class ItemStatic : Item
{
	public Texture2D Texture { get; set; }

	public override InventoryItem ToInventoryItem(Inventory inventory, Panel panel) =>
		new InventoryStaticItem(inventory, panel, this);
}
