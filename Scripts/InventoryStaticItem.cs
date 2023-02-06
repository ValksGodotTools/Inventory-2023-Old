namespace Inventory;

public class InventoryStaticItem : InventoryItem
{
	private Sprite2D Sprite2D { get; set; }

	public InventoryStaticItem(Node parent, string name)
	{
		Sprite2D = new Sprite2D
		{
			Texture = GD.Load<Texture2D>($"res://Sprites/{name}.png"),
			Position = new Vector2(25, 25),
			Scale = Vector2.One * 2
		};

		parent.AddChild(Sprite2D);
	}

	public override void QueueFree() => Sprite2D.QueueFree();
}
