![Capture](https://user-images.githubusercontent.com/6277739/216915482-8fb374ae-f225-4d72-9fac-6ff632e90083.PNG)

Todo
- Hold right click = drag place single item
- Double left click = pick up all items of same type
- Sync animated sprite animations so they do not reset on being placed or picked up
- If a item is picked up, it will lerp to the cursors position then stop lerping when it gets there
- Add another inventory, the player inventory. Work on transfering items between inventories and introducing new hotkeys like Shift + Left Click

Example Code
```cs
var inventory = new Inventory(this);
inventory.SetItem(0, 1, Items.Coin);
inventory.SetItem(1, 1, Items.CoinSnowy);

for (int i = 0; i < inventory.InventorySlots.Length; i++)
    inventory.InventorySlots[i].AddDebugLabel("" + i);
```
