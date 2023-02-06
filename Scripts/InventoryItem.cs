namespace Inventory;

public class InventoryItem
{
	private AnimatedSprite2D AnimatedSprite2D { get; set; }

	public InventoryItem(Node parent, string name)
	{
		AnimatedSprite2D = new AnimatedSprite2D
		{
			SpriteFrames = GD.Load<SpriteFrames>($"res://{name}.tres"),
			Position = new Vector2(25, 25),
			Scale = Vector2.One * 2
		};

		AnimatedSprite2D.Play();

		parent.AddChild(AnimatedSprite2D);

		// Ensure sprite is at index 0 so labels can be drawn over it if needed
		// Another way of doing this could be to set the Z-index of the AnimatedSprite2D node
		// or set the layer for the Label node. Although this has not been tested and may
		// not work.
		parent.MoveChild(AnimatedSprite2D, 0);

		// Label for keeping track of the item stack counts.
		//var label = new Label { Text = "0" };
		//parent.AddChild(label);
	}

	public void QueueFree() => AnimatedSprite2D.QueueFree();
}
