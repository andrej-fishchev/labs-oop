namespace labs.lab10.src;

public class EducationalBook :
    Book
{
    public EducationalBook(string name, DateOnly date, IList<string> authors) :
        base(name, date, authors)
    { }
}