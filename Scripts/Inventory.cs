namespace Inventory;

public class Inventory
{
	public InventorySlot[] InventorySlots { get; set; }
	public int SlotSize { get; set; } = 50;

	private GridContainer GridContainer { get; set; }
	private int Padding { get; set; } = 10;
	private int Columns { get; set; }
	private int Rows { get; set; }
	private Node Parent { get; set; }
	private PanelContainer PanelContainer { get; set; }

	public Inventory(Node parent, int columns = 9, int rows = 5, int slotSize = 50)
	{
		Parent = parent;
		Columns = columns;
		Rows = rows;
		SlotSize = slotSize;

		PanelContainer = new PanelContainer();

		var marginContainer = new MarginContainer();

		foreach (var margin in new string[] { "left", "right", "top", "bottom" })
			marginContainer.AddThemeConstantOverride($"margin_{margin}", Padding);

		PanelContainer.AddChild(marginContainer);

		GridContainer = new GridContainer { Columns = columns };

		marginContainer.AddChild(GridContainer);

		InventorySlots = new InventorySlot[rows * columns];

		for (int i = 0; i < InventorySlots.Length; i++)
			InventorySlots[i] = new InventorySlot(this, GridContainer);

		Parent.AddChild(PanelContainer);
	}

	private void SetItem(int i, Item item) =>
		InventorySlots[i].SetItem(item);

	public void SetItem(int x, int y, Item item) =>
		SetItem(x + y * Columns, item);

}
