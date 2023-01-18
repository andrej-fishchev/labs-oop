namespace labs.lab10.src;

public class EducationalBook :
    Book,
    ICloneable,
    IEquatable<EducationalBook>
{
    private string subject;
    
    public string Subject
    {
        get => subject;
        set
        {
            if (value.Length == 0)
                throw new ArgumentException();

            subject = value;
        }
    }

    public Book Base => new(Name, Date, Authors);

#pragma warning disable CS8618
    public EducationalBook(string name, string subject, DateOnly date, IList<string> authors) :
        base(name, date, authors)
    {
        Subject = subject;
    }
#pragma warning restore CS8618
    
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
        return "Учебник: \n" +
               $"Название: {Name} \n" +
               $"Предмет: {Subject} \n" +
               $"Дата публикации: {Date.ToShortDateString()} \n" +
               $"Авторы: \n{AuthorsToString()}";
    }

    public new object Clone()
    {
        return new EducationalBook(Name, Subject, Date, Authors);
    }

    public bool Equals(EducationalBook? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        return ToString().Equals(other.ToString());
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        
        if (obj.GetType() != GetType()) return false;
     
        return Equals((EducationalBook)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), subject);
    }
}