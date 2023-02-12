namespace Inventory;

public interface IItemHolder
{
	Item Item { get; set; }

	void SetItem(Item item);

	/// <summary>
	/// Swap item in this holder with the item from the other holder
	/// </summary>
	/// <param name="to">Where this item is being swapped to</param>
	void SwapItem(IItemHolder to);

	void RemoveItem();
}
