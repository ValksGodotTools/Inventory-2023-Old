using static Godot.Control;

namespace Inventory;

public partial class UIInventory
{
    public bool Visible
    {
        get => ControlPivot.Visible;
        set => ControlPivot.Visible = value;
    }

    public Container Container { get; }
    public int SlotSize { get; } = 50;
    public GridContainer GridContainer { get; set; }
    public UIInventorySlot[] UIInventorySlots { get; set; }
    public int Size { get; }
    public int Columns { get; }
    public ItemCategory? ItemCategoryFilter { get; set; }
    protected Tween Tween { get; set; }
    protected PanelContainer PanelContainer { get; set; }
    protected bool Animating { get; set; }
    protected LayoutPreset LayoutPreset { get; set; }

    private Control ControlPivot { get; set; }
    private StyleBox PanelStyleBoxVisible { get; set; }

    public UIInventory(Node parent, int size, int columns, ItemCategory? itemCategoryFilter = null)
    {
        LayoutPreset = LayoutPreset.CenterTop;
        Size = size;
        Columns = columns;
        Container = new Container(size);
        ItemCategoryFilter = itemCategoryFilter;
        CreateUI(parent, columns, itemCategoryFilter);
    }

    public void Update() => UIInventorySlots.ForEach(x => x.Update());

    public void SetItem(int i, Item item) => UIInventorySlots[i].Set(item);
    public void SetItem(int x, int y, Item item) => UIInventorySlots[x + y * Columns].Set(item);

    public void SetAnchor(LayoutPreset preset)
    {
        LayoutPreset = preset;
        PanelContainer.SetAnchorsAndOffsetsPreset(preset);
        ControlPivot.SetAnchorsAndOffsetsPreset(preset);
    }

    public Vector2 GetSlotPosition(int i) => UIInventorySlots[i].Parent.GlobalPosition + Vector2.One * (SlotSize / 2);

    public void HideBackPanel() =>
        PanelContainer.AddThemeStyleboxOverride("panel", new StyleBoxEmpty());

    public void ShowBackPanel() =>
        PanelContainer.AddThemeStyleboxOverride("panel", PanelStyleBoxVisible);

    public void Hide() => ControlPivot.Hide();
    public void Show() => ControlPivot.Show();
    public void SetSlotsVisibility(int a, int b, bool visible, bool anchor)
    {
        for (int i = a; i < b; i++)
            UIInventorySlots[i].Visible = visible;

        if (anchor)
            SetAnchor(LayoutPreset);
    }

    public void AnimateHide(double duration = 1) => Transition(duration, false, false);
    public void AnimateShow(double duration = 1) => Transition(duration, true, false);

    protected void Transition(double duration = 1, bool entering = false, bool extended = false)
    {
        Animating = true;

        Tween = PanelContainer.GetTree().CreateTween();

        var finalValue = LayoutPreset == LayoutPreset.CenterTop ?
            -PanelContainer.Size.Y : PanelContainer.Size.Y;

        if (entering)
            finalValue *= -1;

        Tween.TweenProperty(PanelContainer, "position:y", finalValue, duration)
            .SetTrans(Tween.TransitionType.Cubic)
            .SetEase(Tween.EaseType.InOut);

        if (!extended)
            Tween.TweenCallback(Callable.From(() => Animating = false));
    }

    private void CreateUI(Node parent, int columns, ItemCategory? itemCategoryFilter)
    {
        // Setup inventory UI
        ControlPivot = new Control();
        PanelContainer = new PanelContainer();
        var marginContainer = new GMarginContainer(5);
        GridContainer = new GridContainer();

        PanelStyleBoxVisible = PanelContainer.GetThemeStylebox("panel");
        GridContainer.Columns = columns;

        PanelContainer.AddChild(marginContainer);
        marginContainer.AddChild(GridContainer);
        ControlPivot.AddChild(PanelContainer);
        parent.AddChild(ControlPivot);

        // Setup inventory slots
        UIInventorySlots = new UIInventorySlot[Container.Items.Length];

        for (int i = 0; i < Container.Items.Length; i++)
            UIInventorySlots[i] = new UIInventorySlot(this, i, itemCategoryFilter);
    }
}
