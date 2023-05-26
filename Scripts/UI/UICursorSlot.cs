namespace Inventory;

public class UICursorSlot : UISlot
{
    Viewport viewport;

    public UICursorSlot(Control parent)
    {
        container = new(1);
        Parent = parent;
        index = 0;

        viewport = parent.GetViewport();
        parent.Position = viewport.GetMousePosition();
        parent.SetPhysicsProcess(false);
    }

    public void Update()
    {
        Parent.Position = viewport.GetMousePosition();
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
