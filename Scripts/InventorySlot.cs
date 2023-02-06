namespace Inventory;

public class InventorySlot
{
	private PanelContainer PanelContainer { get; set; }
	private InventoryItem InventoryItem { get; set; }

	public InventorySlot(Node parent)
	{
		var slotSize = 50;

		PanelContainer = new PanelContainer
		{
			CustomMinimumSize = Vector2.One * slotSize
		};

		parent.AddChild(PanelContainer);
	}

	public void SetItem(string name)
	{
		// Clear the previous item if any
		foreach (Node child in PanelContainer.GetChildren())
			child.QueueFree();

		InventoryItem = new InventoryItem(PanelContainer, name);
	}
}
