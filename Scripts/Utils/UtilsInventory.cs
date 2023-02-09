namespace Inventory;

public static class UtilsInventory
{
	public static void HandleInput(InputEvent @event)
	{
		InputGame.Handle(@event);

		if (Input.IsActionJustPressed("interact"))
		{
			Inventory.ActiveChest?.Open();
		}

		if (Input.IsActionJustPressed("inventory"))
		{
			Player.Inventory.ToggleVisibility();
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

			if (Player.Inventory.Columns <= hotbar)
				return;

			var hotbarSlot = Player.Inventory.InventorySlots[hotbar];

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
