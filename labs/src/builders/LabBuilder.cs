using labs.entities;

namespace labs.builders;

public sealed class LabBuilder :
    LabEntityBuilder<int>
{
    public LabBuilder(Lab? lab = default) : 
        base(lab ?? new Lab())
    { }

    public LabBuilder Tasks (IList<LabTask> value)
    {
        ((Lab)Build()).Tasks = value
                .Distinct()
                .Select(x => (ILabEntity<int>)x)
                .ToList();

        return this;
    }

    public override LabBuilder Id(int value)
    {
        return (LabBuilder) base.Id(value);
    }

    public override LabBuilder Name(string value)
    {
        return (LabBuilder) base.Name(value);
    }

    public override LabBuilder Description(string value)
    {
        return (LabBuilder) base.Description(value);
    }
}