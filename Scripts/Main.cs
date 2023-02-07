global using Godot;
global using System;
global using System.Collections.Generic;

namespace Inventory;

public partial class Main : Node
{
	public override void _Ready()
	{
		var inventory = new Inventory(this);
		inventory.SetItem(0, 1, new Item(Items.Coin, 3));
		inventory.SetItem(1, 1, new Item(Items.CoinSnowy));

		//for (int i = 0; i < inventory.InventorySlots.Length; i++)
		//	inventory.InventorySlots[i].AddDebugLabel("" + i);
	}
}
