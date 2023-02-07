namespace Inventory;

public static class UtilsLabel
{
	public static Label CreateItemCountLabel()
	{
		var label = new Label
		{
			HorizontalAlignment = HorizontalAlignment.Right,
			VerticalAlignment = VerticalAlignment.Bottom,
			CustomMinimumSize = new Vector2(48, 50) // 50 - 2 because push 2 pixels to left
		};
		label.AddThemeColorOverride("font_shadow_color", Colors.Black);
		label.AddThemeConstantOverride("shadow_outline_size", 5);
		label.AddThemeFontSizeOverride("font_size", 16);
		return label;
	}
}
