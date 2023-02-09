namespace Inventory;

public static class UtilsInventory
{
	private static bool PlayerInventoryVisible { get; set; }

	public static void HandleInput(InputEvent @event)
	{
		InputGame.Handle(@event);

		if (Input.IsActionJustPressed("interact"))
		{
			Inventory.ActiveChest?.Open();
		}

		if (Input.IsActionJustPressed("inventory"))
		{
			if (PlayerInventoryVisible)
			{
				Player.Inventory.SwitchToHotbar();
			}
			else
			{
				Player.Inventory.SwitchToFullInventory();
			}

			PlayerInventoryVisible = !PlayerInventoryVisible;
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

		for (int i = 0; i < Player.Inventory.Columns; i++)
			InputHotbar(i);

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
		if (Input.IsActionJustPressed($"inventory_hotbar_{hotbar + 1}"))
		{
			var activeInvSlot = Inventory.ActiveInventorySlot;

			if (activeInvSlot == null)
				return;

			var activeInvSlotItem = activeInvSlot.InventoryItem;

			if (activeInvSlotItem == null)
				return;

			var playerInv = Player.Inventory;
			var columns = playerInv.Columns;
			var playerInvSlots = playerInv.InventorySlots;

			if (columns <= hotbar)
				return;

			var hotbarSlot = playerInvSlots[playerInvSlots.Length - columns + hotbar];

			if (activeInvSlot == hotbarSlot)
				return;

			ItemPanelDescription.Clear();

			if (hotbarSlot.InventoryItem == null)
			{
				// Just move the item over
				hotbarSlot.SetItem(activeInvSlotItem.Item);
				activeInvSlot.RemoveItem();
			}
			else
			{
				if (hotbarSlot.InventoryItem.Item.Type == activeInvSlotItem.Item.Type)
				{
					// Same type of item

					// Add the item counts together
					activeInvSlotItem.Item.Count += hotbarSlot.InventoryItem.Item.Count;

					// Just move the item over
					hotbarSlot.SetItem(activeInvSlotItem.Item);
					activeInvSlot.RemoveItem();
				}
				else
				{
					// Different type of item

					// Swap the items
					var hotbarItem = hotbarSlot.InventoryItem.Item;

					hotbarSlot.SetItem(activeInvSlotItem.Item);
					activeInvSlot.SetItem(hotbarItem);
				}
			}
		}
	}
}
