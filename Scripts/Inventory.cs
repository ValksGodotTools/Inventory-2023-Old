namespace Inventory;

public class Inventory
{
	private InventorySlot[,] InventorySlots { get; set; }
	private GridContainer GridContainer { get; set; }
	private int Padding { get; set; } = 10;

	public Inventory(Node parent, int columns = 9, int rows = 5)
	{
		var panelContainer = new PanelContainer();

		var marginContainer = new MarginContainer();

		foreach (var margin in new string[] { "left", "right", "top", "bottom" })
			marginContainer.AddThemeConstantOverride($"margin_{margin}", Padding);

		panelContainer.AddChild(marginContainer);

		GridContainer = new GridContainer { Columns = columns };

		marginContainer.AddChild(GridContainer);

		InventorySlots = new InventorySlot[columns, rows];

		for (int x = 0; x < columns; x++)
			for (int y = 0; y < rows; y++)
				InventorySlots[x, y] = new InventorySlot(GridContainer);

		parent.AddChild(panelContainer);
	}

	public void SetItem(int x, int y, string name)
	{
		InventorySlots[x, y].SetItem(name);
	}
}
