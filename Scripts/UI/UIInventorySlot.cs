using System.Linq;

namespace Inventory;

public class UIInventorySlot : UISlot
{
    private UIInventory UIInventory { get; set; }

    public UIInventorySlot(UIInventory uiInventory, int index, ItemCategory? itemCategoryFilter = null)
    {
        Index = index;
        UIInventory = uiInventory;
        Container = uiInventory.Container;
        Parent = this.CreateUI(uiInventory);
        this.ItemCategoryFilter = itemCategoryFilter;
    }

    public void Update()
    {

    }

    private void HandleLeftClick()
    {
        if (Main.Cursor.IsEmpty())
        {
            this.MoveAllTo(Main.Cursor);
        }
        else
        {
            Main.Cursor.MoveAllTo(this);
        }
    }

    private void HandleShiftLeftClick()
    {
        this.TransferAll();
    }

    private void HandleRightClick()
    {
        if (Main.Cursor.IsEmpty())
        {
            this.MoveOneTo(Main.Cursor);
        }
        else
        {
            Main.Cursor.MoveOneTo(this);
        }
    }

    /// <summary>
    /// If cursor is empty, takes half of slot to cursor
    /// If slot is empty, takes half of cursor to slot
    /// </summary>
    private void HandleShiftRightClick()
    {
        if (Main.Cursor.IsEmpty())
        {
            this.MoveHalfTo(Main.Cursor);
        }
        else
        {
            Main.Cursor.MoveHalfTo(this);
        }
    }

    private void TransferAll()
    {
        if (this.IsEmpty())
            return;

        var thisItem = this.Get();

        //Priorise Inventory with same item type filter and disponible slots
        var targetInv = (UIInventory == Main.PlayerInventory) ?
            Main.InventoryCollection.FirstOrDefault
            (i => (i.ItemCategoryFilter == thisItem.Type.ItemCategory || i.ItemCategoryFilter == null) 
                && i.Container.TryGetEmptyOrSameTypeSlot(thisItem.Type) != -1 )
            : Main.PlayerInventory;

        if (targetInv == null)
            return;

        var slotIndex = targetInv.Container.TryGetEmptyOrSameTypeSlot(thisItem.Type);

        if (slotIndex == -1)
            return;

        var targetItem = targetInv.Container.Get(slotIndex);

        if (targetInv.Container.HasItem(slotIndex))
        {
            if (targetItem.Type == thisItem.Type)
            {
                targetItem.Count += thisItem.Count;

                targetInv.SetItem(slotIndex, targetItem);

                this.Remove();
            }
        }
        else
        {
            targetInv.SetItem(slotIndex, thisItem);
            this.Remove();
        }
    }

    private Panel CreateUI(UIInventory uiInventory)
    {
        var panel = new Panel { CustomMinimumSize = Vector2.One * uiInventory.SlotSize };

        panel.GuiInput += (inputEvent) =>
        {
            if (inputEvent is not InputEventMouseButton eventMouseButton)
                return;

            if (eventMouseButton.IsLeftClickPressed())
            {
                if (Input.IsKeyPressed(Key.Shift))
                {
                    this.HandleShiftLeftClick();
                    return;
                }

                this.HandleLeftClick();
                return;
            }

            if (eventMouseButton.IsRightClickPressed())
            {
                if (Input.IsKeyPressed(Key.Shift))
                {
                    this.HandleShiftRightClick();
                    return;
                }

                this.HandleRightClick();
                return;
            }
        };

        panel.MouseEntered += () =>
		{
            Main.ItemDetails.ChangeItem(this.Get());
		};

		panel.MouseExited += () =>
		{
            Main.ItemDetails.Clear();
		};

        uiInventory.GridContainer.AddChild(panel);
        return panel;
    }
}
