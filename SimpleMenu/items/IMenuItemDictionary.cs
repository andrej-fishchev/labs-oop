namespace SimpleMenu.items;

public interface IMenuItemDictionary<TK, TV> :
    IDictionary<TK, TV>
{
    public void Add(IDictionary<TK, TV> items);
}