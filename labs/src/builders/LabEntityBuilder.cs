using labs.entities;
using tier1;

namespace labs.builders;

public abstract  class LabEntityBuilder<T> :
    IBuildable<ILabEntity<T>>
{
    private readonly ILabEntity<T> entity;

    protected LabEntityBuilder(ILabEntity<T> value)
    {
        entity = value;
    }

    public virtual LabEntityBuilder<T> Id(T value)
    {
        entity.Id = value;

        return this;
    }

    public virtual LabEntityBuilder<T> Name (string value)
    {
        entity.Name = value;
        
        return this;
    }

    public virtual LabEntityBuilder<T> Description(string value)
    {
        entity.Description = value;

        return this;
    }
    
    public ILabEntity<T> Build()
    {
        return entity;
    }
}