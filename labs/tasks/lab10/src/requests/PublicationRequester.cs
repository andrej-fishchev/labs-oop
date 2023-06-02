using System.Collections;
using UserDataRequester.converters.console.utils;
using UserDataRequester.converters.parsers;
using UserDataRequester.requests.console.utils;
using UserDataRequester.responses;
using UserDataRequester.responses.console;
using UserDataRequester.validators.console;

namespace labs.tasks.lab10.src.requests;

public static class PublicationRequester
{
    public static Publication? GetRequest<T>(Func<string, IResponsibleData<object>> func, string terminate = "...") 
        where T : Publication
    {
        Console.WriteLine($"\nВвод может быть прекращен в любой момент, используйте '{terminate}')");
        
        IResponsibleData<object> response;
        if(!(response = func.Invoke(terminate)).IsOk())
            return default;

        return response.Data() as T;
    }
    
    public static IResponsibleData<object> GetEduBook(string terminate = "...")
    {
        IResponsibleData<object> book = GetBook(terminate);

        if (!book.IsOk() || book.Data() is not Book buffer)
            return ConsoleResponseDataFactory.MakeResponse<object>();

        IResponsibleData<object> response;
        if (!(response = GetString("Введите предмет: ", terminate: terminate)).IsOk() 
            || response.Data() is not string subject)
            return ConsoleResponseDataFactory.MakeResponse<object>();

        return ConsoleResponseDataFactory.MakeResponse<object>(
            new EducationalBook(
                buffer.Name,
                subject,
                buffer.Date,
                buffer.Authors
            )
        );
    }
    
    public static IResponsibleData<object> GetJournal(string terminate = "...")
    {
        IResponsibleData<object> book = GetBook(terminate);

        if (!book.IsOk() || book.Data() is not Book buffer)
            return ConsoleResponseDataFactory.MakeResponse<object>();

        IResponsibleData<object> response;
        if (!(response = GetString("Введите периодичность: ", terminate: terminate)).IsOk() 
            || response.Data() is not string period)
            return ConsoleResponseDataFactory.MakeResponse<object>();

        return ConsoleResponseDataFactory.MakeResponse<object>(
            new Journal(
                buffer.Name,
                period,
                buffer.Date,
                buffer.Authors
            )
        );
    }
    
    public static IResponsibleData<object> GetBook(string terminate = "...")
    {
        IResponsibleData<object> response;
        if (!(response = GetString(terminate: terminate)).IsOk() || response.Data() is not string name)
            return response;

        if (!(response = GetDate(terminate: terminate)).IsOk() || response.Data() is not DateOnly date)
        {
            response.As<ConsoleResponseData<object>>().Data(default);
            return response;
        }

        if (!(response = GetStringList(terminate: terminate)).IsOk())
            return response;

        List<string> authors = (response.Data() as ArrayList)!.ToArray()
            .Where(x => x is string)
            .Select(x => (x as string)!)
            .ToList();

        return ConsoleResponseDataFactory.MakeResponse<object>(new Book(name, date, authors));
    }

    public static IResponsibleData<object> GetString(
        string message = "Введите название: ", 
        string? terminate = "..."
    ) => BaseDataTypeRequester.RequestString(
            message,
            new ConsoleDataChainedValidator()
                .And(data => data != null)
                .And(data => data is string { Length: > 0 }),
            terminate
    );

    public static IResponsibleData<object> GetDate(
        string message = "Введите дату публикации: ", 
        string? terminate = "..."
    ) => ConsoleBaseRequester.RepeatableGetApprovedData(
            message,
            BaseDataConverterFactory.MakeObjectConverter(
                BaseParser.TryParseSignature<DateOnly>()
            ),
            null,
            terminate
    );
    
    public static IResponsibleData<object> GetStringList(
        string message = "Перечислите авторов через запятую (минимум 1): ", 
        string delimiter = ",", 
        string? terminate = "..."
    ) => ConsoleBaseRequester.RepeatableGetApprovedData(
        message,
        BaseArrayDataConverterFactory.MakeStringArrayConverter(
            new ConsoleDataChainedValidator()
                .And(data => data != null)
                .And(data => data is string { Length: > 0 }),
            delimiter
        ),
        new ConsoleDataValidator((data) => data is ArrayList {Count: > 0}),
        terminate
    );
}