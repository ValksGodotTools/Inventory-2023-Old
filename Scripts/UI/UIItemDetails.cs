using static Godot.Control;

namespace Inventory;

public class UIItemDetails
{
    Label labelName;
    Label labelCategory;
    Label labelDescription;

    PanelContainer panelContainer;
    Control controlPivot;

    public UIItemDetails(Node parent)
    {
        CreateUI(parent);
    }

    public void Clear()
    {
        labelName.Text = string.Empty;
        labelCategory.Text = string.Empty;
        labelDescription.Text = string.Empty;
    }

    public void ChangeItem(Item item)
    {
        if (item != null)
        {
            labelName.Text = item.Type.Name;
            labelCategory.Text = item.Type.ItemCategory.ToString();
            labelDescription.Text = item.Type.Description;
        }
    }

    public void SetAnchor(LayoutPreset preset)
    {
        panelContainer.SetAnchorsAndOffsetsPreset(preset);
        controlPivot.SetAnchorsAndOffsetsPreset(preset);
    }

    void CreateUI(Node parent)
    {
        // Setup inventory UI
        controlPivot = new Control();
        panelContainer = new PanelContainer();
        var gridContainer = new GridContainer();

        var marginContainer = new GMarginContainer(10);

        gridContainer.Columns = 1;
        
        panelContainer.AddChild(marginContainer);
        marginContainer.AddChild(gridContainer);
        controlPivot.AddChild(panelContainer);
        parent.AddChild(controlPivot);

        // Create labels
        labelName = DefaultLabel();

        labelCategory = DefaultLabel();
        labelCategory.AddThemeFontSizeOverride("font_size", 16);

        labelDescription = DefaultLabel();
        labelDescription.AddThemeFontSizeOverride("font_size", 12);

        gridContainer.AddChild(labelName);
        gridContainer.AddChild(labelCategory);
        gridContainer.AddChild(labelDescription);
    }

    static Label DefaultLabel()
    {
        var label = new Label
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Bottom,
            SizeFlagsVertical = SizeFlags.Fill,
            MouseFilter = MouseFilterEnum.Ignore // ignored by default but just in case Godot changes it in the future
        };

        label.AddThemeColorOverride("font_shadow_color", Colors.Black);
        label.AddThemeConstantOverride("shadow_outline_size", 3);
        label.AddThemeFontSizeOverride("font_size", 20);

        return label;
    }
}
