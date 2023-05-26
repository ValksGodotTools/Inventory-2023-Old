namespace Inventory;

public abstract class UISlot
{
    public bool Visible
    {
        get => Parent.Visible;
        set => Parent.Visible = value;
    }

    public Control Parent { get; set; }
    public ItemCategory? ItemCategoryFilter { get; set; } //all categories allowed if null

    protected Container container;
    protected int index;

    private UIItem uiItem;

    public virtual Item Get() => container.Get(index);

    #region Alter Items
    public virtual void MoveAllTo(UISlot other)
    {
        if (other.IsItemCategoryAllowed(container.Get(index)))
        {
            if (other.HasItem())
            {
                if (this.SameType(other))
                {
                    //Check type and stack limit
                    if (other.Get().Stacklimit >= other.Get().Count + this.Get().Count)
                    {
                        var item = container.TakeAll(index);
                        this.ClearGraphic();

                        item.Count += other.Get().Count;

                        other.Set(item);
                    }
                    else //Take amount necesary to reach stack limit
                    {
                        var item = container.Take(index, other.Get().Stacklimit - other.Get().Count);
                        this.UpdateCount();
                        item.Count += other.Get().Count;
                        other.Set(item);
                    }
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
                    other.Set(container.TakeAll(index));
                    this.ClearGraphic();
                }
            }
        }
    }

    public virtual void MoveHalfTo(UISlot other)
    {
        if (other.IsItemCategoryAllowed(container.Get(index)))
        {
            if (other.HasItem())
            {
                if (this.SameType(other))
                {
                    //Check type and stack limit
                    if (other.Get().Stacklimit >= other.Get().Count + this.Get().Count)
                    {
                        var item = container.TakeHalf(index);
                        this.UpdateCount();

                        item.Count += other.Get().Count;

                        other.Set(item);
                    }
                    else //Take amount necesary to reach stack limit
                    {
                        var item = container.Take(index, other.Get().Stacklimit - other.Get().Count);
                        this.UpdateCount();
                        item.Count += other.Get().Count;
                        other.Set(item);
                    }
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
                    var item = container.TakeHalf(index);

                    if (item.Count > 0)
                        other.Set(item);

                    this.UpdateCount();
                }
            }

            if (container.IsEmpty(index))
                this.ClearGraphic();
        }
    }


    public virtual void MoveOneTo(UISlot other)
    {
        if (other.IsItemCategoryAllowed(container.Get(index)))
        {
            if (this.HasItem())
            {
                if (other.HasItem())
                {
                    //Check type and stack limit
                    if (this.SameType(other) && other.Get().Stacklimit >= other.Get().Count + 1)
                    {
                        var item = container.TakeOne(index);

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
                    if (other.IsItemCategoryAllowed(container.Get(index)))
                    {
                        var item = container.TakeOne(index);

                        this.UpdateCount();

                        other.Set(item);
                    }
                }
            }

            if (container.IsEmpty(index))
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
            container.Set(index, item);
            this.SetGraphic(item);
            this.SetCount(item.Count);
        }
    }

    public virtual void Remove()
    {
        container.Destroy(index);
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

    public virtual bool IsEmpty() => container.IsEmpty(index);

    public virtual bool HasItem() => !this.IsEmpty();

    #region UI
    public void Hide() => uiItem.Hide();
    public void Show() => uiItem.Show();

    private void SetCount(int count) => uiItem.SetText(count == 1 ? "" : count + "");
    public void UpdateCount()
    {
        if (HasItem())
            uiItem.SetText(Get().Count == 1 ? "" : Get().Count + "");
    }

    private void SetGraphic(Item item)
    {
        this.ClearGraphic();
        uiItem = new UIItem(Parent, item);
    }

    private void ClearGraphic() => Parent.QueueFreeChildren();

    #endregion
}
