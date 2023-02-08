namespace Inventory;

public static class InputGame
{
	public static ulong DoubleClickTime  { get; } = 250;

	public static bool HoldingLeftClick  { get; private set; }
	public static bool HoldingRightClick { get; private set; }
	public static bool DoubleClick       { get; private set; }

	private static ulong LastClick { get; set; }

	public static void Handle(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventMouseButton)
			HandleMouseButton(eventMouseButton);
	}

	private static void HandleMouseButton(InputEventMouseButton @event)
	{
		if (@event.IsLeftClickPressed())
		{
			HoldingLeftClick = true;
			DoubleClick = Time.GetTicksMsec() - LastClick <= DoubleClickTime;
			LastClick = Time.GetTicksMsec();
		}

		if (@event.IsLeftClickReleased())
			HoldingLeftClick = false;

		if (@event.IsRightClickPressed())
			HoldingRightClick = true;

		if (@event.IsRightClickReleased())
			HoldingRightClick = false;
	}
}
