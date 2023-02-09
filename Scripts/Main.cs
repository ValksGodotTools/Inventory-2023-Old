global using Godot;
global using System;
global using System.Linq;
global using System.Collections.Generic;

namespace Inventory;

public partial class Main : Node
{
	private Inventory ChestInv { get; set; }
	private Inventory PlayerInv { get; set; }

	public override void _Ready()
	{
		ChestInv = new Inventory(this);
		ChestInv.SetAnchor(Control.LayoutPreset.CenterTop);

		for (int i = 0; i < 9; i++)
			ChestInv.SetItem(i, new Item(Items.Coin));

		ChestInv.SetItem(0, 2, new Item(Items.Coin, 3));
		ChestInv.SetItem(1, 2, new Item(Items.CoinSnowy));

		PlayerInv = new Inventory(this);
		PlayerInv.SetAnchor(Control.LayoutPreset.CenterBottom);

		for (int i = 18; i < 27; i++)
			PlayerInv.SetItem(i, new Item(Items.CoinPink));

		Inventory.PlayerInventory = PlayerInv;
		Inventory.OtherInventory = ChestInv;
	}

	public override void _Input(InputEvent @event)
	{
		InputGame.Handle(@event);

		if (Input.IsActionJustPressed("inventory_take_all"))
		{
			ItemPanelDescription.Clear();

			Inventory.OtherInventory.TakeAll();
		}

		if (Input.IsActionJustPressed("inventory_sort"))
		{
			ItemPanelDescription.Clear();

			Inventory.OtherInventory.Sort();
			Inventory.PlayerInventory.Sort();
		}

		if (Input.IsActionJustPressed("inventory_hotbar_1"))
		{
			var activeInvSlot = Inventory.ActiveInventorySlot;

			if (activeInvSlot == null)
				return;

			var activeInvSlotItem = activeInvSlot.InventoryItem;

			if (activeInvSlotItem == null)
				return;

			if (Inventory.PlayerInventory.Columns <= 1)
				return;

			var hotbarSlot = Inventory.PlayerInventory.InventorySlots[0];

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

		/*if (Input.IsKeyPressed(Key.E))
		{
			for (int i = 0; i < ChestInv.InventorySlots.Length; i++)
			{
				var invSlot = ChestInv.InventorySlots[i];
				var invItem = invSlot.InventoryItem;
				invSlot.SetDebugLabel($"{(invItem == null ? "null" : "")}");
			}
				
		}

		if (Input.IsKeyPressed(Key.Q))
		{
			for (int i = 0; i < ChestInv.InventorySlots.Length; i++)
				ChestInv.InventorySlots[i].SetDebugLabel("");
		}*/
	}
}
