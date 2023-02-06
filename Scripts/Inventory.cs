namespace Inventory;

public class Inventory
{
	private InventorySlot[] InventorySlots { get; set; }
	private GridContainer GridContainer { get; set; }
	private int Padding { get; set; } = 10;
	private int Columns { get; set; }
	private int Rows { get; set; }

	public Inventory(Node parent, int columns = 9, int rows = 5)
	{
		Columns = columns;
		Rows = rows;

		var panelContainer = new PanelContainer();

		var marginContainer = new MarginContainer();

		foreach (var margin in new string[] { "left", "right", "top", "bottom" })
			marginContainer.AddThemeConstantOverride($"margin_{margin}", Padding);

		panelContainer.AddChild(marginContainer);

		GridContainer = new GridContainer { Columns = columns };

		marginContainer.AddChild(GridContainer);

		InventorySlots = new InventorySlot[rows * columns];

		for (int i = 0; i < InventorySlots.Length; i++)
			InventorySlots[i] = new InventorySlot(GridContainer);

		parent.AddChild(panelContainer);
	}

	public void SetItem(int i, string name) =>
		InventorySlots[i].SetItem(name);

	public void SetItem(int x, int y, string name) =>
		SetItem(x + y * Columns, name);
}
