using static Godot.Control;

namespace Inventory;

public class UIPlayerInventory : UIInventory
{
    public UIPlayerInventory(Node parent, int size, int columns) : base(parent, size, columns)
    {
        SetAnchor(LayoutPreset.CenterBottom);
    }

    public void SwitchToHotbarInstant()
    {
        HideBackPanel();
        SetSlotsVisibility(0, UIInventorySlots.Length - Columns, false, true);
    }

    public void SwitchToFullInventoryInstant()
    {
        ShowBackPanel();
        SetSlotsVisibility(0, UIInventorySlots.Length - Columns, true, true);
    }

    public void SwitchToHotbarAnimated(double exitTime = 1, double reEntryTime = 1) =>
        SwitchAnimated(exitTime, reEntryTime, () => SwitchToHotbarInstant());

    public void SwitchToFullInventoryAnimated(double exitTime = 1, double reEntryTime = 1) =>
        SwitchAnimated(exitTime, reEntryTime, () => SwitchToFullInventoryInstant());

    private void SwitchAnimated(double exitTime = 1, double reEntryTime = 1, Action action = default)
    {
        Transition(exitTime, false, true);

        Tween.TweenCallback(Callable.From(() =>
        {
            action();

            // SetAnchor() mucks up position so lets reset it
            PanelContainer.Position = new Vector2(PanelContainer.Position.X, 0);
            PanelContainer.SortChildren += animateEnter;
        }));

        void animateEnter()
        {
            PanelContainer.SortChildren -= animateEnter;

            Transition(reEntryTime, true);
        }
    }
}
