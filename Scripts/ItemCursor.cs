namespace Inventory;

public partial class ItemCursor : Control
{
	private static Node ItemCursorParent { get; set; }
	private static Item Item { get; set; }

	public static void ClearItem()
	{
		ItemPanelDescription.ToggleVisiblity(true);

		// Only move the parent when there is an item in this cursor
		ItemCursorParent.SetPhysicsProcess(false);

		foreach (Node child in ItemCursorParent.GetChildren())
			child.QueueFree();
	}

	public static Item GetItem()
	{
		if (ItemCursorParent.GetChildren().Count == 0)
			return null;

		return Item;
	}

	public static void SetItem(Item item)
	{
		ItemPanelDescription.ToggleVisiblity(false);

		// There is an item in this cursor, lets move the parent now
		ItemCursorParent.SetPhysicsProcess(true);
		Item = item;

		foreach (Node child in ItemCursorParent.GetChildren())
			child.QueueFree();

		var itemCountLabel = UtilsLabel.CreateItemCountLabel();
		itemCountLabel.ZIndex = 1;

		if (item.Count > 1)
			itemCountLabel.Text = item.Count + "";
		
		ItemCursorParent.AddChild(itemCountLabel);

		if (item.Type is ItemStatic itemStatic)
		{
			var cursorItem = new Sprite2D
			{
				Texture = itemStatic.Texture,
				Scale = Vector2.One * 2,
				ZIndex = 1 // ensure cursor item rendered above Inventory UI
			};

			ItemCursorParent.AddChild(cursorItem);
		}
		
		if (item.Type is ItemAnimated itemAnimated)
		{
			var animatedItem = new AnimatedSprite2D
			{
				SpriteFrames = itemAnimated.SpriteFrames,
				Scale = Vector2.One * 2,
				ZIndex = 1 // ensure cursor item rendered above Inventory UI
			};

			animatedItem.Play();

			ItemCursorParent.AddChild(animatedItem);
		}

		// Quick and dirty way to center the label. This is how it has to be
		// for now until someone can figure out a better way
		itemCountLabel.Position = -itemCountLabel.Size / 2;
	}

	public override void _Ready()
	{
		SetPhysicsProcess(false);
		ItemCursorParent = this;
	}

	public override void _PhysicsProcess(double delta)
	{
		Position = GetViewport().GetMousePosition();
	}
}
