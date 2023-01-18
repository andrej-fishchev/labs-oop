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
        (ConsoleResponseData<string> name, ConsoleResponseData<DateOnly> date, ConsoleResponseData<string[]> authors)?  
            publication = RequestPublication();

        if (publication == null)
            return new ConsoleResponseData<Book>(code: ConsoleResponseDataCode.ConsoleInputRejected);

        return new ConsoleResponseData<Book>(new Book(
            publication.Value.name.Data(), 
            publication.Value.date.Data(), 
            publication.Value.authors.Data().ToList()
        ));
    }
    
    public static ConsoleResponseData<EducationalBook> MakeEduBook()
    {
        (ConsoleResponseData<string> name, ConsoleResponseData<DateOnly> date, ConsoleResponseData<string[]> authors)?  
            publication = RequestPublication();
        
        if (publication == null)
            return new ConsoleResponseData<EducationalBook>(code: ConsoleResponseDataCode.ConsoleInputRejected);

        ConsoleResponseData<string> subject = RequestEduBookSubject(send: false);

        if (subject) return new ConsoleResponseData<EducationalBook>(new EducationalBook(
            publication.Value.name.Data(),
            subject.Data(),
            publication.Value.date.Data(),
            publication.Value.authors.Data().ToList()
        ));

        return new ConsoleResponseData<EducationalBook>(error: subject.Error(), code: subject.Code());
    }

    public static ConsoleResponseData<Journal> MakeJournal()
    {
        (ConsoleResponseData<string> name, ConsoleResponseData<DateOnly> date, ConsoleResponseData<string[]> authors)?  
            publication = RequestPublication();

        if (publication == null)
            return new ConsoleResponseData<Journal>(code: ConsoleResponseDataCode.ConsoleInputRejected);

        ConsoleResponseData<string> period = RequestJournalPeriod(send: false);

        if (period) return new ConsoleResponseData<Journal>(new Journal(
            publication.Value.name.Data(),
            period.Data(),
            publication.Value.date.Data(), 
            publication.Value.authors.Data().ToList()
        ));

        return new ConsoleResponseData<Journal>(error: period.Error(), code: period.Code());
    }

    private static (ConsoleResponseData<string> name,
        ConsoleResponseData<DateOnly> date,
        ConsoleResponseData<string[]> authors)? RequestPublication()
    {
        ConsoleResponseData<string> name = RequestPublicationName();

        if (!name) return null;

        ConsoleResponseData<DateOnly> date = RequestPublicationDate(false);

        if (!date) return null;

        ConsoleResponseData<string[]> authors = RequestAuthors(false);

        if (!authors) return null;
        
        return (name, date, authors);
    }
    
    public static ConsoleResponseData<string> RequestPublicationName(bool send = true) =>
        RequestStringAttribute("Введите название: ", send: send);

    public static ConsoleResponseData<string> RequestJournalPeriod(bool send = true) =>
        RequestStringAttribute("Введите периодичность выпуска: ", send: send);

    public static ConsoleResponseData<string> RequestEduBookSubject(bool send = true) =>
        RequestStringAttribute("Введите предмет учебника: ", send: send);
    
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