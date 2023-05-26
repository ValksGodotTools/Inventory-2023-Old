namespace Inventory;

public class Container
{
    public Item[] Items { get; set; }

    public Container(int size)
    {
        Items = new Item[size];
    }

    #region Substracts from item count
    public Item TakeAll(int i)
    {
        var item = Items[i];

        Destroy(i);

        return item;
    }

    public Item TakeOne(int i)
    {
        // Lets say we have 1 pink coins
        // We want to take 1 so there will be 0 pink coins left

        // Lets take 1 pink coin
        var item = Items[i].Clone();
        item.Count = 1;

        // 1 pink coins becomes 0 pink coins
        Items[i].Count -= 1;

        // Nothing left in this stack
        if (Items[i].Count == 0)
        {
            // Set this item to null
            Destroy(i);
        }

        return item;
    }

    public Item TakeHalf(int i)
    {
        // Lets say we have 11 pink coins
        // We want to take 5 so there will be 6 pink coins left

        // Lets take 5 pink coin
        var item = Items[i].Clone();
        item.Count = Items[i].Count / 2;

        // 11 pink coins becomes 6 pink coins
        Items[i].Count -= item.Count;

        // Nothing left in this stack
        if (Items[i].Count == 0)
        {
            // Set this item to null
            Destroy(i);
        }

        return item;
    }

    public Item Take(int i, int amount)
    {
        var item = Items[i].Clone();

        item.Count = amount;
        Items[i].Count -= amount;

        if (Items[i].Count <= 0)
            Destroy(i);

        return item;
    }
    #endregion

    public int TryGetEmptyOrSameTypeSlot(ItemType itemType)
    {
        var foundEmptySlot = false;
        var emptySlotIndex = -1;

        for (int i = 0; i < Items.Length; i++)
        {
            var item = Items[i];

            //Check for same type items without reaching stack limit
            if (item != null && item.Type == itemType && item.Count < item.Stacklimit)
                return i;

            if (!foundEmptySlot && item == null)
            {
                foundEmptySlot = true;
                emptySlotIndex = i;
            }
        }

        if (foundEmptySlot)
            return emptySlotIndex;

        return -1;
    }

    public void Set(int i, Item item) => Items[i] = item;
    public Item Get(int i) => Items[i];
    public void Swap(int i, int j) => (Items[i], Items[j]) = (Items[j], Items[i]);
    public bool IsEmpty(int i) => Items[i] == null;
    public bool HasItem(int i) => !IsEmpty(i);
    public void Destroy(int i) => Items[i] = null;
}
