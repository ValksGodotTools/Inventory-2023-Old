global using Godot;
global using System;

namespace Inventory;

public partial class Main : Node
{
	public override void _Ready()
	{
		var inventory = new Inventory(this);
		inventory.SetAnimatedItem(0, 1, "sprite_frames_coin");
		inventory.SetStaticItem  (1, 1, "snowy_coin");

		for (int i = 0; i < inventory.InventorySlots.Length; i++)
			inventory.InventorySlots[i].AddDebugLabel("" + i);
	}
}
