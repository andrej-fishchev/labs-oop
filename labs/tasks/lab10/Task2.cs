using IO.requests;
using IO.responses;
using IO.utils;
using IO.validators;
using labs.builders;
using labs.entities;
using labs.lab10.src;
using labs.lab10.src.utils;

namespace labs.lab10;

public sealed class Task2 : LabTask
{
    private static Task2? instance;
    
    public List<IDescribe> Describes
    {
        get;
        private set;
    }
    
    public static Task2 GetInstance(string name, string description)
    {
        if (instance == null)
            instance = new Task2(name, description);

        return instance;
    }
    
    private Task2(string name, string description) :
        base(name, description)
    {
        Describes = new List<IDescribe>();
        
        Actions = new List<ILabEntity<string>>
        {
            new LabTaskActionBuilder().Name("Добавить книгу")
                .ExecuteAction(RequestBook)
                .Build(),
            
            new LabTaskActionBuilder().Name("Добавить журнал")
                .ExecuteAction(RequestJournal)
                .Build(),
            
            new LabTaskActionBuilder().Name("Добавить учебник")
                .ExecuteAction(RequestEduBook)
                .Build(),
            
            new LabTaskActionBuilder().Name("Добавить студента")
                .ExecuteAction(RequestStudent)
                .Build(),
            
            new LabTaskActionBuilder().Name("Отсортировать элементы")
                .ExecuteAction(SortElements)
                .Build(),
            
            new LabTaskActionBuilder().Name("Бинарный поиск по книгам")
                .ExecuteAction(BinarySearch)
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
            
            new LabTaskActionBuilder().Name("Вывести все публикации")
                .ExecuteAction(() => OutputDescribe(Describes.Where(x => x is Publication).ToList()))
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывести все")
                .ExecuteAction(() => OutputDescribe(Describes.ToList()))
                .Build()
        };
    }

    public void RequestBook()
    {
        ConsoleResponseData<Book> book;
        if((book = PublicationRequestFactory.MakeBook()).IsOk())
            AddDescribe(book.Data());
    }
    
    public void RequestEduBook()
    {
        ConsoleResponseData<EducationalBook> book;
        if((book = PublicationRequestFactory.MakeEduBook()).IsOk())
            AddDescribe(book.Data());
    }

    public void RequestJournal()
    {
        ConsoleResponseData<Journal> journal;
        if((journal = PublicationRequestFactory.MakeJournal()).IsOk())
            AddDescribe(journal.Data());
    }

    public void RequestStudent()
    {
        const int minAge = 17;
        const int maxAge = 100;
        
        ConsoleResponseData<string> name = new ConsoleDataRequest<string>("Введите имя студента: ")
            .Request(BaseTypeDataConverterFactory.MakeSimpleStringConverter(),
                (ConsoleDataValidator<string>) (data => data.Length > 0, "значение не может быть пустым"))
            .As<ConsoleResponseData<string>>();

        if (!name) return;

        ConsoleResponseData<int> age = new ConsoleDataRequest<int>("Введите возраст (в годах): ")
            .Request(BaseTypeDataConverterFactory.MakeSimpleIntConverter(),
                BaseComparableValidatorFactory
                    .MakeInRangeStrictValidator(minAge, maxAge, "ожидалось значение от 18 до 99 лет"))
            .As<ConsoleResponseData<int>>();
        
        if(!age) return;
        
        AddDescribe(new Student(name.Data(), age.Data()));
    }
    
    public void AddDescribe(IDescribe publication)
    {
        Describes.Add(publication);
        Target.Output.WriteLine("Печатное издание добавлено");
    }

    public void BinarySearch()
    {
        if(lab4.Task1.IsValueZeroWithNotify(Describes.Count, 
               "Список объектов пуст"))
            return;
        
        ConsoleResponseData<Book> book = PublicationRequestFactory.MakeBook();
        
        if(!book) return;

        int i = Array.BinarySearch(Describes.ToArray(), book.Data());

        if (i > -1)
            Target.Output.WriteLine($"Книга: {book.Data().Name} найдена, ее номер - {i + 1}");
    }

    public void SortElements()
    {
        IDescribe[] array = Describes.ToArray();
        
        Array.Sort(array, (obj1, obj2) => 
            string.Compare(obj1.Describe(), obj2.Describe(), StringComparison.Ordinal));

        Describes = array.ToList();
        
        OutputDescribe(Describes);
    }

    public void DisplayAllBooksWhereDateLessThanUserDate()
    {
        ConsoleResponseData<DateOnly> date;
        if(!(date = PublicationRequestFactory.RequestPublicationDate())) 
            return;

        OutputDescribe(
            Describes
                .Where((x) => x is Book book && book.Date < date.Data())
                .ToList()
        );   
    }

    public void DisplayJournals() =>
        OutputDescribe(Describes.Where(x => x is Journal).ToList());

    public void DisplayBooksByAuthors()
    {
        ConsoleResponseData<string[]> authors;
        
        if(!(authors = PublicationRequestFactory.RequestAuthors()))
            return;
        
        OutputDescribe(Describes
            .Where(x => x is Book book && authors.Data().All(y => book.Authors.Contains(y)))
            .ToList());
    }
    
    public void DeletePublicationByName()
    {
        if(lab4.Task1.IsValueZeroWithNotify(Describes.Count, 
               "Список объектов пуст"))
            return;
        
        ConsoleResponseData<string> name;
        if ((name = PublicationRequestFactory.RequestPublicationName()).IsOk()) 
            Describes = Describes
                .Where(x => x is Publication publication && !publication.Name.Equals(name.Data()))
                .ToList();
    }

    public void OutputDescribe(IList<IDescribe> describes)
    {
        for (var i = 0; i < describes.Count; i++)
            Target.Output.WriteLine($"{i+1}: { describes[i].Describe() }");

        if(describes.Count == 0)
            Target.Output.WriteLine("Список пуст");   
    }
}