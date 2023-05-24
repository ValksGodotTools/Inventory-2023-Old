namespace Inventory;

public abstract class ItemHolder
{
	public virtual Item Item { get; set; }

	public Label ItemCountLabel { get; set; }

	public abstract void SetItem(Item item);
	public abstract void RemoveItem();

	/// <summary>
	/// Split stack and place half to ItemHolder
	/// </summary>
	public void SplitStack(ItemHolder to)
	{
		var itemA = Item;

		// Prevent duping 1 item to 2 items
		if (itemA.Count == 1)
			return;

		var itemB = itemA.Clone();

		var half = itemA.Count / 2;
		var remainder = itemA.Count % 2;

		itemA.Count = half + remainder;
		itemB.Count = half;

		SetItem(itemA);
		to.SetItem(itemB);
	}

	/// <summary>
	/// Place all items to this from ItemHolder
	/// </summary>
	public void PlaceAll(ItemHolder from)
	{
		var item = from.Item.Clone();
		item.Count += Item.Count;

		from.RemoveItem();

		SetItem(item);
	}

	/// <summary>
	/// Place one item to this from ItemHolder
	/// </summary>
	public void PlaceOne(ItemHolder from)
	{
		var item = from.Item.Clone();
		from.PickupOne();
		
		item.Count = 1;

		if (Item != null)
			item.Count += Item.Count;

		SetItem(item);
	}

	/// <summary>
	/// Take all item from the stack
	/// </summary>
	public void PickupAll(ItemHolder to)
	{
		to.SetItem(Item);
		RemoveItem();
	}

	/// <summary>
	/// Take one item from the stack
	/// </summary>
	public void PickupOne()
	{
		Item.Count -= 1;
		ItemCountLabel.Text = Item.Count + "";

		if (Item.Count <= 0)
			RemoveItem();
	}

	/// <summary>
	/// Pickup a item from ItemHolder of the same type
	/// </summary>
	public void PickupSameType(ItemHolder from)
	{
		if (Item.Type != from.Item.Type)
			return;

		Item.Count += from.Item.Count;

		from.SetItem(Item);
		RemoveItem();
	}

	/// <summary>
	/// Move item from ItemHolder A to ItemHolder B
	/// </summary>
	public void MoveItem(ItemHolder to)
	{
		SetItem(to.Item);
		to.RemoveItem();
	}

	/// <summary>
	/// Swap item in this holder with the item from the other holder
	/// </summary>
	/// <param name="to">Where this item is being swapped to</param>
	public void SwapItem(ItemHolder to)
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
}
