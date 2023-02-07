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

		if (item is ItemStatic itemStatic)
		{
			var cursorItem = new Sprite2D
			{
				Texture = itemStatic.Texture,
				Scale = Vector2.One * 2,
				ZIndex = 1 // ensure cursor item rendered above Inventory UI
			};

			ItemCursorParent.AddChild(cursorItem);
		}
		
		if (item is ItemAnimated itemAnimated)
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
