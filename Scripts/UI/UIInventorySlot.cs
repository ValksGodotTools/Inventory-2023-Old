namespace Inventory;

public class UIInventorySlot : UISlot
{
	private UIInventory UIInventory { get; set; }

	public UIInventorySlot(UIInventory uiInventory, int index)
	{
		Index = index;
		UIInventory = uiInventory;
		Container = uiInventory.Container;
		Parent = CreateUI(uiInventory);
	}

	public void Update()
	{
		
	}

	private void HandleLeftClick()
	{
		if (Main.Cursor.IsEmpty())
		{
			MoveAllTo(Main.Cursor);
		}
		else
		{
			Main.Cursor.MoveAllTo(this);
		}
	}

	private void HandleShiftLeftClick()
	{
		TransferAll();
	}

	private void HandleRightClick()
	{
		if (Main.Cursor.IsEmpty())
		{
			MoveOneTo(Main.Cursor);
		}
		else
		{
			Main.Cursor.MoveOneTo(this);
		}
	}

	private void TransferAll()
	{
		if (IsEmpty())
			return;

		var targetInv = UIInventory == Main.PlayerInventory ?
			Main.OtherInventory : Main.PlayerInventory;

		var slotIndex = targetInv.Container.TryGetEmptyOrSameTypeSlot(Get().Type);

		if (slotIndex == -1)
			return;

		var thisItem = Get();
		var targetItem = targetInv.Container.Get(slotIndex);

		if (targetInv.Container.HasItem(slotIndex))
		{
			if (targetItem.Type == thisItem.Type)
			{
				targetItem.Count += thisItem.Count;

				targetInv.SetItem(slotIndex, targetItem);

				Remove();
			}
		}
		else
		{
			targetInv.SetItem(slotIndex, thisItem);
			Remove();
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
					HandleShiftLeftClick();
					return;
				}

				HandleLeftClick();
				return;
			}

			if (eventMouseButton.IsRightClickPressed())
			{
				HandleRightClick();
				return;
			}
		};

		uiInventory.GridContainer.AddChild(panel);
		return panel;
	}
}
