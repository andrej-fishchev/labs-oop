using labs.entities;

namespace labs.builders;

public sealed class LabTaskActionBuilder<T> :
    LabEntityBuilder<T>
{
    public LabTaskActionBuilder() :
        base(new LabTaskAction<T>(default!))
    { }

    public LabTaskActionBuilder<T> Delegator (Action value)
    {
        Build<LabTaskAction<T>>()
            .Delegator = value;

        return this;
    }

    public override LabTaskActionBuilder<T> Description(string value)
    {
        return (LabTaskActionBuilder<T>) base.Description(value);
    }

    public override LabTaskActionBuilder<T> Name(string value)
    {
        return (LabTaskActionBuilder<T>) base.Name(value);
    }

    public override LabTaskActionBuilder<T> Id(T value)
    {
        return (LabTaskActionBuilder<T>)base.Id(value);
    }
}