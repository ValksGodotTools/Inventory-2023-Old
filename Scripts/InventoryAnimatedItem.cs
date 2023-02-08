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

		AnimatedSprite2D.Frame = GD.RandRange(0, AnimatedSprite2D.SpriteFrames.GetFrameCount("default") - 1);
		AnimatedSprite2D.Play();

		parent.AddChild(AnimatedSprite2D);
	}

	public override void QueueFreeGraphic() => AnimatedSprite2D.QueueFree();
}
