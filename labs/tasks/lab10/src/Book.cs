namespace labs.tasks.lab10.src;

public class Book :
    Publication,
    ICloneable,
    IEquatable<Book>
{
    public Book(string name, DateOnly date, IList<string>? authors = default) :
        base(name, date, authors)
    { }

    public override void Describe(TextWriter target)
    {
        NoOverridingDescribe(target);
    }
    
#pragma warning disable CS0108
    public void NoOverridingDescribe(TextWriter t)
    {
        t.WriteLine(ToString());
    }
#pragma warning restore CS0108

    public override string ToString()
    {
        return "Книга: \n" +
               $"Название: {Name} \n" +
               $"Дата публикации: {Date.ToShortDateString()} \n" +
               $"Авторы: \n{AuthorsToString()}";
    }

    public object Clone()
    {
        return new Book(Name, Date, Authors);
    }

    public bool Equals(Book? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        return ToString().Equals(other.ToString());
    }
}