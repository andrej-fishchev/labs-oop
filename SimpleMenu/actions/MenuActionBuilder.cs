using tier1;

namespace SimpleMenu.actions;

public abstract class MenuActionBuilder<TK, TV> :
    IBuildable<MenuAction<TK, TV>> 
    where TK : notnull
{
    private readonly MenuAction<TK, TV> entity;

    protected MenuActionBuilder(MenuAction<TK, TV> entity)
    {
        this.entity = entity;
    }

    public virtual MenuActionBuilder<TK, TV> OnDraw(Action<IMenu<TK, TV>> value)
    {
        entity.OnDraw = value;

        return this;
    }
    
    public virtual MenuActionBuilder<TK, TV> OnDisplay(Action<IMenu<TK, TV>> value)
    {
        entity.OnDisplay = value;

        return this;
    }

    public virtual MenuActionBuilder<TK, TV> OnClose(Action<IMenu<TK, TV>> value)
    {
        entity.OnClose = value;

        return this;
    }
    
    public virtual MenuActionBuilder<TK, TV> OnSelect(Action<IMenu<TK, TV>, TK> value)
    {
        entity.OnSelect = value;

        return this;
    }
    
    public MenuAction<TK, TV> Build()
    {
        return entity;
    }
}