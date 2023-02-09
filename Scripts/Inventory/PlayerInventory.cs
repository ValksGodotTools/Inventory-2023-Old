namespace Inventory;

public class PlayerInventory : Inventory
{
	public Inventory Hotbar { get; set; }

	public PlayerInventory(Node node, int columns = 9, int rows = 5, int slotSize = 50) 
		: base(node, columns, rows, slotSize)
	{
		Hotbar = new Inventory(node, 9, 1);
		Hotbar.SetAnchor(Control.LayoutPreset.CenterBottom);
		Hotbar.MakePanelInvisible();
		Hotbar.Show();
	}

	public override void SetItem(int i, Item item)
	{
		base.SetItem(i, item);

		GD.Print(i);

		if (i <= Columns)
			Hotbar.InventorySlots[i].SetItem(item);
	}

	public override void SetItem(int x, int y, Item item)
	{
		base.SetItem(x, y, item);

		var i = x + y * Columns;

		if (i <= Columns)
			Hotbar.InventorySlots[i].SetItem(item);
	}
}
