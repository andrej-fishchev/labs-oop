using labs.interfaces;

namespace labs.abstracts;

public abstract class MenuBuilder<TK, TV> :
    IBuildable<IMenu<TK, TV>> where TK : notnull
{
    private readonly IMenu<TK, TV> m_Entity;

    protected MenuBuilder(IMenu<TK, TV> entity)
    {
        m_Entity = entity;
    }

    public virtual MenuBuilder<TK, TV> Title(string value)
    {
        m_Entity.Title = value;

        return this;
    }
    
    public virtual MenuBuilder<TK, TV> Exit(TK value)
    {
        m_Entity.Exit = value;

        return this;
    }

    public virtual MenuBuilder<TK, TV> Items(IMenuItemDictionary<TK, TV> value)
    {
        m_Entity.Items = value;

        return this;
    }
    
    public IMenu<TK, TV> Build()
    {
        return m_Entity;
    }
}