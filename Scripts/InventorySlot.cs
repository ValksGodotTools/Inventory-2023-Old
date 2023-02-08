using static System.Net.Mime.MediaTypeNames;

namespace Inventory;

public class InventorySlot
{
	public Label ItemCountLabel { get; set; }
	private Label DebugLabel { get; set; }

	private Panel Panel { get; set; }
	public InventoryItem InventoryItem { get; set; }
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
			if (inputEvent is not InputEventMouseButton inputMouseButtonEvent)
				return;

			if (inputMouseButtonEvent.IsLeftClickPressed())
			{
				HandleLeftClick();
			}

			if (inputMouseButtonEvent.IsRightClickPressed())
			{
				HandleRightClick();
			}
		};

		Panel.MouseEntered += () =>
		{
			if (InventoryItem == null)
			{
				var cursorItem = ItemCursor.GetItem();

				if (ItemCursor.HoldingRightClick && cursorItem != null)
				{
					// Take 1 item from the cursor
					ItemCursor.TakeItem();

					// Pass 1 item from cursor to the inventory slot
					var item = cursorItem.Clone();
					item.Count = 1;
					SetItem(item);
				}
			}
			else
			{
				ItemPanelDescription.Display(InventoryItem.Item);
			}
		};

		Panel.MouseExited += () =>
		{
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

	public void SetDebugLabel(string text)
	{
		DebugLabel.Text = text;
	}

	public void SetItem(Item item)
	{
		UpdateItemCountLabel(item.Count);
		InventoryItem?.QueueFreeGraphic();
		InventoryItem = item.Type.ToInventoryItem(Inventory, Panel, item);
	}

	private void UpdateItemCountLabel(int count)
	{
		if (count > 1)
			ItemCountLabel.Text = count + "";
		else
			ItemCountLabel.Text = "";
	}
	
	private void HandleLeftClick()
	{
		var cursorItem = ItemCursor.GetItem();

		// Double click
		if (ItemCursor.DoubleClick)
		{
			var itemCursor = ItemCursor.GetItem();

			// Double clicked with a item in the cursor
			if (itemCursor != null)
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
						if (invItem.Item.Type == itemCursor.Type)
						{
							otherItemCounts += invItem.Item.Count;

							// Clear the item graphic for this inventory slot
							invItem.QueueFreeGraphic();
							slot.InventoryItem = null;

							// Clear the item count graphic
							slot.ItemCountLabel.Text = "";
						}
					}
				}

				var counts = itemCursor.Count + otherItemCounts;
				itemCursor.Count = counts;
				ItemCursor.SetItem(itemCursor);
			}

			return;
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
					ItemCursor.ClearItem();

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
			Panel.GetTree().CreateTimer(ItemCursor.DoubleClickTime / 1000.0).Timeout += () => 
				JustPickedUpItem = false;

			// There is an item attached to the cursor
			if (cursorItem != null)
			{
				// The cursor and inv slot items are of the same type
				if (cursorItem.Type == InventoryItem.Item.Type)
				{
					var item = cursorItem.Clone();
					item.Count += InventoryItem.Item.Count;

					ItemCursor.ClearItem();

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

					// Store temporary reference to the item in this inventory slot
					var item = InventoryItem.Item;

					// Clear the item graphic for this inventory slot
					InventoryItem.QueueFreeGraphic();
					InventoryItem = null;

					// Clear the item count graphic
					UpdateItemCountLabel(0);

					// Remove the item from the cursor
					ItemCursor.ClearItem();

					// Set the item in this inventory slot to the item from the cursor
					SetItem(cursorItem);

					// Attach the item from the inventory slot to the cursor (pick up the item)
					ItemCursor.SetItem(item);
				}
			}
			// There is no item attached to the cursor
			else
			{
				// Store temporary reference to the item in this inventory slot
				var item = InventoryItem.Item;

				// Clear the item graphic for this inventory slot
				InventoryItem.QueueFreeGraphic();
				InventoryItem = null;

				// Clear the item count graphic
				UpdateItemCountLabel(0);

				// Set the item in this inventory slot to the item from the cursor
				ItemCursor.SetItem(item);
			}
		}

		
	}

	private void HandleRightClick()
	{
		var cursorItem = ItemCursor.GetItem();

		// Is there a item attached to the cursor?
		if (cursorItem != null)
		{
			// There is no item in this inventory slot
			if (InventoryItem == null)
			{
				// Take 1 item from the cursor
				ItemCursor.TakeItem();

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
					ItemCursor.TakeItem();

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
				var invSlotItemCount = InventoryItem.Item.Count;

				// Is this the last item in the stack?
				if (invSlotItemCount - 1 == 0)
				{
					ItemCursor.SetItem(InventoryItem.Item);

					// Clear the item graphic for this inventory slot
					InventoryItem.QueueFreeGraphic();
					InventoryItem = null;
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
					ItemCursor.SetItem(item);
				}
			}
		}
	}
}
