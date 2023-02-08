https://user-images.githubusercontent.com/6277739/217415936-173f1cae-ed8c-4790-9be6-cfe970446f78.mp4

https://user-images.githubusercontent.com/6277739/217419952-8adfd0e8-eb7a-4382-a633-ab195bcc5250.mp4

## A Game Inventory being made in Godot 4 Beta 17 C#

### Features
- [x] Inventories of varying sizes can be defined
- [x] Inventory slot size can be customized

### Implemented Controls
- [x] `Left Click` to pick up / place item stack
- [x] `Shift + Left Click` to transfer item stack from inventories A to B
- [x] `Right Click` to pick up / place single item
- [x] `Hold + Right Click` to continuously place a single item
- [x] `Double Click` to pick up all items of the same type

### Todo
- Sync animated sprite animations so they do not reset on being placed or picked up (might not do this)
- If a item is picked up, it will lerp to the cursors position then stop lerping when it gets there (might not do this)
- Player inventory hotbar
- Saving / loading inventory data
- Allow items to define their own max stack sizes
- Top down player controller + world with several containers to open
- Hold Left Click = Continuously Pickup Entire Stack
- Hold Shift + Left Click = Continuously Transfer Item from Inv A -> B
- Shift + Right Click = Split stack
- Shift + Q = Sort Inventory
- Ctrl + W = Take all items from Inv A -> B
- Hotkeys 1, 2, ... , 8, 9 transfer item to corresponding hotbar slot
- I = Open Inventory
- Q = Drop Item (x1)
- Ctrl + Q = Drop Stack

### Known Issues
- Multiple description panels stack on top of each other and do not disappear until more mouse movements or other inputs are made
- `Hold + Right Click` only works when dragging across empty inventory slots, not when there are items of the same type in the inventory slots already. Perhaps a simple bool check could solve this.

### Setup a Inventory
```cs
Inventory = new Inventory(this);

for (int i = 0; i < 9; i++)
    Inventory.SetItem(i, new Item(Items.Coin));

Inventory.SetItem(0, 2, new Item(Items.Coin, 3));
Inventory.SetItem(1, 2, new Item(Items.CoinSnowy));
```
