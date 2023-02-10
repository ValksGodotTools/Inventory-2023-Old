using Godot;

namespace Inventory;

public class PlayerInventory : Inventory
{
	public bool CurrentlyAnimating { get; set; }
	public bool IsHotbar { get; set; } = true;

	private Node Node { get; set; }
	private Tween TweenExit { get; set; }
	private Tween TweenReEntry { get; set; }

	public PlayerInventory(Node node, int columns = 9, int rows = 5, int slotSize = 50) 
		: base(node, columns, rows, slotSize)
	{
		Node = node;

		SetAnchor(Control.LayoutPreset.CenterBottom);
		SwitchToHotbar();
		Show();
	}

	// Instantly switch
	public void SwitchToHotbar()
	{
		// ensure animation tweens are dead
		TweenExit?.Kill();
		TweenReEntry?.Kill();

		IsHotbar = true;
		CurrentlyAnimating = false;

		MakePanelInvisible();
		SetSlotsVisibility(0, InventorySlots.Length - Columns, false, true);
	}

	// Instantly switch
	public void SwitchToFullInventory()
	{
		// ensure animation tweens are dead
		TweenExit?.Kill();
		TweenReEntry?.Kill();

		IsHotbar = false;
		CurrentlyAnimating = false;

		MakePanelVisible();
		SetSlotsVisibility(0, InventorySlots.Length - Columns, true, true);
	}

	public void SwitchToHotbarAnimated()
	{
		Transition(true, () =>
		{
			MakePanelInvisible();
			SetSlotsVisibility(0, InventorySlots.Length - Columns, false, true);
		}, 1, 1, Tween.TransitionType.Back);
	}

	public void SwitchToFullInventoryAnimated()
	{
		Transition(false, () =>
		{
			MakePanelVisible();
			SetSlotsVisibility(0, InventorySlots.Length - Columns, true, true);
		}, 1, 0.5, Tween.TransitionType.Sine);
	}

	private void Transition(bool isHotbar, Action action, double exitTime, double reEntryTime,
		Tween.TransitionType reEntryTransType)
	{
		if (CurrentlyAnimating)
			return;

		IsHotbar = isHotbar;
		CurrentlyAnimating = true;

		TweenExit = Node.GetTree().CreateTween();
		var container = PanelContainer;

		// Move the hotbar out of the game view by moving down by its own height
		TweenExit.TweenProperty(container, "position:y", container.Size.Y, exitTime)
			.SetTrans(Tween.TransitionType.Cubic)
			.SetEase(Tween.EaseType.InOut);

		TweenExit.TweenCallback(Callable.From(() =>
		{
			// Transform the hotbar into a full inventory
			action();

			// Reset the hotbar y position
			// Make sure to reset the y position after SetSlotsVisibility(...) because
			// SetSlotsVisibility makes a call to SetAnchorsAndOffsetsPreset(...) which
			// sets the position to a unwanted y value
			container.Position = new Vector2(container.Position.X, 0);
		}));

		// Since the hotbar was transformed to a full inventory, its container size has changed
		// But container.Size.Y will not be updated until SortChildren has fired
		// So lets subscribe to SortChildren
		container.SortChildren += animateUp;

		void animateUp()
		{
			// SortChildren is called multiple times and we only need it for one time
			container.SortChildren -= animateUp;

			// The previous tween is no longer any good, so lets make another
			TweenReEntry = Node.GetTree().CreateTween();

			// Move the full inventory into the games view by moving up by its own height
			TweenReEntry.TweenProperty(container, "position:y", -PanelContainer.Size.Y, reEntryTime)
				.SetTrans(reEntryTransType)
				.SetEase(Tween.EaseType.Out);

			TweenReEntry.TweenCallback(Callable.From(() => CurrentlyAnimating = false));
		}
	}
}
