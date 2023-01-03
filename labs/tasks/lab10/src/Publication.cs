using System.Text;

namespace labs.lab10.src;

public abstract class Publication
{
    public string Name { get; set; }
    
    public DateOnly Date { get; set; }

    protected Publication(string name, DateOnly date)
    {
        Name = name;
        Date = date;
    }

    public virtual void Describe(TextWriter target)
    {
        target.WriteLine($"Печатное издание: {ToString()} \n");
    }
    
    public void NoOverridingDescribe(TextWriter target)
    {
        StringBuilder builder = new StringBuilder("")
            .Append($"Название: {Name} \n")
            .Append($"Дата публикации: {Date.ToShortDateString()} \n");
            
        target.WriteLine($"Печатное издание: \n{ builder } \n");
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder("")
            .Append($"Название: {Name} \n")
            .Append($"Дата публикации: {Date.ToShortDateString()} \n");

        return builder.ToString();
    }
}