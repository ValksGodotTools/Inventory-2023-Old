using static Godot.Control;

namespace Inventory;

public class UIItemDetails
{
    public Control Parent { get; set; }
    private Label LabelName { get; set; }
    private Label LabelCategory { get; set; }
    private Label LabelDescription { get; set; }
    public GridContainer GridContainer { get; set; }
    protected PanelContainer PanelContainer { get; set; }
    protected LayoutPreset LayoutPreset { get; set; }

    private Control ControlPivot { get; set; }
    private StyleBox PanelStyleBoxVisible { get; set; }

    public UIItemDetails(Node parent)
    {
        LayoutPreset = LayoutPreset.TopLeft;

        CreateUI(parent);

        // Create labels
        LabelName = DefaultLabel();

        LabelCategory = DefaultLabel();
        LabelCategory.AddThemeFontSizeOverride("font_size", 16);

        LabelDescription = DefaultLabel();
        LabelDescription.AddThemeFontSizeOverride("font_size", 12);

        GridContainer.AddChild(LabelName);
        GridContainer.AddChild(LabelCategory);
        GridContainer.AddChild(LabelDescription);
    }

    public void Clear()
    {
        LabelName.Text = string.Empty;
        LabelCategory.Text = string.Empty;
        LabelDescription.Text = string.Empty;
    }

    public void ChangeItem(Item item)
    {
        if (item != null)
        {
            LabelName.Text = item.Type.Name;
            LabelCategory.Text = item.Type.ItemCategory.ToString();
            LabelDescription.Text = item.Type.Description;
        }
    }

    public void SetAnchor(LayoutPreset preset)
    {
        LayoutPreset = preset;
        PanelContainer.SetAnchorsAndOffsetsPreset(preset);
        ControlPivot.SetAnchorsAndOffsetsPreset(preset);
    }
    private void CreateUI(Node parent)
    {
        // Setup inventory UI
        ControlPivot = new Control();
        PanelContainer = new PanelContainer();
        GridContainer = new GridContainer();

        PanelStyleBoxVisible = PanelContainer.GetThemeStylebox("panel");

        var marginContainer = new MarginContainer();
        marginContainer.AddMargin(10);

        GridContainer.Columns = 1;
        
        PanelContainer.AddChild(marginContainer);
        marginContainer.AddChild(GridContainer);
        ControlPivot.AddChild(PanelContainer);
        parent.AddChild(ControlPivot);
    }

    public static Label DefaultLabel()
    {
        Label label = new Label
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Bottom,
            SizeFlagsVertical = Control.SizeFlags.Fill,
            MouseFilter = Control.MouseFilterEnum.Ignore // ignored by default but just in case Godot changes it in the future
        };

        label.AddThemeColorOverride("font_shadow_color", Colors.Black);
        label.AddThemeConstantOverride("shadow_outline_size", 3);
        label.AddThemeFontSizeOverride("font_size", 20);

        return label;
    }
}
