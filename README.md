https://user-images.githubusercontent.com/6277739/217415936-173f1cae-ed8c-4790-9be6-cfe970446f78.mp4

## A Game Inventory being made in Godot 4 Beta 17 C#

### Features
- [x] Inventories of varying sizes can be defined
- [x] Inventory slot size can be customized

### Implemented Controls
- [x] `Left Click` to pick up / place item stack
- [x] `Hold + Left Click` to continuously pick up items of the same type
- [x] `Shift + Left Click` to transfer item stack from inventories A to B
- [x] `Hold + Shift + Left Click` to continuously transfer item stack from inventories A to B
- [x] `Shift + Right Click` to split stack
- [x] `Right Click` to pick up / place single item
- [x] `Hold + Right Click` to continuously place a single item
- [x] `Double Click` to pick up all items of the same type
- [x] `Shift + W` to take all items from the 'other inventory' and put them in the players inventory
- [x] `Shift + Q` to sort items in all open inventories by descending item count
- [x] `1` `2` `3` `4` `5` `6` `7` `8` `9` to move / swap items to player hotbar slots

### Todo
- If a item is picked up, it will lerp to the cursors position then stop lerping when it gets there (might not do this)
- Player inventory hotbar
- Saving / loading inventory data
- Allow items to define their own max stack sizes
- Top down player controller + world with several containers to open
- I = Open Inventory
- Q = Drop Item (x1)
- Ctrl + Q = Drop Stack

### Known Issues
- Multiple description panels stack on top of each other and do not disappear until more mouse movements or other inputs are made

### Example Code
```cs
private Inventory ChestInv { get; set; }
private Inventory PlayerInv { get; set; }

public override void _Ready()
{
    ChestInv = new Inventory(this);
    ChestInv.SetAnchor(Control.LayoutPreset.CenterTop);

    for (int i = 0; i < 9; i++)
        ChestInv.SetItem(i, new Item(Items.Coin));

    ChestInv.SetItem(0, 2, new Item(Items.Coin, 3));
    ChestInv.SetItem(1, 2, new Item(Items.CoinSnowy));

    PlayerInv = new Inventory(this);
    PlayerInv.SetAnchor(Control.LayoutPreset.CenterBottom);

    for (int i = 18; i < 27; i++)
        PlayerInv.SetItem(i, new Item(Items.CoinPink));

    Inventory.PlayerInventory = PlayerInv;
    Inventory.OtherInventory = ChestInv;
}
```
