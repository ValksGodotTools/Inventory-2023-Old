using static Godot.Control;

namespace Inventory;

public partial class UIInventory
{
    public bool Visible
    {
        get => controlPivot.Visible;
        set => controlPivot.Visible = value;
    }

    public Container Container { get; }
    public int SlotSize { get; } = 50;
    public GridContainer GridContainer { get; set; }
    public UIInventorySlot[] UIInventorySlots { get; set; }
    public int Size { get; }
    public int Columns { get; }
    public ItemCategory? ItemCategoryFilter { get; set; }

    protected Tween tween;
    protected PanelContainer panelContainer;
    protected bool animating;
    protected LayoutPreset layoutPreset;

    private Control controlPivot;
    private StyleBox panelStyleBoxVisible;

    public UIInventory(Node parent, int size, int columns, ItemCategory? itemCategoryFilter = null)
    {
        layoutPreset = LayoutPreset.CenterTop;
        Size = size;
        Columns = columns;
        Container = new Container(size);
        ItemCategoryFilter = itemCategoryFilter;
        CreateUI(parent, columns, itemCategoryFilter);
    }

    public void Update() => UIInventorySlots.ForEach(x => x.Update());

    /// <summary>
    /// Refresh UISlots content based on Container's items
    /// </summary>
    public void Refresh()
    {
        for (var i = 0; i < UIInventorySlots.Count(); i++)
        {
            if (this.Container.Get(i) == null)
                UIInventorySlots[i].Remove();
            else
                UIInventorySlots[i].Set(this.Container.Get(i));
        }
    }

    public void SetItem(int i, Item item) => UIInventorySlots[i].Set(item);
    public void SetItem(int x, int y, Item item) => UIInventorySlots[x + y * Columns].Set(item);

    public void SetAnchor(LayoutPreset preset)
    {
        layoutPreset = preset;
        panelContainer.SetAnchorsAndOffsetsPreset(preset);
        controlPivot.SetAnchorsAndOffsetsPreset(preset);
    }

    public Vector2 GetSlotPosition(int i) => UIInventorySlots[i].Parent.GlobalPosition + Vector2.One * (SlotSize / 2);

    public void HideBackPanel() =>
        panelContainer.AddThemeStyleboxOverride("panel", new StyleBoxEmpty());

    public void ShowBackPanel() =>
        panelContainer.AddThemeStyleboxOverride("panel", panelStyleBoxVisible);

    public void Hide() => controlPivot.Hide();
    public void Show() => controlPivot.Show();
    public void SetSlotsVisibility(int a, int b, bool visible, bool anchor)
    {
        for (int i = a; i < b; i++)
            UIInventorySlots[i].Visible = visible;

        if (anchor)
            SetAnchor(layoutPreset);
    }

    public void AnimateHide(double duration = 1) => Transition(duration, false, false);
    public void AnimateShow(double duration = 1) => Transition(duration, true, false);

    protected void Transition(double duration = 1, bool entering = false, bool extended = false)
    {
        animating = true;

        tween = panelContainer.GetTree().CreateTween();

        var finalValue = layoutPreset == LayoutPreset.CenterTop ?
            -panelContainer.Size.Y : panelContainer.Size.Y;

        if (entering)
            finalValue *= -1;

        tween.TweenProperty(panelContainer, "position:y", finalValue, duration)
            .SetTrans(Tween.TransitionType.Cubic)
            .SetEase(Tween.EaseType.InOut);

        if (!extended)
            tween.TweenCallback(Callable.From(() => animating = false));
    }

    private void CreateUI(Node parent, int columns, ItemCategory? itemCategoryFilter)
    {
        // Setup inventory UI
        controlPivot = new Control();
        panelContainer = new PanelContainer();
        var marginContainer = new GMarginContainer(5);
        GridContainer = new GridContainer();

        panelStyleBoxVisible = panelContainer.GetThemeStylebox("panel");
        GridContainer.Columns = columns;

        panelContainer.AddChild(marginContainer);
        marginContainer.AddChild(GridContainer);
        controlPivot.AddChild(panelContainer);
        parent.AddChild(controlPivot);

        // Setup inventory slots
        UIInventorySlots = new UIInventorySlot[Container.Items.Length];

        for (int i = 0; i < Container.Items.Length; i++)
            UIInventorySlots[i] = new UIInventorySlot(this, i, itemCategoryFilter);
    }
}
