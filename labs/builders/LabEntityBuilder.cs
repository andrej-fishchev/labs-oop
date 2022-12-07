using labs.interfaces;

namespace labs.builders;

public abstract class LabEntityBuilder<T> :
    IBuildable
{
    private readonly ILabEntity<T> m_Entity;

    protected LabEntityBuilder(ILabEntity<T> entity)
    {
        m_Entity = entity;
    }

    public virtual LabEntityBuilder<T> Id(T value)
    {
        m_Entity.Id = value;

        return this;
    }

    public virtual LabEntityBuilder<T> Name (string value)
    {
        m_Entity.Name = value;
        
        return this;
    }

    public virtual LabEntityBuilder<T> Description(string value)
    {
        m_Entity.Description = value;

        return this;
    }
    
    public TB Build<TB>()
    {
        return (TB) m_Entity;
    }
}