namespace lab_exec;

public abstract class Menu<TK, TV>
    where TK : notnull
{
    public virtual string Title { get; set; }
    
    public virtual TK ExitButton { get; set; }
    
    public virtual IDictionary<TK, TV> Items { get; set; }

    protected Menu(string title = "Unknown", IDictionary<TK, TV>? items = default)
    {
        Title = title;
        Items = items ?? new Dictionary<TK, TV>();
    }

    public virtual void AddItem(TK itemKey, TV itemValue)
    {
        Items.Add(itemKey, itemValue);
    }

    public virtual bool ContainsItem(TK itemKey)
    {
        return Items.ContainsKey(itemKey);
    }

    public virtual bool RemoveItem(TK itemKey)
    {
        return Items.Remove(itemKey);
    }

    public virtual TV GetItem(TK itemKey)
    {
        return Items[itemKey];
    }
}