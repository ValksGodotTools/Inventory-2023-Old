namespace Inventory;

interface IItemHolder
{
	Item Item { get; set; }

	void SetItem(Item item);
	void RemoveItem();
}
