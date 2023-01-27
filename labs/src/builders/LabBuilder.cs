using labs.entities;

namespace labs.builders;

public sealed class LabBuilder :
    LabEntityBuilder<string>
{
    public LabBuilder(Lab? lab = default) : 
        base(lab ?? new Lab())
    { }

    public LabBuilder Tasks (IList<LabTask> value)
    {
        ((Lab)Build()).Tasks = value
                .Distinct()
                .Select(x => (ILabEntity<string>)x)
                .ToList();

        return this;
    }

    public override LabBuilder Id(string value)
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