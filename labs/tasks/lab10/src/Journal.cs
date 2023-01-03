using System.Text;

namespace labs.lab10.src;

public class Journal :
    Publication
{
    public string Period { get; set; }
    
    public Journal(string name, DateOnly dateTime, string period = "1 month") :
        base(name, dateTime)
    {
        Period = period;
    }
    
    public override void Describe(TextWriter target)
    {
        target.WriteLine($"Журнал: \n{ToString()}");
    }

    public override string ToString()
    {
        return new StringBuilder(base.ToString())
            .Append($"Периодичность выпуска: {Period} \n")
            .ToString();
    }
}