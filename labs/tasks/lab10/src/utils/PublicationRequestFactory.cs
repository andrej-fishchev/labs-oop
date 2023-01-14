using IO.converters;
using IO.requests;
using IO.responses;
using IO.utils;
using IO.validators;

namespace labs.lab10.src.utils;

public static class PublicationRequestFactory
{
    public static ConsoleResponseData<Book> MakeBook()
    {
        (ConsoleResponseData<string> name, ConsoleResponseData<DateOnly> date)  
            publication = RequestPublication();

        if (!IsPublicationOk(publication))
            return new ConsoleResponseData<Book>(error: publication.date.Error(), code: publication.date.Code());

        ConsoleResponseData<string[]> authors = RequestAuthors(false);

        if (!authors)
            return new ConsoleResponseData<Book>(error: authors.Error(), code: authors.Code());

        return new ConsoleResponseData<Book>(new Book(
            publication.name.Data(), 
            publication.date.Data(), 
            authors.Data()
        ));
    }

    public static ConsoleResponseData<Journal> MakeJournal()
    {
        (ConsoleResponseData<string> name, ConsoleResponseData<DateOnly> date)  
            publication = RequestPublication();

        if (!IsPublicationOk(publication))
            return new ConsoleResponseData<Journal>(error: publication.date.Error(), code: publication.date.Code());

        ConsoleResponseData<string> period = RequestJournalPeriod();

        if (period) return new ConsoleResponseData<Journal>(new Journal(
            publication.name.Data(), 
            publication.date.Data(), 
            period.Data()
        ));

        return new ConsoleResponseData<Journal>(error: period.Error(), code: period.Code());
    }

    private static bool IsPublicationOk(
        (ConsoleResponseData<string> name, ConsoleResponseData<DateOnly> date) publication) => 
        publication.name && publication.date;

    private static (ConsoleResponseData<string> name, ConsoleResponseData<DateOnly> date) 
        RequestPublication() => (RequestPublicationName(), RequestPublicationDate(false));

    public static ConsoleResponseData<string> RequestPublicationName(bool send = true) =>
        RequestStringAttribute("Введите название: ", send: send);

    public static ConsoleResponseData<string> RequestJournalPeriod(bool send = true) =>
        RequestStringAttribute("Введите периодичность выпуска: ", send: send);

    public static ConsoleResponseData<DateOnly> RequestPublicationDate(bool send = true)
        => new ConsoleDataRequest<DateOnly>("Введите дату публикации: ")
                .Request(ConsoleDataConverterFactory
                    .MakeSimpleConverter<DateOnly>(DateOnly.TryParse), sendRejectMessage: send)
                .As<ConsoleResponseData<DateOnly>>();

    public static ConsoleResponseData<string[]> RequestAuthors(bool send = true)
    {
        ConsoleArrayDataConverter<string> converter = ConsoleDataConverterFactory.MakeArrayConverter(
            BaseTypeDataConverterFactory.MakeSimpleStringConverter(), new ConsoleDataValidator<string>(
                data => data.Trim().Length > 0, "значение не может быть пустым"));
        
        ConsoleDataValidator<string[]> validator = 
            (data => data.Length > 0, "список авторов не может быть пустым");

        return new ConsoleDataRequest<string[]>($"Ввведите авторов (через '{converter.Delimiter}'): ")
            .Request(converter, validator, send)
            .As<ConsoleResponseData<string[]>>();
    }

    private static ConsoleResponseData<string> RequestStringAttribute(string msg, string rejectKey = "...", bool send = true)
        => new ConsoleDataRequest<string>(msg, rejectKey)
            .Request(BaseTypeDataConverterFactory.MakeSimpleStringConverter(),
                new ConsoleDataValidator<string>(data => data.Trim().Length > 0, 
                    "значение не может быть пустым"), 
                send)
            .As<ConsoleResponseData<string>>();
}