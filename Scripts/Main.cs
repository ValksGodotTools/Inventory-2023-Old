global using Godot;
global using System;
global using System.Collections.Generic;

namespace Inventory;

public partial class Main : Node
{
	public override void _Ready()
	{
		var inventory = new Inventory(this);

		for (int i = 0; i < 9; i++)
			inventory.SetItem(i, new Item(Items.Coin));

		inventory.SetItem(0, 2, new Item(Items.Coin, 3));
		inventory.SetItem(1, 2, new Item(Items.CoinSnowy));

		//for (int i = 0; i < inventory.InventorySlots.Length; i++)
		//	inventory.InventorySlots[i].AddDebugLabel("" + i);
	}
}
