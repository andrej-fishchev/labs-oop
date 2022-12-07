using labs.entities;

namespace labs.builders;

public class LabBuilder<T> :
    LabEntityBuilder<T>
{
    public LabBuilder() : 
        base(new Lab<T>(default!))
    { }

    public LabBuilder<T> Tasks (IList<LabTask<T>> value)
    {
        Build<Lab<T>>()
            .Tasks = value;

        return this;
    }

    public override LabBuilder<T> Id(T value)
    {
        return (LabBuilder<T>)base.Id(value);
    }

    public override LabBuilder<T> Name(string value)
    {
        return (LabBuilder<T>)base.Name(value);
    }

    public override LabBuilder<T> Description(string value)
    {
        return (LabBuilder<T>)base.Description(value);
    }
}