namespace Inventory;

public class InventoryAnimatedItem : InventoryItem
{
	private AnimatedSprite2D AnimatedSprite2D { get; set; }

	public InventoryAnimatedItem(Inventory inv, Node parent, string name)
	{
		AnimatedSprite2D = new AnimatedSprite2D
		{
			SpriteFrames = GD.Load<SpriteFrames>($"res://{name}.tres"),
			Position = Vector2.One * (inv.SlotSize / 2),
			Scale = Vector2.One * (inv.SlotSize / 25)
		};

		AnimatedSprite2D.Play();

		parent.AddChild(AnimatedSprite2D);

		// Label for keeping track of the item stack counts.
		//var label = new Label { Text = "0" };
		//parent.AddChild(label);
	}

	public override void QueueFree() => AnimatedSprite2D.QueueFree();
}
