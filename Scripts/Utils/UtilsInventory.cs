namespace Inventory;

public static class UtilsInventory
{
	public static int IsHotbarHotkeyJustPressed()
	{
		for (int i = 0; i < Player.Inventory.Columns; i++)
			if (Input.IsActionJustPressed($"inventory_hotbar_{i + 1}"))
				return i;

		return -1;
	}

	public static int IsHotbarHotkeyPressed()
	{
		for (int i = 0; i < Player.Inventory.Columns; i++)
			if (Input.IsActionPressed($"inventory_hotbar_{i + 1}"))
				return i;

		return -1;
	}

	public static void HandleInput(InputEvent @event)
	{
		InputGame.Handle(@event);

		if (Input.IsActionJustPressed("interact"))
		{
			Inventory.ActiveChest?.Open();
		}

		if (Input.IsActionJustPressed("inventory"))
		{
			if (!Player.Inventory.IsHotbar)
			{
				Player.Inventory.SwitchToHotbarAnimated();
			}
			else
			{
				Player.Inventory.SwitchToFullInventoryAnimated();
			}
		}

		if (Input.IsActionJustPressed("inventory_take_all"))
		{
			ItemPanelDescription.Clear();

			Inventory.OtherInventory?.TakeAll();
		}

		if (Input.IsActionJustPressed("inventory_sort"))
		{
			ItemPanelDescription.Clear();

			Inventory.OtherInventory?.Sort();
			Player.Inventory.Sort();
		}

		var hotbar = IsHotbarHotkeyJustPressed();
		if (hotbar != -1)
			Player.Inventory.Actions.Enqueue(() => InputHotbar(hotbar));
			
		// DEBUG
		var debugInv = Inventory.OtherInventory;

		if (debugInv == null)
			return;

		if (Input.IsKeyPressed(Key.F1))
			for (int i = 0; i < debugInv.InventorySlots.Length; i++)
				debugInv.InventorySlots[i].SetDebugLabel(debugInv.InventorySlots[i].InventoryItem == null ? "null" : "item" + "");

		if (Input.IsKeyPressed(Key.F2))
			for (int i = 0; i < debugInv.InventorySlots.Length; i++)
				debugInv.InventorySlots[i].SetDebugLabel("");
	}

	private static void InputHotbar(int hotbar)
	{
		var cursorItem = Main.ItemCursor;

		if (cursorItem.GetItem() != null)
		{
			HotbarInv(cursorItem, hotbar);
			return;
		}

		var activeInvSlot = Inventory.ActiveInventorySlot;

		if (activeInvSlot != null && activeInvSlot.InventoryItem != null)
			HotbarInv(activeInvSlot, hotbar);
	}

	private static void HotbarInv(ItemHolder itemHolder, int hotbar)
	{
		var playerInv = Player.Inventory;
		var playerInvSlots = playerInv.InventorySlots;
		var columns = playerInv.Columns;

		// If for example there are only 5 columns and hotbar is 6 then this
		// hotbar does not exist because there are only 5 columns
		if (columns <= hotbar)
			return;

		// Get the hotbar slot this item is being transfered to
		var hotbarSlot = playerInvSlots[playerInvSlots.Length - columns + hotbar];

		// If the hotbar slot is the same slot as the slot we are hovering over then dont do anything
		if (itemHolder == hotbarSlot)
			return;

		// Prevent item panel description from lingering around when the item disappears from the cursor or inv slot
		ItemPanelDescription.Clear();

		// There is no item in the hotbar slot
		if (hotbarSlot.InventoryItem == null)
		{
			// Just move the item over
			hotbarSlot.SetItem(itemHolder.Item);
			itemHolder.RemoveItem();
		}
		// There is a item in the hotbar slot
		else
		{
			// The item in the hotbar slot and the item being transfered are of the same type
			if (hotbarSlot.InventoryItem.Item.Type == itemHolder.Item.Type)
			{
				// Add the item counts together
				itemHolder.Item.Count += hotbarSlot.InventoryItem.Item.Count;

				// Then move the item over
				hotbarSlot.SetItem(itemHolder.Item);
				itemHolder.RemoveItem();
			}
			// Hotbar slot item and item being transfered are not the same type
			else
			{
				itemHolder.SwapItem(hotbarSlot);
			}
		}
	}
}
