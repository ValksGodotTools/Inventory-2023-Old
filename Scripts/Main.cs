global using Godot;
global using System;

namespace Inventory;

public partial class Main : Node
{
	public override void _Ready()
	{
		var inventory = new Inventory(this);
		inventory.SetItem(8, 1, "sprite_frames_coin");
	}
}
