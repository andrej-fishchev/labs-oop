using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using IO.targets;

namespace labs.lab10.src;

public class Book :
    Publication
{
    public IList<string> Authors { get; set; }

    public Book(string name, string date, IList<string>? authors = default) :
        base(name, date)
    {
        Authors = authors ?? new List<string>();
    }

    public static bool TryParse(string? data, out Book obj)
    {
        obj = new Book("", DateTime.Now.ToShortDateString());

        if (data == null)
            return false;
        
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
        };

        Book? buffer;
        if ((buffer = JsonSerializer.Deserialize<Book>(data, options)) == null)
            return false;

        obj = buffer;
        return true;
    }

    public override void Describe(TextWriter target)
    {
        StringBuilder builder = new StringBuilder("Книга \n");
        
        builder.Append($"Название: {Name} \n");
        builder.Append($"Дата: {Date} \n");
        builder.Append("Авторы: \n");

        foreach (var author in Authors)
            builder.Append($"- {author} \n");

        if (Authors.Count == 0)
            builder.Append("Неизвестно \n");
        
        target.Write(builder.ToString());
    }
}