namespace Inventory;

public class ItemCursor : ItemHolder
{
	private Node Parent { get; set; }

	public ItemCursor(Node parent)
	{
		Parent = parent;
	}

	public override void SetItem(Item item)
	{
		ItemPanelDescription.ToggleVisiblity(false);

		// There is an item in this cursor, lets move the parent now
		Parent.SetPhysicsProcess(true);
		Item = item.Clone();

		Parent.QueueFreeChildren();

		ItemCountLabel = UtilsLabel.CreateItemCountLabel();
		ItemCountLabel.ZIndex = 3;

		if (item.Count > 1)
			ItemCountLabel.Text = item.Count + "";

		Parent.AddChild(ItemCountLabel);

		if (item.Type is ItemStatic itemStatic)
		{
			var staticSprite = new Sprite2D
			{
				Texture = itemStatic.Texture,
				Scale = Vector2.One * 2,
				ZIndex = 2 // ensure cursor item rendered above Inventory UI
			};

			Parent.AddChild(staticSprite);
		}

		if (item.Type is ItemAnimated itemAnimated)
		{
			var animatedSprite = new AnimatedSprite2D
			{
				SpriteFrames = itemAnimated.SpriteFrames,
				Scale = Vector2.One * 2,
				ZIndex = 2 // ensure cursor item rendered above Inventory UI
			};

			animatedSprite.Play();

			Parent.AddChild(animatedSprite);
		}

		// Quick and dirty way to center the label. This is how it has to be
		// for now until someone can figure out a better way
		ItemCountLabel.Position = -ItemCountLabel.Size / 2 + new Vector2(4, 0);
	}

	public override void RemoveItem()
	{
		ItemPanelDescription.ToggleVisiblity(true);

		// Only move the parent when there is an item in this cursor
		Parent.SetPhysicsProcess(false);
		Parent.QueueFreeChildren();
	}

	public Item GetItem()
	{
		if (Parent.GetChildren().Count == 0)
			return null;

		return Item;
	}
}
