namespace SimpleMenu.items;

public interface IMenuItemDictionary<TK, TV> :
    IDictionary<TK, TV>
{
    public void Add(IDictionary<TK, TV> items);

    public virtual TO As<TO>() where TO : class, IMenuItemDictionary<TK, TV>
        => (this as TO)!;
}