global using Godot;
global using System;
global using System.Collections.Generic;

namespace Inventory;

public partial class Main : Node
{
	private Inventory Inventory { get; set; }

	public override void _Ready()
	{
		Inventory = new Inventory(this);

		for (int i = 0; i < 9; i++)
			Inventory.SetItem(i, new Item(Items.Coin));

		Inventory.SetItem(0, 2, new Item(Items.Coin, 3));
		Inventory.SetItem(1, 2, new Item(Items.CoinSnowy));
	}

	public override void _Input(InputEvent @event)
	{
		if (Input.IsKeyPressed(Key.E))
		{
			for (int i = 0; i < Inventory.InventorySlots.Length; i++)
			{
				var invSlot = Inventory.InventorySlots[i];
				var invItem = invSlot.InventoryItem;
				invSlot.SetDebugLabel($"{(invItem == null ? "null" : "")}");
			}
				
		}

		if (Input.IsKeyPressed(Key.Q))
		{
			for (int i = 0; i < Inventory.InventorySlots.Length; i++)
				Inventory.InventorySlots[i].SetDebugLabel("");
		}
	}
}
