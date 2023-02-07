namespace Inventory;

public class InventoryStaticItem : InventoryItem
{
	private Sprite2D Sprite2D { get; set; }

	public InventoryStaticItem(Inventory inv, Node parent, ItemStatic itemStatic, Item item)
	{
		Item = item;
		Sprite2D = new Sprite2D
		{
			Texture = itemStatic.Texture,
			Position = Vector2.One * (inv.SlotSize / 2),
			Scale = Vector2.One * (inv.SlotSize / 25)
		};

		parent.AddChild(Sprite2D);
	}

	public override void QueueFree() => Sprite2D.QueueFree();
}
