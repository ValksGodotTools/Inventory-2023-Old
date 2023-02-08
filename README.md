https://user-images.githubusercontent.com/6277739/217415936-173f1cae-ed8c-4790-9be6-cfe970446f78.mp4

https://user-images.githubusercontent.com/6277739/217419952-8adfd0e8-eb7a-4382-a633-ab195bcc5250.mp4

## A Game Inventory being made in Godot 4 Beta 17 C#

### Todo
- Sync animated sprite animations so they do not reset on being placed or picked up (might not do this)
- If a item is picked up, it will lerp to the cursors position then stop lerping when it gets there (might not do this)
- Hold Left Click = Mass Place (x1)
- Shift + Left Click = Transfer Item from Inv A -> B
- Hold Shift + Left Click = Mass Transfer Item from Inv A -> B
- Shift + Right Click = Split stack
- Shift + Q = Sort Inventory
- Ctrl + W = Take all items from Inv A -> B
- Hotkeys 1, 2, ... , 8, 9 transfer item to corresponding hotbar slot
- I = Open Inventory
- Q = Drop Item (x1)
- Ctrl + Q = Drop Stack

### Setup a Inventory
```cs
Inventory = new Inventory(this);

for (int i = 0; i < 9; i++)
    Inventory.SetItem(i, new Item(Items.Coin));

Inventory.SetItem(0, 2, new Item(Items.Coin, 3));
Inventory.SetItem(1, 2, new Item(Items.CoinSnowy));
```
