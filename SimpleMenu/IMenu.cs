using SimpleMenu.items;

namespace SimpleMenu;

public interface IMenu<TK, TV>
    where TK : notnull
{
    public string Title { get; set; }
    
    public TK Exit { get; set; }
    
    public IMenuItemDictionary<TK, TV> Items { get; set; }
}