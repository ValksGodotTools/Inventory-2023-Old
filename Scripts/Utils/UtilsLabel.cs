namespace Inventory;

public static class UtilsLabel
{
	public static Label CreateItemCountLabel()
	{
		var label = new Label
		{
			HorizontalAlignment = HorizontalAlignment.Left,
			VerticalAlignment = VerticalAlignment.Bottom,
			CustomMinimumSize = new Vector2(50, 50),
			ZIndex = 1, // ensure label is rendered on top of item
			Position = new Vector2(4, 0) // push 4 pixels from left to right
		};
		label.AddThemeColorOverride("font_shadow_color", Colors.Black);
		label.AddThemeConstantOverride("shadow_outline_size", 5);
		label.AddThemeFontSizeOverride("font_size", 16);
		return label;
	}
}
