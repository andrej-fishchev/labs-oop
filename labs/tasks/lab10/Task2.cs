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
        private init;
    }
    
    public Task2(string name = "lab10.task1", string description = "") :
        base(2, name, description)
    {
        Publications = new List<Publication>();
        
        Actions = new List<ILabEntity<int>>
        {
            new LabTaskActionBuilder().Name("Добавить книгу")
                .ExecuteAction(AddBook)
                .Build(),
            
            new LabTaskActionBuilder().Name("Добавить журнал")
                .ExecuteAction(AddJournal)
                .Build(),
            
            new LabTaskActionBuilder().Name("Добавить журнал")
                .ExecuteAction(() => {})
                .Build()
            
        };
    }

    public void AddBook()
    {
        (string Name, string Date)? publication = RequestPublication();
        
        if(publication == null)
            return;

        ConsoleArrayDataConverter<string> converter = ConsoleDataConverterFactory.MakeArrayConverter(
            BaseTypeDataConverterFactory.MakeSimpleStringConverter(), new ConsoleDataValidator<string>(
                data => data.Trim().Length > 0, "значение не может быть пустым"));

        ConsoleDataValidator<string[]> validator = new ConsoleDataValidator<string[]>(
            data => data.Length > 0, "список авторов не может быть пустым");

        ConsoleResponseData<string[]> authors = 
            new ConsoleDataRequest<string[]>($"Ввведите авторов (через '{converter.Delimiter}'): ")
                .Request(converter, validator, false)
                .As<ConsoleResponseData<string[]>>();
        
        if(authors) Publications
            .Add(new Book(publication.Value.Name, publication.Value.Date, authors.Data()));
    }

    public void AddJournal()
    {
       
    }

    public (string name, string date)? RequestPublication()
    {
        ConsoleSimpleDataConverter<string> converter = BaseTypeDataConverterFactory
            .MakeSimpleStringConverter();

        ConsoleDataValidator<string> validator = new ConsoleDataValidator<string>(
            data => data.Trim().Length > 0,
            "значение не может быть пустым"
        );

        ConsoleResponseData<string> name = new ConsoleDataRequest<string>("Введите название: ")
            .Request(converter, validator)
            .As<ConsoleResponseData<string>>();

        if (!name) return null;

        ConsoleResponseData<string> date = new ConsoleDataRequest<string>("Введите дату публикации: ")
            .Request(converter, validator, false)
            .As<ConsoleResponseData<string>>();

        if (!date) return null;

        return (name.Data(), date.Data());
    }
}