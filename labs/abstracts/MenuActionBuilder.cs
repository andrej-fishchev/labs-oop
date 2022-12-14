using labs.interfaces;

namespace labs.abstracts;

public abstract class MenuActionBuilder<TK, TV> :
    IBuildable<MenuAction<TK, TV>> 
    where TK : notnull
{
    private readonly MenuAction<TK, TV> m_Entity;

    protected MenuActionBuilder(MenuAction<TK, TV> entity)
    {
        m_Entity = entity;
    }

    public virtual MenuActionBuilder<TK, TV> OnDraw(Action<IMenu<TK, TV>> value)
    {
        m_Entity.OnDraw = value;

        return this;
    }
    
    public virtual MenuActionBuilder<TK, TV> OnDisplay(Action<IMenu<TK, TV>> value)
    {
        m_Entity.OnDisplay = value;

        return this;
    }

    public virtual MenuActionBuilder<TK, TV> OnClose(Action<IMenu<TK, TV>> value)
    {
        m_Entity.OnClose = value;

        return this;
    }
    
    public virtual MenuActionBuilder<TK, TV> OnSelect(Action<IMenu<TK, TV>, TK> value)
    {
        m_Entity.OnSelect = value;

        return this;
    }
    
    public MenuAction<TK, TV> Build()
    {
        return m_Entity;
    }
}