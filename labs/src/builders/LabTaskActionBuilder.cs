using labs.entities;

namespace labs.builders;

public sealed class LabTaskActionBuilder :
    LabEntityBuilder<int>
{
    public LabTaskActionBuilder(LabTaskAction? labTaskAction = default) :
        base(labTaskAction ?? new LabTaskAction())
    { }

    public LabTaskActionBuilder ExecuteAction (Action value)
    {
        ((LabTaskAction) Build())
            .ExecuteAction = value;

        return this;
    }

    public override LabTaskActionBuilder Description(string value)
    {
        return (LabTaskActionBuilder) base.Description(value);
    }

    public override LabTaskActionBuilder Name(string value)
    {
        return (LabTaskActionBuilder) base.Name(value);
    }

    public override LabTaskActionBuilder Id(int value)
    {
        return (LabTaskActionBuilder) base.Id(value);
    }
}