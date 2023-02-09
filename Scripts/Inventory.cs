﻿namespace Inventory;

public class Inventory
{
	public static Inventory OtherInventory { get; set; } // a container the player opened
	public static Inventory PlayerInventory { get; set; }

	public InventorySlot[] InventorySlots { get; set; }
	public int SlotSize { get; set; } = 50;

	private GridContainer GridContainer { get; set; }
	private int Padding { get; set; } = 10;
	private int Columns { get; set; }
	private int Rows { get; set; }
	private Node Parent { get; set; }
	private PanelContainer PanelContainer { get; set; }

	public Inventory(Node parent, int columns = 9, int rows = 5, int slotSize = 50)
	{
		Parent = parent;
		Columns = columns;
		Rows = rows;
		SlotSize = slotSize;

		PanelContainer = new PanelContainer();

		var marginContainer = new MarginContainer();
		marginContainer.AddMargin(Padding);

		PanelContainer.AddChild(marginContainer);

		GridContainer = new GridContainer { Columns = columns };

		marginContainer.AddChild(GridContainer);

		InventorySlots = new InventorySlot[rows * columns];

		for (int i = 0; i < InventorySlots.Length; i++)
			InventorySlots[i] = new InventorySlot(this, GridContainer);

		SetAnchor(Control.LayoutPreset.Center);

		Parent.AddChild(PanelContainer);
	}

	public void SetAnchor(Control.LayoutPreset preset) =>
		PanelContainer.SetAnchorsAndOffsetsPreset(preset);

	public void SetItem(int i, Item item) =>
		InventorySlots[i].SetItem(item);

	public void SetItem(int x, int y, Item item) =>
		SetItem(x + y * Columns, item);

	public void TakeAll()
	{
		for (int i = 0; i < InventorySlots.Length; i++) 
		{
			var invItem = InventorySlots[i].InventoryItem;

			if (invItem != null)
			{
				var otherSlot = PlayerInventory.TryGetEmptyOrSameTypeSlot(invItem.Item.Type);
				
				var otherInvSlotItem = PlayerInventory.InventorySlots[otherSlot].InventoryItem;

				if (otherInvSlotItem != null)
				{
					var item = otherInvSlotItem.Item;
					item.Count += invItem.Item.Count;

					PlayerInventory.InventorySlots[otherSlot].SetItem(item);
				}
				else
				{
					PlayerInventory.InventorySlots[otherSlot].SetItem(invItem.Item);
				}

				InventorySlots[i].RemoveItem();
			}
		}
	}

	public int TryGetEmptyOrSameTypeSlot(ItemType itemType)
	{
		var foundEmptySlot = false;
		var emptySlotIndex = -1;

		for (int i = 0; i < InventorySlots.Length; i++)
		{
			var slot = InventorySlots[i];

			if (slot.InventoryItem != null && slot.InventoryItem.Item.Type == itemType)
				return i;

			if (!foundEmptySlot && slot.InventoryItem == null)
			{
				foundEmptySlot = true;
				emptySlotIndex = i;
			}
		}

		if (foundEmptySlot)
			return emptySlotIndex;

		return -1;
	}
}
