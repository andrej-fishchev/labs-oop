using IO.responses;
using labs.builders;
using labs.entities;
using labs.lab10.src;
using labs.lab10.src.utils;

namespace labs.lab10;

public sealed class Task2 : LabTask
{
    public IList<Publication> Publications
    {
        get;
        private set;
    }
    
    public Task2(string name = "lab10.task1", string description = "") :
        base(2, name, description)
    {
        Publications = new List<Publication>();
        
        Actions = new List<ILabEntity<int>>
        {
            new LabTaskActionBuilder().Name("Добавить книгу")
                .ExecuteAction(RequestBook)
                .Build(),
            
            new LabTaskActionBuilder().Name("Добавить журнал")
                .ExecuteAction(RequestJournal)
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывести книги, опубликованные ранее заданной даты")
                .ExecuteAction(DisplayAllBooksWhereDateLessThanUserDate)
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывести все журналы")
                .ExecuteAction(DisplayJournals)
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывести книги авторов")
                .ExecuteAction(DisplayBooksByAuthors)
                .Build(),
            
            new LabTaskActionBuilder().Name("Удалить издания по названию")
                .ExecuteAction(DeletePublicationByName)
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывести список печатных изданий")
                .ExecuteAction(() => OutputPublications(Publications))
                .Build()
        };
    }

    public void RequestBook()
    {
        ConsoleResponseData<Book> book;
        if((book = PublicationRequestFactory.MakeBook()).IsOk())
            AddPublication(book.Data());
    }

    public void RequestJournal()
    {
        ConsoleResponseData<Journal> journal;
        if((journal = PublicationRequestFactory.MakeJournal()).IsOk())
            AddPublication(journal.Data());
    }
    
    public void AddPublication(Publication publication)
    {
        Publications.Add(publication);
        Target.Output.WriteLine("Печатное издание добавлено");
    }

    public void DisplayAllBooksWhereDateLessThanUserDate()
    {
        ConsoleResponseData<DateOnly> date;
        if(!(date = PublicationRequestFactory.RequestPublicationDate())) 
            return;

        OutputPublications(
            Publications.Where((x) => x is Book && x.Date < date.Data()).ToList()
        );   
    }

    public void DisplayJournals() =>
        OutputPublications(Publications.Where(x => x is Journal).ToList());

    public void DisplayBooksByAuthors()
    {
        ConsoleResponseData<string[]> authors;
        
        if(!(authors = PublicationRequestFactory.RequestAuthors()))
            return;
        
        OutputPublications(Publications
            .Where(x => x is Book book && authors.Data().All(y => book.Authors.Contains(y)))
            .ToList());
    }
    
    public void DeletePublicationByName()
    {
        ConsoleResponseData<string> name;
        if ((name = PublicationRequestFactory.RequestPublicationName()).IsOk()) 
            Publications = Publications.Where(x => !x.Name.Equals(name.Data())).ToList();
    }
    
    public void OutputPublications(IList<Publication> publications)
    {
        for (var i = 0; i < publications.Count; i++)
        {
            Target.Output.Write($"{i+1}: ");
            publications[i].Describe(Target.Output);
        }
        
        if(publications.Count == 0)
            Target.Output.WriteLine("Список пуст");
    }
}