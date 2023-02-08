namespace Inventory;

public class InventoryAnimatedItem : InventoryItem
{
	private AnimatedSprite2D AnimatedSprite2D { get; set; }

	public InventoryAnimatedItem(Inventory inv, Node parent, ItemAnimated itemAnimated, Item item)
	{
		Item = item;
		AnimatedSprite2D = new AnimatedSprite2D
		{
			SpriteFrames = itemAnimated.SpriteFrames,
			Position = Vector2.One * (inv.SlotSize / 2),
			Scale = Vector2.One * (inv.SlotSize / 25)
		};

		AnimatedSprite2D.Play();

		parent.AddChild(AnimatedSprite2D);

		// Label for keeping track of the item stack counts.
		//var label = new Label { Text = "0" };
		//parent.AddChild(label);
	}

	public override void QueueFreeGraphic() => AnimatedSprite2D.QueueFree();
}
