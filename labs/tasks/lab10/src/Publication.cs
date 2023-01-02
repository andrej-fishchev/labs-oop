using IO.targets;

namespace labs.lab10.src;

public abstract class Publication
{
    public string Name { get; set; }
    
    public String Date { get; set; }

    protected Publication(string name, string date)
    {
        Name = name;
        Date = date;
    }

    public virtual void Describe(TextWriter target)
    {
        target.WriteLine($"Release year of {Name} is {Date}");
    }
    
    public void NoOverridingDescribe(TextWriter target)
    {
        target.WriteLine($"Release year of {Name} is {Date}");
    }
}