using Godot;

namespace Inventory;

public class PlayerInventory : Inventory
{
	public bool CurrentlyAnimating { get; set; }
	public bool IsHotbar { get; set; } = true;

	private Node Node { get; set; }

	public PlayerInventory(Node node, int columns = 9, int rows = 5, int slotSize = 50) 
		: base(node, columns, rows, slotSize)
	{
		Node = node;

		SetAnchor(Control.LayoutPreset.CenterBottom);
		SwitchToHotbar(true);
		Show();
	}

	public void SwitchToHotbarAnimated()
	{
		if (CurrentlyAnimating)
			return;

		IsHotbar = true;
		CurrentlyAnimating = true;

		var tween = Node.GetTree().CreateTween();
		var container = PanelContainer;

		// Move the full inv out of the game view by moving down by its own height
		tween.TweenProperty(container, "position:y", container.Size.Y, 0.3);
		tween.TweenCallback(Callable.From(() =>
		{

			// Transform the full inv into a hotbar
			MakePanelInvisible();
			SetSlotsVisibility(0, InventorySlots.Length - Columns, false, true);
			// Reset the full inv y position
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
			var tween = Node.GetTree().CreateTween();

			// Move the full inventory into the games view by moving up by its own height
			tween.TweenProperty(container, "position:y", -PanelContainer.Size.Y, 0.3);
			tween.TweenCallback(Callable.From(() => CurrentlyAnimating = false));
		}
	}

	public void SwitchToFullInventoryAnimated()
	{
		if (CurrentlyAnimating)
			return;

		IsHotbar = false;
		CurrentlyAnimating = true;

		var tween = Node.GetTree().CreateTween();
		var container = PanelContainer;

		// Move the hotbar out of the game view by moving down by its own height
		tween.TweenProperty(container, "position:y", container.Size.Y, 0.2);
		tween.TweenCallback(Callable.From(() =>
		{
			// Transform the hotbar into a full inventory
			MakePanelVisible();
			SetSlotsVisibility(0, InventorySlots.Length - Columns, true, true);

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
			var tween = Node.GetTree().CreateTween();
			
			// Move the full inventory into the games view by moving up by its own height
			tween.TweenProperty(container, "position:y", -PanelContainer.Size.Y, 0.3);
			tween.TweenCallback(Callable.From(() => CurrentlyAnimating = false));
		}
	}

	private void Animate(bool isHotbar, Action action)
	{

	}

	public void SwitchToHotbar(bool updateAnchor)
	{
		if (CurrentlyAnimating)
			return;

		//CurrentlyAnimating = true;
		IsHotbar = true;
		MakePanelInvisible();
		SetSlotsVisibility(0, InventorySlots.Length - Columns, false, updateAnchor);
	}
}
