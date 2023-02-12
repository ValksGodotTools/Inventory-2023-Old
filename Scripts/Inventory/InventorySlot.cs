namespace Inventory;

public class InventorySlot : IItemHolder
{
	public InventoryItem InventoryItem { get; set; }
	public Item Item { get => InventoryItem.Item; set => InventoryItem.Item = value; }

	private Vector2 Position { get => Panel.GlobalPosition + Vector2.One * (Inventory.SlotSize / 2); }
	private bool CurrentlyAnimating { get; set; }
	private Node2D Graphic { get; set; }
	private Tween Tween { get; set; }
	private InventorySlot OtherInventorySlot { get; set; }

	private Label ItemCountLabel { get; set; }
	private Label DebugLabel { get; set; }

	private Panel Panel { get; set; }
	private Inventory Inventory { get; set; }
	private bool JustPickedUpItem { get; set; }

	public InventorySlot(Inventory inv, Node parent)
	{
		Inventory = inv;

		Panel = new Panel
		{
			CustomMinimumSize = Vector2.One * Inventory.SlotSize
		};

		Panel.GuiInput += (inputEvent) =>
		{
			if (inputEvent is not InputEventMouseButton eventMouseButton)
				return;

			if (eventMouseButton.IsLeftClickPressed())
				HandleLeftClick();

			if (eventMouseButton.IsRightClickPressed())
				HandleRightClick();
		};

		Panel.MouseEntered += () =>
		{
			Inventory.ActiveInventorySlot = this;

			var cursorItem = Main.ItemCursor.GetItem();

			ContinuousLeftClickPickup(cursorItem);
			ContinuousRightClickPlace(cursorItem);
			ContinuousShiftClickTransfer();

			if (InventoryItem != null)
				ItemPanelDescription.Display(InventoryItem.Item);
		};

		Panel.MouseExited += () =>
		{
			Inventory.ActiveInventorySlot = null;

			if (InventoryItem == null)
				return;

			ItemPanelDescription.Clear();
		};

		parent.AddChild(Panel);

		ItemCountLabel = UtilsLabel.CreateItemCountLabel();
		Panel.AddChild(ItemCountLabel);

		DebugLabel = new Label
		{
			Text = "",
			ZIndex = 100,
			CustomMinimumSize = Vector2.One * Inventory.SlotSize,
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center,
		};

		DebugLabel.AddThemeColorOverride("font_shadow_color", Colors.Black);

		Panel.AddChild(DebugLabel);
	}

	public void ResetCleanUpAnimations()
	{
		if (!CurrentlyAnimating)
			return;

		Tween.Kill();

		if (OtherInventorySlot != null)
		{
			OtherInventorySlot.CurrentlyAnimating = false;
			OtherInventorySlot.InventoryItem?.Show();
		}

		CurrentlyAnimating = false;

		if (GodotObject.IsInstanceValid(Graphic))
			Graphic.QueueFree();
	}

	public void SetVisible(bool v) => Panel.Visible = v;

	private void ContinuousLeftClickPickup(Item cursorItem)
	{
		// Pickup items of the same type
		if (!InputGame.HoldingLeftClick || cursorItem == null || InventoryItem == null)
			return;

		if (cursorItem.Type != InventoryItem.Item.Type)
			return;

		var item = InventoryItem.Item;
		item.Count += cursorItem.Count;

		Main.ItemCursor.SetItem(item);

		RemoveItem();//
	}

	private void ContinuousShiftClickTransfer()
	{
		if (!InputGame.HoldingLeftClick || !Input.IsKeyPressed(Key.Shift))
			return;

		TransferItem();
	}

	private void ContinuousRightClickPlace(Item cursorItem)
	{
		if (!InputGame.HoldingRightClick || cursorItem == null)
			return;

		if (InventoryItem == null)
		{
			// Take 1 item from the cursor
			Main.ItemCursor.TakeItem();

			// Pass 1 item from cursor to the inventory slot
			var item = cursorItem.Clone();
			item.Count = 1;
			SetItem(item);
		}
		else
		{
			if (cursorItem.Type != InventoryItem.Item.Type)
				return;

			// Take 1 item from the cursor
			Main.ItemCursor.TakeItem();

			// Pass 1 item from cursor to the inventory slot
			var item = cursorItem.Clone();
			item.Count = 1 + InventoryItem.Item.Count;
			SetItem(item);
		}
	}

	public void SetDebugLabel(string text)
	{
		DebugLabel.Text = text;
	}

	public void SwapItem(IItemHolder to)
	{
		// Destination item exists and Item and to.Item are of different types
		// If this is the case lets swap
		if (to.Item != null && to.Item.Type != Item.Type)
		{
			// Remember Item as item before removing it
			var item = Item;

			RemoveItem();

			SetItem(to.Item);
			to.SetItem(item);
		}
	}

	public void SetItem(Item item)
	{
		UpdateItemCountLabel(item.Count);
		InventoryItem?.QueueFreeGraphic();
		InventoryItem = item.Type.ToInventoryItem(Inventory, Panel, item);
	}

	public void RemoveItem()
	{
		// Clear the item graphic for this inventory slot
		InventoryItem.QueueFreeGraphic();
		InventoryItem = null;

		// Clear the item count graphic
		UpdateItemCountLabel(0);
	}

	public Inventory GetOtherInventory() => this.Inventory == Player.Inventory ?
		Inventory.OtherInventory : Player.Inventory;

	private void UpdateItemCountLabel(int count)
	{
		if (count > 1)
			ItemCountLabel.Text = count + "";
		else
			ItemCountLabel.Text = "";
	}

	private void CollectAndMergeAllItemsFrom(Item itemToMergeTo)
	{
		var otherItemCounts = 0;

		// Scan the inventory for items of the same type and combine them to the cursor
		foreach (var slot in Inventory.InventorySlots)
		{
			// Skip the slot we double clicked on
			if (slot == this)
				continue;

			var invItem = slot.InventoryItem;

			// A item exists in this inv slot
			if (invItem != null)
			{
				// The inv slot item is the same type as the cursor item type
				if (invItem.Item.Type == itemToMergeTo.Type)
				{
					otherItemCounts += invItem.Item.Count;

					slot.RemoveItem();
				}
			}
		}

		var counts = itemToMergeTo.Count + otherItemCounts;
		itemToMergeTo.Count = counts;
		Main.ItemCursor.SetItem(itemToMergeTo);
	}

	// Transfer the item in the inventory slot we are currently hovering over
	private void TransferItem()
	{
		// Do not transfer item if this item is currently being animated
		if (CurrentlyAnimating)
			return;

		// There is no item here thus no item to transfer
		if (InventoryItem == null)
			return;

		// Get the 'other inventory'
		var targetInv = GetOtherInventory();

		// No 'other inventory' is open, lets use the player inventory so the
		// item gets transfered to the same inventory instead of doing nothing
		if (targetInv == null)
			targetInv = Player.Inventory;

		// Try to find a empty slot in the target inventory
		var emptySlot = targetInv.TryGetEmptyOrSameTypeSlot(InventoryItem.Item.Type);

		// No empty slot was found!
		if (emptySlot == -1)
			return;

		// Store temporary reference to the item in this inventory slot
		var itemRef = InventoryItem.Item;

		// Get a copy of the item sprite that is being transfered
		// This is purely for visuals and does not effect the item logic
		Graphic = InventoryItem.GenerateGraphic();
		Graphic.GlobalPosition = Position;

		// Get the other slot this item is being transfered to
		var otherInvSlot = targetInv.InventorySlots[emptySlot];

		//if (otherInvSlot.CurrentlyAnimating)
		//	return;

		// Remove the item before it gets transfered
		this.RemoveItem();

		var otherInvSlotItem = otherInvSlot.InventoryItem;

		var hide = false;

		// If the other inventory slot has no item in it, then hide the item that
		// gets transfered over. So it does not look like there is a duplicate
		// when the graphic sprite is animated over
		if (otherInvSlot.InventoryItem == null)
			hide = true;

		// Set the other inventory slot item to the item that is being transfered over
		if (otherInvSlotItem == null)
		{
			otherInvSlot.SetItem(itemRef);
		}
		else
		// If the item transfered has more than one than add that
		{
			itemRef.Count += otherInvSlotItem.Item.Count;
			otherInvSlot.SetItem(itemRef);
		}

		OtherInventorySlot = otherInvSlot;

		// Hide the other item
		if (hide)
			otherInvSlot.InventoryItem.Hide();

		// Start animation
		CurrentlyAnimating = true;
		otherInvSlot.CurrentlyAnimating = true;

		// Add graphic to the world
		Main.AddToCanvasLayer(Graphic);

		Tween = Panel.GetTree().CreateTween();
		Tween.TweenProperty(Graphic, "global_position", otherInvSlot.Position, 1)
			.SetEase(Tween.EaseType.Out)
			.SetTrans(Tween.TransitionType.Cubic);

		Tween.TweenCallback(Callable.From(() =>
		{
			// Sprite graphic reached its destination, lets show the other inventory slot item now
			
			// For the love of god I have no fucking idea why I need a
			// null check here. I spent 2 hours trying to figure out
			// why InventoryItem becomes null. And with the null check
			// earlier it was just hiding items. But now it all magically
			// works all of a sudden wtf?
			// Please just stay working okay?
			// Thank you code
			// I love you code
			otherInvSlot.InventoryItem?.Show();

			CurrentlyAnimating = false;
			otherInvSlot.CurrentlyAnimating = false;
			Graphic.QueueFree();
		}));
	}

	private void HandleLeftClick()
	{
		var cursorItem = Main.ItemCursor.GetItem();

		// If were double clicking and not holding shift and
		// we are holding a item in our cursor [and the inventory
		// slot we are hovering over has no item or there is
		// a item in this inventory slot and it is of the
		// same item type as the item type in our cursor] then
		// collect all items to the cursor and return preventing
		// any other mouse inventory logic from executing.
		if (InputGame.DoubleClick && !Input.IsKeyPressed(Key.Shift))
		{
			if (cursorItem != null)
			{
				if (InventoryItem == null || InventoryItem.Item.Type == cursorItem.Type)
				{
					CollectAndMergeAllItemsFrom(cursorItem);

					// We collected all items to the cursor. Do not let anything else happen.
					return;
				}
			}
			// There is an item in the cursor and this is a double click
			else
			{
				if (InventoryItem != null)
				{
					CollectAndMergeAllItemsFrom(InventoryItem.Item);

					RemoveItem(); // prevent dupe glitch

					// We collected all items to the cursor. Do not let anything else happen.
					return;
				}
			}
		}

		// There is no item in this inventory slot
		if (InventoryItem == null)
		{
			if (!JustPickedUpItem)
			{
				// Is there a item attached to the cursor?
				if (cursorItem != null)
				{
					// Remove the item from the cursor
					Main.ItemCursor.RemoveItem();

					// Set the item in this inventory slot to the item from the cursor
					SetItem(cursorItem);
				}
			}
		}
		// There is a item in this inventory slot
		else
		{
			// Recap
			// 1. Left click
			// 2. There is an item in the inventory slot

			JustPickedUpItem = true;
			Panel.GetTree().CreateTimer(InputGame.DoubleClickTime / 1000.0).Timeout += () => 
				JustPickedUpItem = false;

			// There is an item attached to the cursor
			if (cursorItem != null)
			{
				// The cursor and inv slot items are of the same type
				if (cursorItem.Type == InventoryItem.Item.Type)
				{
					var item = cursorItem.Clone();
					item.Count += InventoryItem.Item.Count;

					Main.ItemCursor.RemoveItem();

					SetItem(item);
				}
				// The cursor and inv slot items are of different types
				else
				{
					// Recap:
					// 1. Left Click
					// 2. There is an item in the inventory slot
					// 3. There is a item in the cursor
					// 4. The cursor item and inv slot item are of different types
					// 
					// So lets swap the cursor item with the inv slot item
					SwapItem(Main.ItemCursor);
				}
			}
			// There is no item attached to the cursor
			else
			{
				// Shift + Click
				if (Input.IsKeyPressed(Key.Shift))
				{
					TransferItem();

					return;
				}

				var item = InventoryItem.Item;

				this.RemoveItem();

				// Set the item in this inventory slot to the item from the cursor
				Main.ItemCursor.SetItem(item);
			}
		}
	}

	private void HandleRightClick()
	{
		var cursorItem = Main.ItemCursor.GetItem();

		// Is there a item attached to the cursor?
		if (cursorItem != null)
		{
			// There is no item in this inventory slot
			if (InventoryItem == null)
			{
				// Take 1 item from the cursor
				Main.ItemCursor.TakeItem();

				// Pass 1 item from cursor to the inventory slot
				var item = cursorItem.Clone();
				item.Count = 1;
				SetItem(item);
			}
			// There is a item in this inventory slot
			else
			{
				// Is the cursor item being held of the same type for the item in the inventory slot?
				if (cursorItem.Type == InventoryItem.Item.Type)
				{
					Main.ItemCursor.TakeItem();

					var item = cursorItem.Clone();

					// Take 1 item from cursor and the item count from the inv slot and combine them
					item.Count = 1 + InventoryItem.Item.Count;

					// Set all these items to the inv slot
					SetItem(item);
				}
			}
		}
		// There is no item being held in the cursor
		else
		{
			// There is a item in this inventory slot
			if (InventoryItem != null)
			{
				// Shift + Right Click = Split Stack
				if (Input.IsKeyPressed(Key.Shift))
				{
					var itemA = InventoryItem.Item;

					// Prevent duping 1 item to 2 items
					if (itemA.Count == 1)
						return;

					var itemB = itemA.Clone();
					
					var half = itemA.Count / 2;
					var remainder = itemA.Count % 2;

					itemA.Count = half + remainder;
					itemB.Count = half;

					SetItem(itemA);
					Main.ItemCursor.SetItem(itemB);

					return;
				}

				var invSlotItemCount = InventoryItem.Item.Count;

				// Is this the last item in the stack?
				if (invSlotItemCount - 1 == 0)
				{
					Main.ItemCursor.SetItem(InventoryItem.Item);

					RemoveItem();
				}
				// There are two or more items in this inv slot
				else
				{
					// Recap:
					// 1. User did a right click
					// 2. There is no item being held in the cursor
					// 3. There are two or more items in this inv slot

					// Lets take 1 item from the inv slot and bring it to the cursor
					InventoryItem.Item.Count -= 1;

					UpdateItemCountLabel(InventoryItem.Item.Count);
					
					var item = InventoryItem.Item.Clone();
					item.Count = 1;
					Main.ItemCursor.SetItem(item);
				}
			}
		}
	}
}
