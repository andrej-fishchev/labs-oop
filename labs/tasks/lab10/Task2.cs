using IO.converters;
using IO.requests;
using IO.responses;
using IO.utils;
using IO.validators;
using labs.builders;
using labs.entities;
using labs.lab10.src;

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
        (string Name, DateOnly Date)? publication = RequestPublication();
        
        if(publication == null)
            return;

        ConsoleResponseData<string[]> authors = RequestAuthors(false);

        if (authors) AddPublication(new Book(publication.Value.Name, publication.Value.Date, authors.Data()));
    }

    public void RequestJournal()
    {
        (string Name, DateOnly Date)? publication = RequestPublication();
        
        if(publication == null)
            return;

        ConsoleResponseData<string> period = new ConsoleDataRequest<string>("Введите периодичность выпуска: ")
            .Request(BaseTypeDataConverterFactory.MakeSimpleStringConverter(), sendRejectMessage: false)
            .As<ConsoleResponseData<string>>();
        
        if(period) AddPublication(new Journal(publication.Value.Name, publication.Value.Date, period.Data()));
    }
    
    public (string name, DateOnly date)? RequestPublication()
    {
        ConsoleResponseData<string> name = RequestName();

        if (!name) return null;

        ConsoleResponseData<DateOnly> date = new ConsoleDataRequest<DateOnly>("Введите дату публикации: ")
            .Request(MakeDateOnlyConverter(), sendRejectMessage: false)
            .As<ConsoleResponseData<DateOnly>>();

        if (!date) return null;

        return (name.Data(), date.Data());
    }
    
    public void AddPublication(Publication publication)
    {
        Publications.Add(publication);
        Target.Output.WriteLine("Печатное издание добавлено");
    }

    public void DisplayAllBooksWhereDateLessThanUserDate()
    {
        ConsoleResponseData<DateOnly> date = new ConsoleDataRequest<DateOnly>("Введите дату: ")
            .Request(MakeDateOnlyConverter(), sendRejectMessage: false)
            .As<ConsoleResponseData<DateOnly>>();
        
        if(!date) return;

        OutputPublications(
            Publications.Where((x) => x is Book && x.Date < date.Data()).ToList()
        );   
    }

    public void DisplayJournals() =>
        OutputPublications(Publications.Where(x => x is Journal).ToList());

    public void DisplayBooksByAuthors()
    {
        ConsoleResponseData<string[]> authors;
        
        if(!(authors = RequestAuthors()))
            return;
        
        OutputPublications(Publications
            .Where(x => x is Book book && authors.Data().All(y => book.Authors.Contains(y)))
            .ToList());
    }
    
    public void DeletePublicationByName()
    {
        ConsoleResponseData<string> name = RequestName();

        if (name)
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

    private static ConsoleResponseData<string> RequestName(bool send = true)
    {
        ConsoleSimpleDataConverter<string> converter = BaseTypeDataConverterFactory
            .MakeSimpleStringConverter();

        ConsoleDataValidator<string> validator = new ConsoleDataValidator<string>(
            data => data.Trim().Length > 0,
            "значение не может быть пустым"
        );

        return new ConsoleDataRequest<string>("Введите название: ")
            .Request(converter, validator, send)
            .As<ConsoleResponseData<string>>();
    }

    private static ConsoleResponseData<string[]> RequestAuthors(bool send = true)
    {
        ConsoleArrayDataConverter<string> converter = ConsoleDataConverterFactory.MakeArrayConverter(
            BaseTypeDataConverterFactory.MakeSimpleStringConverter(), new ConsoleDataValidator<string>(
                data => data.Trim().Length > 0, "значение не может быть пустым"));
        
        ConsoleDataValidator<string[]> validator = new ConsoleDataValidator<string[]>(
            data => data.Length > 0, "список авторов не может быть пустым");

        return new ConsoleDataRequest<string[]>($"Ввведите авторов (через '{converter.Delimiter}'): ")
                .Request(converter, validator, send)
                .As<ConsoleResponseData<string[]>>();
    }

    private static ConsoleSimpleDataConverter<DateOnly> MakeDateOnlyConverter()
    {
        return ConsoleDataConverterFactory.MakeSimpleConverter<DateOnly>(DateOnly.TryParse);
    }
}