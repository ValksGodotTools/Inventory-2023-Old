namespace Inventory;

public class UIItem
{
    public Vector2 Position
    {
        get => Parent.Position;
        set => Parent.Position = value;
    }

    public Control Parent { get; set; }

    private Node2D Sprite { get; set; }
    private Label Label { get; set; }

    public UIItem(Node parent, Item item)
    {
        Parent = new Control();

        // Need 'centered' bool because cursor is offset by (InvSlotSize / 2) on both axis
        var centered = parent.Name != "ParentCursor";

        // Create sprite
        Sprite = item.Type.GenerateGraphic();
        Sprite.Position = centered ? Vector2.One * (50 / 2) : Vector2.Zero;
        Sprite.Scale = Vector2.One * 2;
        Parent.AddChild(Sprite);

        // Create count label
        var marginContainer = new MarginContainer
        {
            CustomMinimumSize = Vector2.One * 50,
            MouseFilter = Control.MouseFilterEnum.Ignore,
            Position = centered ? Vector2.Zero : Vector2.One * -25,
            Visible = item.Count != 1
        };
        marginContainer.AddThemeConstantOverride("margin_left", 3);

        Label = new Label
        {
            Text = item.Count + "",
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Bottom,
            SizeFlagsVertical = Control.SizeFlags.Fill,
            MouseFilter = Control.MouseFilterEnum.Ignore // ignored by default but just in case Godot changes it in the future
        };
        Label.AddThemeColorOverride("font_shadow_color", Colors.Black);
        Label.AddThemeConstantOverride("shadow_outline_size", 3);
        Label.AddThemeFontSizeOverride("font_size", 20);

        marginContainer.AddChild(Label);
        Parent.AddChild(marginContainer);
        parent.AddChild(Parent);
    }

    public void SetText(string text) => Label.Text = text;

    public void Hide()
    {
        // not sure why its not valid all the time
        if (GodotObject.IsInstanceValid(Label))
            Label.Hide();
        
        Sprite.Hide();
    }

    public void Show()
    {
        // not sure why its not valid all the time
        if (GodotObject.IsInstanceValid(Label))
            Label.Show();
        Sprite.Show();
    }
}
