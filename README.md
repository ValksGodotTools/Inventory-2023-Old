![Capture](https://user-images.githubusercontent.com/6277739/216915482-8fb374ae-f225-4d72-9fac-6ff632e90083.PNG)

```cs
var inventory = new Inventory(this)
{
	SlotSize = 50
};

inventory.AddChild();
inventory.SetAnimatedItem(0, 1, "sprite_frames_coin");
inventory.SetStaticItem  (1, 1, "snowy_coin");

for (int i = 0; i < inventory.InventorySlots.Length; i++)
	inventory.InventorySlots[i].AddDebugLabel("" + i);
```
