using System.Text;

namespace labs.lab10.src;

public class Journal :
    Publication,
    ICloneable
{
    public string Period { get; set; }
    
    public Journal(string name, string period, DateOnly dateTime, IList<string> authors) :
        base(name, dateTime, authors)
    {
        Period = period;
    }
    
    public override void Describe(TextWriter target)
    {
        target.WriteLine(ToString());
    }

    public override string ToString()
    {
        return new StringBuilder("Журнал: \n")
            .Append($"Название: {Name} \n")
            .Append($"Дата публикации: {Date.ToShortDateString()} \n")
            .Append($"Периодичность выпуска: {Period} \n")
            .Append($"Авторы: \n{AuthorsToString()}")
            .ToString();
    }

    public object Clone()
    {
        return new Journal(Name, Period, Date, Authors);
    }
}