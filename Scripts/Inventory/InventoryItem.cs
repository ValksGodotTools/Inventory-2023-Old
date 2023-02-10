namespace Inventory;

public abstract class InventoryItem
{
	public abstract bool Visible { get; set; }

	public Item Item { get; set; }

	public abstract Node2D GenerateGraphic();
	public abstract void QueueFreeGraphic();
	public abstract void Hide();
	public abstract void Show();
}
