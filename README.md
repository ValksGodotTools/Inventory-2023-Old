https://user-images.githubusercontent.com/6277739/217415936-173f1cae-ed8c-4790-9be6-cfe970446f78.mp4

A Game Inventory being made in Godot 4 Beta 17 C#.

Todo
- Sync animated sprite animations so they do not reset on being placed or picked up
- If a item is picked up, it will lerp to the cursors position then stop lerping when it gets there
- Add another inventory, the player inventory. Work on transfering items between inventories and introducing new hotkeys like Shift + Left Click

Example Code
```cs
Inventory = new Inventory(this);

for (int i = 0; i < 9; i++)
    Inventory.SetItem(i, new Item(Items.Coin));

Inventory.SetItem(0, 2, new Item(Items.Coin, 3));
Inventory.SetItem(1, 2, new Item(Items.CoinSnowy));
```
