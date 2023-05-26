﻿using static Godot.Control;

namespace Inventory;

public class UIItemDetails
{
    GLabel labelName;
    GLabel labelCategory;
    GLabel labelDescription;

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
        labelCategory.SetFontSize(16);

        labelDescription = DefaultLabel();
        labelDescription.SetFontSize(12);

        gridContainer.AddChild(labelName);
        gridContainer.AddChild(labelCategory);
        gridContainer.AddChild(labelDescription);
    }

    static GLabel DefaultLabel()
    {
        var label = new GLabel
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Bottom,
            SizeFlagsVertical = SizeFlags.Fill,
            MouseFilter = MouseFilterEnum.Ignore // ignored by default but just in case Godot changes it in the future
        };

        label.AddThemeColorOverride("font_shadow_color", Colors.Black);
        label.AddThemeConstantOverride("shadow_outline_size", 3);
        label.SetFontSize(20);

        return label;
    }
}