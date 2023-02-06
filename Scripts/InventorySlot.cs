namespace Inventory;

public class InventorySlot
{
	private Panel Panel { get; set; }
	private InventoryItem InventoryItem { get; set; }

	public InventorySlot(Node parent)
	{
		var slotSize = 50;

		Panel = new Panel
		{
			CustomMinimumSize = Vector2.One * slotSize
		};

		parent.AddChild(Panel);
	}

	public void AddDebugLabel(string text)
	{
		var label = new Label
		{
			Text = text,
			CustomMinimumSize = Vector2.One * 50,
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
		};

		label.AddThemeColorOverride("font_shadow_color", Colors.Black);

		Panel.AddChild(label);
	}

	public void SetItem(string name)
	{
		InventoryItem?.QueueFree();
		InventoryItem = new InventoryItem(Panel, name);
	}
}
