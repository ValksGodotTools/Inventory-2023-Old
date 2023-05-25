namespace Inventory;

public abstract class UISlot
{
    public bool Visible
    {
        get => Parent.Visible;
        set => Parent.Visible = value;
    }

    public Control Parent { get; set; }

    protected Container Container { get; set; }
    protected int Index { get; set; }

    private UIItem UIItem { get; set; }

    public void Hide() => UIItem.Hide();
    public void Show() => UIItem.Show();

    public virtual Item Get() => Container.Get(Index);

    public virtual void MoveAllTo(UISlot other)
    {
        if (other.HasItem())
        {
            if (this.SameType(other))
            {
                var item = Container.TakeAll(Index);
                this.ClearGraphic();

                item.Count += other.Get().Count;

                other.Set(item);
            }
            else
            {
                this.Swap(other);
            }
        }
        else
        {
            if (this.HasItem())
            {
                other.Set(Container.TakeAll(Index));
                this.ClearGraphic();
            }
        }
    }

    public virtual void MoveHalfTo(UISlot other)
    {
        if (other.HasItem())
        {
            if (this.SameType(other))
            {
                var item = Container.TakeHalf(Index);
                this.UpdateCount();

                item.Count += other.Get().Count;

                other.Set(item);
            }
            else
            {
                this.Swap(other);
            }
        }
        else
        {
            if (this.HasItem())
            {
                var item = Container.TakeHalf(Index);

                if (item.Count > 0)
                    other.Set(item);

                this.UpdateCount();
            }
        }
        
        if (Container.IsEmpty(Index))
            this.ClearGraphic();
    }

    public virtual void MoveOneTo(UISlot other)
    {
        if (this.HasItem())
        {
            if (other.HasItem())
            {
                if (this.SameType(other))
                {
                    var item = Container.TakeOne(Index);

                    this.UpdateCount();

                    // Combine
                    item.Count += other.Get().Count;

                    // Set the [cursor] with the item that was taken
                    other.Set(item);
                }
                else
                {
                    this.Swap(other);
                }
            }
            else
            {
                var item = Container.TakeOne(Index);

                this.UpdateCount();

                other.Set(item);
            }
        }

        if (Container.IsEmpty(Index))
            this.ClearGraphic();
    }

    public virtual void Swap(UISlot other)
    {
        var otherItem = other.Get();

        other.Set(Get());
        this.Set(otherItem);
    }

    public virtual void Set(Item item)
    {
        Container.Set(Index, item);
        this.SetGraphic(item);
        this.SetCount(item.Count);
    }

    public virtual void Remove()
    {
        Container.Destroy(Index);
        this.ClearGraphic();
    }

    public virtual bool SameType(UISlot other) => other.Get().Type == Get().Type;

    public virtual bool IsEmpty() => Container.IsEmpty(Index);

    public virtual bool HasItem() => !this.IsEmpty();

    // UI Stuff
    private void SetCount(int count) => UIItem.SetText(count == 1 ? "" : count + "");
    private void UpdateCount()
    {
        if (HasItem())
            UIItem.SetText(Get().Count == 1 ? "" : Get().Count + "");
    }

    private void SetGraphic(Item item)
    {
        this.ClearGraphic();
        UIItem = new UIItem(Parent, item);
    }

    private void ClearGraphic() => Parent.QueueFreeChildren();
}
