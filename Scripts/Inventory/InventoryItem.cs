namespace Inventory;

public abstract class InventoryItem
{
	public abstract bool Visible { get; set; }
	public abstract Node2D Node { get; }

	public Item Item { get; set; }

	public abstract Node2D GenerateGraphic();

	public void QueueFreeGraphic() => Node.QueueFree();
	public void Hide() => Node.Hide();
	public void Show() => Node.Show();
}
