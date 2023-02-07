namespace Inventory;

public class ItemAnimated : ItemType
{
	public SpriteFrames SpriteFrames { get; set; }

	public override InventoryItem ToInventoryItem(Inventory inventory, Panel panel, Item item) =>
		new InventoryAnimatedItem(inventory, panel, this, item);
}
