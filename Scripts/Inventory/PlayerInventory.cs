namespace Inventory;

public class PlayerInventory : Inventory
{
	public PlayerInventory(Node node, int columns = 9, int rows = 5, int slotSize = 50) 
		: base(node, columns, rows, slotSize)
	{
		SetAnchor(Control.LayoutPreset.CenterBottom);
		SwitchToHotbar();
		Show();
	}

	public void SwitchToFullInventory()
	{
		MakePanelVisible();
		SetSlotsVisibility(Columns, InventorySlots.Length, true);
	}

	public void SwitchToHotbar()
	{
		MakePanelInvisible();
		SetSlotsVisibility(Columns, InventorySlots.Length, false);
	}
}
