using Godot;

namespace Inventory;

public class InventoryItem
{
	public InventoryItem(Node parent, string name)
	{
		var animatedSprite = new AnimatedSprite2D
		{
			SpriteFrames = GD.Load<SpriteFrames>($"res://{name}.tres"),
			Position = new Vector2(25, 25),
			Scale = Vector2.One * 2
		};

		animatedSprite.Play();

		parent.AddChild(animatedSprite);
	}
}
