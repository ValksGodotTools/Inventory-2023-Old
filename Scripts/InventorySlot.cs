namespace Inventory;

public class InventorySlot
{
	private Panel Panel { get; set; }
	private InventoryItem InventoryItem { get; set; }
	private Inventory Inventory { get; set; }
	private Label ItemCountLabel { get; set; }

	public InventorySlot(Inventory inv, Node parent)
	{
		Inventory = inv;

		Panel = new Panel
		{
			CustomMinimumSize = Vector2.One * Inventory.SlotSize
		};

		HandleLeftClick();

		Panel.MouseEntered += () =>
		{
			if (InventoryItem == null)
				return;

			ItemPanelDescription.Display(InventoryItem.Item);
		};

		Panel.MouseExited += () =>
		{
			if (InventoryItem == null)
				return;

			ItemPanelDescription.Clear();
		};

		parent.AddChild(Panel);

		ItemCountLabel = new Label
		{
			HorizontalAlignment = HorizontalAlignment.Right,
			VerticalAlignment = VerticalAlignment.Bottom,
			CustomMinimumSize = new Vector2(48, 50) // 50 - 2 because push 2 pixels to left
		};
		ItemCountLabel.AddThemeColorOverride("font_shadow_color", Colors.Black);
		ItemCountLabel.AddThemeConstantOverride("shadow_outline_size", 5);
		ItemCountLabel.AddThemeFontSizeOverride("font_size", 16);
		Panel.AddChild(ItemCountLabel);
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
		if (item.Count > 1)
			ItemCountLabel.Text = item.Count + "";
		InventoryItem?.QueueFree();
		InventoryItem = item.Type.ToInventoryItem(Inventory, Panel, item);
	}
	
	private void HandleLeftClick()
	{
		Panel.GuiInput += (inputEvent) =>
		{
			if (inputEvent is not InputEventMouseButton inputMouseButtonEvent)
				return;

			if (inputMouseButtonEvent.IsLeftClickPressed())
			{
				ItemCountLabel.Text = "";

				var cursorItem = ItemCursor.GetItem();

				// There is no item in this inventory slot
				if (InventoryItem == null)
					HandleLeftClickNoInvItem(cursorItem);
				// There is a item in this inventory slot
				else
					HandleLeftClickInvItem(cursorItem);
			}
		};
	}

	private void HandleLeftClickNoInvItem(Item cursorItem)
	{
		// Is there a item attached to the cursor?
		if (cursorItem != null)
		{
			// Remove the item from the cursor
			ItemCursor.ClearItem();

			// Set the item in this inventory slot to the item from the cursor
			SetItem(cursorItem);
		}
	}

	private void HandleLeftClickInvItem(Item cursorItem)
	{
		// Store temporary reference to the item in this inventory slot
		var item = InventoryItem.Item;

		// Clear the item graphic for this inventory slot
		InventoryItem.QueueFree();
		InventoryItem = null;

		// Is there a item attached to the cursor?
		if (cursorItem != null)
		{
			// Remove the item from the cursor
			ItemCursor.ClearItem();

			// Set the item in this inventory slot to the item from the cursor
			SetItem(cursorItem);
		}

		// Attach the item from the inventory slot to the cursor (pick up the item)
		ItemCursor.SetItem(item);
	}
}
