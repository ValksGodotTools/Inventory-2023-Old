https://user-images.githubusercontent.com/6277739/217958511-d5182a65-60e9-464d-82d3-23a069720ae3.mp4

## A Game Inventory being made in Godot 4 C#
The goal of this project is to attempt to perfect a game inventory system so it can be used across all games that make use of inventories.

#### Table of Contents
1. [Features](#features)
2. [Implemented Controls](#implemented-controls)
3. [Todo](#todo)
4. [Known Issues](#known-issues)
5. [Example Code](#example-code)
6. [Contributing](#contributing)
7. [Previews](#previews)

### Features
- [x] Inventories of varying sizes can be defined
- [x] Inventory slot size can be customized
- [x] Player Inventory Hotbar
- [x] Smooth player inventory open / close animations

### Implemented Controls
- [x] `Left Click` to pick up / place item stack
- [x] `Hold + Left Click` to continuously pick up items of the same type
- [x] `Shift + Left Click` to transfer item stack from inventories A to B
- [x] `Hold + Shift + Left Click` to continuously transfer item stack from inventories A to B
- [x] `Shift + Right Click` to split stack
- [x] `Right Click` to pick up / place single item
- [x] `Hold + Right Click` to continuously place a single item
- [x] `Double Click` to pick up all items of the same type
- [x] `Shift + R` to sort items in all open inventories by descending item count
- [x] `Shift + T` to take all items from the 'other inventory' and put them in the players inventory
- [x] `1` `2` `3` `4` `5` `6` `7` `8` `9` to move / swap items to player hotbar slots
- [x] `I` to open the players inventory
- [x] `E` to interact with objects in the world

### Todo
- Item movement animations (might not do this)
- Hotkey to deposit all items from player inventory to 'other inventory'
- Abstract the code so the player inventory and hotbar appear at the top instead of the bottom (so can easily switch between if you want the player inventory to be at the bottom or top for your specific game)
- Saving / loading inventory data
- Allow items to define their own max stack sizes
- Q = Drop Item (x1)
- Ctrl + Q = Drop Stack
- Maybe implementing ENet-CSharp and syncing all of this inventory stuff over the network. (There would be a demo where 2 players would see each other walking around and see the inventory update when the other player interacts with it)
- Giving items a category property so the sorting hotkey can sort by category or any other item property for that matter
- Add item rarities (maybe rare items have a glowing outline achieved used Godot Shaders)

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

### Contributing
Please talk to me over Discord `va#9904` before you do anything. 

Have a look at the [todo list](#todo) and the [projects issues](https://github.com/Valks-Games/Inventory/issues) for things to do. On top of all that, I'm always trying to find ways to clean up the code and make it more humanly readable.

Please follow [this code style](https://github.com/Valks-Games/sankari/wiki/Code-Style)

### Previews
https://user-images.githubusercontent.com/6277739/217701119-4f5dcf6e-3004-4f91-b966-8de9b8ba98c7.mp4

https://user-images.githubusercontent.com/6277739/217701126-bbe38e2b-e962-4ad8-a6ce-28c29035e0d0.mp4

https://user-images.githubusercontent.com/6277739/217701135-0271306f-b915-4006-b333-18eb170ac19b.mp4

https://user-images.githubusercontent.com/6277739/217701142-748b222a-4f6d-46e3-bbc5-b333c83d628e.mp4
