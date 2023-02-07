namespace Inventory;

public class ItemAnimated : Item
{
	public SpriteFrames SpriteFrames { get; set; }

	public override InventoryItem ToInventoryItem(Inventory inventory, Panel panel) =>
		new InventoryAnimatedItem(inventory, panel, this);
}
