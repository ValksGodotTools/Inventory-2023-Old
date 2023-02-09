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
		if (Input.IsKeyPressed(Key.F1))
			for (int i = 0; i < Player.Inventory.InventorySlots.Length; i++)
				Player.Inventory.InventorySlots[i].SetDebugLabel(i + "");

		if (Input.IsKeyPressed(Key.F2))
			for (int i = 0; i < Player.Inventory.InventorySlots.Length; i++)
				Player.Inventory.InventorySlots[i].SetDebugLabel("");
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
