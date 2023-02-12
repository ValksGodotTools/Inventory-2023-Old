namespace Inventory;

public partial class ItemCursor : Control, IItemHolder
{
	private Node ItemCursorParent { get; set; }
	public Item Item { get; set; }
	private Label LabelItemCount { get; set; }

	public override void _Ready()
	{
		SetPhysicsProcess(false);
		ItemCursorParent = this;
	}

	public override void _PhysicsProcess(double delta)
	{
		Position = GetViewport().GetMousePosition();
	}

	public void SetItem(Item item)
	{
		ItemPanelDescription.ToggleVisiblity(false);

		// There is an item in this cursor, lets move the parent now
		ItemCursorParent.SetPhysicsProcess(true);
		Item = item.Clone();

		ItemCursorParent.QueueFreeChildren();

		LabelItemCount = UtilsLabel.CreateItemCountLabel();
		LabelItemCount.ZIndex = 3;

		if (item.Count > 1)
			LabelItemCount.Text = item.Count + "";

		ItemCursorParent.AddChild(LabelItemCount);

		if (item.Type is ItemStatic itemStatic)
		{
			var staticSprite = new Sprite2D
			{
				Texture = itemStatic.Texture,
				Scale = Vector2.One * 2,
				ZIndex = 2 // ensure cursor item rendered above Inventory UI
			};

			ItemCursorParent.AddChild(staticSprite);
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

			ItemCursorParent.AddChild(animatedSprite);
		}

		// Quick and dirty way to center the label. This is how it has to be
		// for now until someone can figure out a better way
		LabelItemCount.Position = -LabelItemCount.Size / 2 + new Vector2(4, 0);
	}

	public Item GetItem()
	{
		if (ItemCursorParent.GetChildren().Count == 0)
			return null;

		return Item;
	}

	public void TakeItem()
	{
		if (Item.Count - 1 <= 0)
		{
			RemoveItem();
		}

		Item.Count -= 1;
		LabelItemCount.Text = Item.Count + "";
	}

	public void RemoveItem()
	{
		ItemPanelDescription.ToggleVisiblity(true);

		// Only move the parent when there is an item in this cursor
		ItemCursorParent.SetPhysicsProcess(false);
		ItemCursorParent.QueueFreeChildren();
	}
}
