using System.Text;

namespace labs.tasks.lab10.src;

public abstract class Publication :
    IDescribe,
    IEquatable<Publication>
{
    public string Name { get; set; }
    
    public DateOnly Date { get; set; }

    public IList<string> Authors { get; set; }
    
    protected Publication(string name, DateOnly date, IList<string>? authors = default)
    {
        Name = name;
        Date = date;
        Authors = authors ?? new List<string>();
    }

    public virtual void Describe(TextWriter target) => 
        target.WriteLine(ToString());

    public void NoOverridingDescribe(TextWriter target) => 
        target.WriteLine(new StringBuilder("Печатное издание: \n")
            .Append($"Название: {Name} \n")
            .Append($"Дата публикации: {Date.ToShortDateString()} \n")
            .Append($"Авторы: \n{AuthorsToString()}")
            .ToString());

    protected virtual string AuthorsToString()
    {
        StringBuilder builder = new StringBuilder();

        foreach (var author in Authors)
            builder.Append($"- {author}\n");

        if (Authors.Count == 0)
            builder.Append("- Список пуст\n");

        return builder.ToString();
    }

    public override string ToString() =>
        new StringBuilder("Печатное издание: \n")
            .Append($"Название: {Name} \n")
            .Append($"Дата публикации: {Date.ToShortDateString()} \n")
            .Append($"Авторы: \n{AuthorsToString()}")
            .ToString();

    public string Describe() => ToString();

    public bool Equals(Publication? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        
        return Name == other.Name 
               && Date.ToShortDateString().Equals(other.Date.ToShortDateString()) 
               && Authors.Equals(other.Authors);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        
        return Equals((Publication)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Date, Authors);
    }
}