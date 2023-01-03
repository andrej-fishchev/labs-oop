using System.Text;

namespace labs.lab10.src;

public class Book :
    Publication
{
    public IList<string> Authors { get; set; }

    public Book(string name, DateOnly date, IList<string>? authors = default) :
        base(name, date)
    {
        Authors = authors ?? new List<string>();
    }

    public override void Describe(TextWriter target)
    {
        NoOverridingDescribe(target);
    }
    
#pragma warning disable CS0108
    public void NoOverridingDescribe(TextWriter t)
    {
        t.WriteLine($"Книга: \n{ToString()}");
    }
#pragma warning restore CS0108

    private string AuthorsToString()
    {
        StringBuilder builder = new StringBuilder("Авторы: \n");

        foreach (var author in Authors)
            builder.Append($"- {author} \n");

        if (Authors.Count == 0)
            builder.Append("Неизвестно \n");

        return builder.ToString();
    }

    public override string ToString()
    {
        return new StringBuilder(base.ToString())
            .Append(AuthorsToString())
            .ToString();
    }
}