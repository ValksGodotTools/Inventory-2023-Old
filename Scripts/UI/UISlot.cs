namespace Inventory;

public abstract class UISlot
{
	public bool Visible
	{
		get => Parent.Visible;
		set => Parent.Visible = value;
	}

	public    Control   Parent    { get; set; }
						 
	protected Container Container { get; set; }
	protected int       Index     { get; set; }

	private   UIItem    UIItem    { get; set; }

	public void Hide() => UIItem.Hide();
	public void Show() => UIItem.Show();

	public virtual Item Get() => Container.Get(Index);

	public virtual void MoveAllTo(UISlot other)
	{
		if (other.HasItem())
		{
			if (SameType(other))
			{
				var item = Container.TakeAll(Index);
				ClearGraphic();

				item.Count += other.Get().Count;

				other.Set(item);
			}
			else
			{
				Swap(other);
			}
		}
		else
		{
			if (HasItem())
			{
				other.Set(Container.TakeAll(Index));
				ClearGraphic();
			}
		}
	}

	public virtual void MoveOneTo(UISlot other)
	{
		if (HasItem())
		{
			if (other.HasItem())
			{
				if (SameType(other))
				{
					var item = Container.TakeOne(Index);

					UpdateCount();

					// Combine
					item.Count += other.Get().Count;

					// Set the [cursor] with the item that was taken
					other.Set(item);
				}
				else
				{
					Swap(other);
				}
			}
			else
			{
				var item = Container.TakeOne(Index);

				UpdateCount();

				other.Set(item);
			}
		}

		if (Container.IsEmpty(Index))
			ClearGraphic();
	}

	public virtual void Swap(UISlot other)
	{
		var otherItem = other.Get();

		other.Set(Get());
		Set(otherItem);
	}

	public virtual void Set(Item item)
	{
		Container.Set(Index, item);
		SetGraphic(item);
		SetCount(item.Count);
	}

	public virtual void Remove()
	{
		Container.Destroy(Index);
		ClearGraphic();
	}

	public virtual bool SameType(UISlot other) => other.Get().Type == Get().Type;

	public virtual bool IsEmpty() => Container.IsEmpty(Index);

	public virtual bool HasItem() => !IsEmpty();

	// UI Stuff
	private void SetCount(int count) => UIItem.SetText(count == 1 ? "" : count + "");
	private void UpdateCount()
	{
		if (HasItem())
			UIItem.SetText(Get().Count == 1 ? "" : Get().Count + "");
	}

	private void SetGraphic(Item item)
	{
		ClearGraphic();
		UIItem = new UIItem(Parent, item);
	}

	private void ClearGraphic() => Parent.QueueFreeChildren();
}
