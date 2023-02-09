namespace Inventory;

public class Inventory
{
	public static Inventory OtherInventory { get; set; } // a container the player opened
	public static InventorySlot ActiveInventorySlot { get; set; } // mouse is currently hovering over this slot
	public static Chest ActiveChest { get; set; }

	public InventorySlot[] InventorySlots { get; set; }
	public int SlotSize { get; set; } = 50;

	private GridContainer GridContainer { get; set; }
	private int Padding { get; set; } = 10;
	public int Columns { get; set; }
	private int Rows { get; set; }
	private Node Parent { get; set; }
	private PanelContainer PanelContainer { get; set; }

	public Inventory(Node node, int columns = 9, int rows = 5, int slotSize = 50)
	{
		Parent = node.GetTree().Root.GetNode("Main/CanvasLayer");
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

		Parent.AddChild(PanelContainer);

		// Must set anchor after all children are added to the scene
		SetAnchor(Control.LayoutPreset.CenterTop); // center top by default

		// hide by default
		PanelContainer.Hide();
	}

	public void MakePanelInvisible() =>
		PanelContainer.AddThemeStyleboxOverride("panel", new StyleBoxEmpty());

	public void Show() => PanelContainer.Show();
	public void Hide() => PanelContainer.Hide();
	public void ToggleVisibility() => PanelContainer.Visible = !PanelContainer.Visible;

	public void SetAnchor(Control.LayoutPreset preset) =>
		PanelContainer.SetAnchorsAndOffsetsPreset(preset);

	public void SetItem(int i, Item item) =>
		InventorySlots[i].SetItem(item);

	public void SetItem(int x, int y, Item item) =>
		SetItem(x + y * Columns, item);

	public void Sort()
	{
		var items = new Dictionary<ItemType, int>();

		// Store all items in a dictionary
		for (int i = 0; i < InventorySlots.Length; i++)
		{
			var invItem = InventorySlots[i].InventoryItem;

			if (invItem == null)
				continue;

			if (items.ContainsKey(invItem.Item.Type))
				items[invItem.Item.Type] += invItem.Item.Count;
			else
				items[invItem.Item.Type] = invItem.Item.Count;

			InventorySlots[i].RemoveItem();
		}

		// Sort by item count (descending)
		items = items.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

		// Place all items from the dictionary
		var index = 0;

		foreach (var item in items)
		{
			var itemType = item.Key;
			var itemCount = item.Value;

			var theItem = new Item(itemType, itemCount);

			InventorySlots[index++].SetItem(theItem);
		}
	}

	public void TakeAll()
	{
		for (int i = 0; i < InventorySlots.Length; i++) 
		{
			var invItem = InventorySlots[i].InventoryItem;

			if (invItem == null)
				continue;

			var otherSlot = Player.Inventory.TryGetEmptyOrSameTypeSlot(invItem.Item.Type);

			var otherInvSlotItem = Player.Inventory.InventorySlots[otherSlot].InventoryItem;

			if (otherInvSlotItem != null)
			{
				var item = otherInvSlotItem.Item;
				item.Count += invItem.Item.Count;

				Player.Inventory.InventorySlots[otherSlot].SetItem(item);
			}
			else
			{
				Player.Inventory.InventorySlots[otherSlot].SetItem(invItem.Item);
			}

			InventorySlots[i].RemoveItem();
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
