using SimpleMenu.items;

namespace SimpleMenu;

public abstract class MenuBuilder<TK, TV> :
    IBuildable<IMenu<TK, TV>> where TK : notnull
{
    private readonly IMenu<TK, TV> entity;

    protected MenuBuilder(IMenu<TK, TV> entity)
    {
        this.entity = entity;
    }

    public virtual MenuBuilder<TK, TV> Title(string value)
    {
        entity.Title = value;

        return this;
    }
    
    public virtual MenuBuilder<TK, TV> Exit(TK value)
    {
        entity.Exit = value;

        return this;
    }

    public virtual MenuBuilder<TK, TV> Items(IMenuItemDictionary<TK, TV> value)
    {
        entity.Items = value;

        return this;
    }
    
    public IMenu<TK, TV> Build()
    {
        return entity;
    }
}