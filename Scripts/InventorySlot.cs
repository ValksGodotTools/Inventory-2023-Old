namespace Inventory;

public class InventorySlot
{
	private Panel Panel { get; set; }
	private InventoryItem InventoryItem { get; set; }
	private Inventory Inventory { get; set; }

	public InventorySlot(Inventory inv, Node parent)
	{
		Inventory = inv;

		Panel = new Panel
		{
			CustomMinimumSize = Vector2.One * Inventory.SlotSize
		};

		parent.AddChild(Panel);
	}

	public void AddDebugLabel(string text)
	{
		var label = new Label
		{
			Text = text,
			CustomMinimumSize = Vector2.One * Inventory.SlotSize,
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
		};

		label.AddThemeColorOverride("font_shadow_color", Colors.Black);

		Panel.AddChild(label);
	}

	public void SetItem(Item item)
	{
		InventoryItem?.QueueFree();
		InventoryItem = item.ToInventoryItem(Inventory, Panel);
	}
}
