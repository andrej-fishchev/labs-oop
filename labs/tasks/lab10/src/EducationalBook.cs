namespace labs.lab10.src;

public class EducationalBook :
    Book
{
    public EducationalBook(string name, string date, IList<string> authors) :
        base(name, date, authors)
    { }
}