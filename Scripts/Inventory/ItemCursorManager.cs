namespace Inventory;

public partial class ItemCursorManager : Control
{
	public ItemCursor ItemCursor { get; set; }

	public override void _Ready()
	{
		SetPhysicsProcess(false);

		ItemCursor = new ItemCursor(this);
	}

	public override void _PhysicsProcess(double delta)
	{
		Position = GetViewport().GetMousePosition();
	}
}
