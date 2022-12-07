using labs.entities;
using labs.interfaces;

namespace labs.builders;

public class LabTaskBuilder<T> :
    LabEntityBuilder<T>
{
    public LabTaskBuilder(ILabEntity<T> entity) : 
        base(entity)
    { }

    public LabTaskBuilder<T> Actions (IList<LabTaskAction<T>> value)
    {
        Build<LabTask<T>>()
            .Actions = value;

        return this;
    }

    public override LabTaskBuilder<T> Id(T value)
    {
        return (LabTaskBuilder<T>)base.Id(value);
    }

    public override LabTaskBuilder<T> Name(string value)
    {
        return (LabTaskBuilder<T>)base.Name(value);
    }

    public override LabTaskBuilder<T> Description(string value)
    {
        return (LabTaskBuilder<T>)base.Description(value);
    }
}