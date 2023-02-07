![Capture](https://user-images.githubusercontent.com/6277739/216915482-8fb374ae-f225-4d72-9fac-6ff632e90083.PNG)

Todo
- [x] A hovering panel appears when mousing over an item showing information about that item
- [x] Pick up a item, swap it for another or drop it somewhere else
- [ ] Items can be stacked
- [ ] If a item is picked up, it will lerp to the cursors position then stop lerping when it gets there

Example Code
```cs
var inventory = new Inventory(this);
inventory.SetItem(0, 1, Items.Coin);
inventory.SetItem(1, 1, Items.CoinSnowy);

for (int i = 0; i < inventory.InventorySlots.Length; i++)
    inventory.InventorySlots[i].AddDebugLabel("" + i);
```
