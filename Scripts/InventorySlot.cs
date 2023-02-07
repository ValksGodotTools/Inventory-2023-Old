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

		Panel.GuiInput += (inputEvent) =>
		{
			if (inputEvent is not InputEventMouseButton inputMouseButtonEvent)
				return;

			if (inputMouseButtonEvent.IsLeftClickPressed())
			{
				var cursorItem = ItemCursor.GetItem();

				// There is no item in this inventory slot
				if (InventoryItem == null)
					HandleLeftClickNoInvItem(cursorItem);
				// There is a item in this inventory slot
				else
					HandleLeftClickInvItem(cursorItem);
			}
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
