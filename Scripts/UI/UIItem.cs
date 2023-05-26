namespace Inventory;

public class UIItem
{
    public Vector2 Position
    {
        get => parent.Position;
        set => parent.Position = value;
    }

    readonly Control parent;
    readonly Node2D sprite;
    readonly Label label;

    public UIItem(Node parent, Item item)
    {
        this.parent = new Control();

        // Need 'centered' bool because cursor is offset by (InvSlotSize / 2) on both axis
        var centered = parent.Name != "ParentCursor";

        // Create sprite
        sprite = item.Type.GenerateGraphic();
        sprite.Position = centered ? Vector2.One * (50 / 2) : Vector2.Zero;
        sprite.Scale = Vector2.One * 2;
        this.parent.AddChild(sprite);

        // Create count label
        var marginContainer = new GMarginContainer
        {
            CustomMinimumSize = Vector2.One * 50,
            MouseFilter = Control.MouseFilterEnum.Ignore,
            Position = centered ? Vector2.Zero : Vector2.One * -25,
            Visible = item.Count != 1
        };
        marginContainer.SetMarginLeft(3);

        label = new GLabel(item.Count + "", 20)
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Bottom,
            SizeFlagsVertical = Control.SizeFlags.Fill,
            MouseFilter = Control.MouseFilterEnum.Ignore // ignored by default but just in case Godot changes it in the future
        };
        label.AddThemeColorOverride("font_shadow_color", Colors.Black);
        label.AddThemeConstantOverride("shadow_outline_size", 3);

        marginContainer.AddChild(label);
        this.parent.AddChild(marginContainer);
        parent.AddChild(this.parent);
    }

    public void SetText(string text) => label.Text = text;

    public void Hide()
    {
        // not sure why its not valid all the time
        if (GodotObject.IsInstanceValid(label))
            label.Hide();
        
        sprite.Hide();
    }

    public void Show()
    {
        // not sure why its not valid all the time
        if (GodotObject.IsInstanceValid(label))
            label.Show();
        sprite.Show();
    }
}
