namespace Inventory;

public class UICursorSlot : UISlot
{
	private Viewport Viewport { get; set; }

	public UICursorSlot(Control parent)
	{
		Container = new(1);
		Parent = parent;
		Index = 0;

		Viewport = parent.GetViewport();
		parent.Position = Viewport.GetMousePosition();
		parent.SetPhysicsProcess(false);
	}

	public void Update()
	{
		Parent.Position = Viewport.GetMousePosition();
	}

	public override void Set(Item item)
	{
		base.Set(item);
		Parent.SetPhysicsProcess(true);
	}

	public override void Remove()
	{
		base.Remove();
		Parent.SetPhysicsProcess(false);
	}
}
