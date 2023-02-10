namespace Inventory;

public class InventoryAnimatedItem : InventoryItem
{
	public override bool Visible { get => AnimatedSprite2D.Visible; set => AnimatedSprite2D.Visible = value; }
	private AnimatedSprite2D AnimatedSprite2D { get; set; }
	private ItemAnimated ItemAnimated { get; set; }
	private Inventory Inv { get; set; }

	public InventoryAnimatedItem(Inventory inv, Node parent, ItemAnimated itemAnimated, Item item)
	{
		Item = item;
		Inv = inv;
		ItemAnimated = itemAnimated;
		AnimatedSprite2D = (AnimatedSprite2D)GenerateGraphic();

		parent.AddChild(AnimatedSprite2D);
	}

	public override Node2D GenerateGraphic()
	{
		var sprite = new AnimatedSprite2D
		{
			SpriteFrames = ItemAnimated.SpriteFrames,
			Position = Vector2.One * (Inv.SlotSize / 2),
			Scale = Vector2.One * (Inv.SlotSize / 25)
		};

		sprite.Frame = GD.RandRange(0, sprite.SpriteFrames.GetFrameCount("default") - 1);
		sprite.Play();

		return sprite;
	}

	public override void QueueFreeGraphic() => AnimatedSprite2D.QueueFree();
	public override void Hide() => AnimatedSprite2D.Hide();
	public override void Show() => AnimatedSprite2D.Show();
}
