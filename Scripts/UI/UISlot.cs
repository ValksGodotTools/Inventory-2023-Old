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

    public ItemCategory? ItemCategoryFilter { get; set; } //all categories allowed if null

    public virtual Item Get() => Container.Get(Index);

    #region Alter Items
    public virtual void MoveAllTo(UISlot other)
    {
        if (other.IsItemCategoryAllowed(Container.Get(Index)))
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
    }

    public virtual void MoveHalfTo(UISlot other)
    {
        if (other.IsItemCategoryAllowed(Container.Get(Index)))
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
    }


    public virtual void MoveOneTo(UISlot other)
    {
        if (other.IsItemCategoryAllowed(Container.Get(Index)))
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
                    if (other.IsItemCategoryAllowed(Container.Get(Index)))
                    {
                        var item = Container.TakeOne(Index);

                        this.UpdateCount();

                        other.Set(item);
                    }
                }
            }

            if (Container.IsEmpty(Index))
                this.ClearGraphic();
        }
    }

    /// <summary>
    /// Swap this Slot Item with other Slot Item if allowed.
    /// </summary>
    /// <param name="other"></param>
    public virtual void Swap(UISlot other)
    {
        var otherItem = other.Get();
        if (this.IsItemCategoryAllowed(otherItem))
        {
            other.Set(this.Get());
            this.Set(otherItem);
        }
    }

    /// <summary>
    /// Set Item for this Slot.
    /// </summary>
    /// <param name="item"></param>
    public virtual void Set(Item item)
    {
        if (this.IsItemCategoryAllowed(item))
        {
            Container.Set(Index, item);
            this.SetGraphic(item);
            this.SetCount(item.Count);
        }
    }

    public virtual void Remove()
    {
        Container.Destroy(Index);
        this.ClearGraphic();
    }

    #endregion

    public virtual bool SameType(UISlot other) => other.Get().Type == this.Get().Type;

    /// <summary>
    /// Checks if param ItemCategory is allowed on this slot.
    /// </summary>
    /// <param name="item"></param>
    /// <returns>true if ItemCategory is allowed on this slot, false otherwise</returns>
    public virtual bool IsItemCategoryAllowed(Item item) => (ItemCategoryFilter == null || item.Type.ItemCategory == ItemCategoryFilter);

    public virtual bool IsEmpty() => Container.IsEmpty(Index);

    public virtual bool HasItem() => !this.IsEmpty();

    #region UI
    public void Hide() => UIItem.Hide();
    public void Show() => UIItem.Show();

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

    #endregion
}
