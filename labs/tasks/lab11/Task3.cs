using System.Diagnostics;
using IO.requests;
using IO.responses;
using IO.utils;
using labs.builders;
using labs.entities;
using labs.lab10.src;
using labs.lab10.src.comparators;
using labs.lab10.src.utils;

namespace labs.lab11;

public class Task3 : LabTask
{
    private static Task3? instance;
    
    private List<Book> books;
    private List<string> strBooks;

    private Dictionary<Book, EducationalBook> dictBooks;
    private Dictionary<string, EducationalBook> dictStrBooks;
    
    public static Task3 GetInstance(string name, string description)
    {
        if (instance == null)
            instance = new Task3(name, description);

        return instance;
    }
    
    private Task3(string name, string description) : 
        base(name, description)
    {
        books = new List<Book>();
        strBooks = new List<string>();
        
        dictBooks = new Dictionary<Book, EducationalBook>(comparer: new DescribeEqualityComparer<Book>());
        dictStrBooks = new Dictionary<string, EducationalBook>();
        
        Actions = new List<ILabEntity<string>>
        {
            new LabTaskActionBuilder().Name("Автоматически заполнить")
                .ExecuteAction(InitCollections)
                .Build(),
            
            new LabTaskActionBuilder().Name("Добавить пару")
                .ExecuteAction(RequestBook)
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывести информацию по номеру")
                .ExecuteAction(OutputByNumber)
                .Build(),
            
            new LabTaskActionBuilder().Name("Поиск по ключу [ручной]")
                .ExecuteAction(SearchByKey)
                .Build(),
            
            new LabTaskActionBuilder().Name("Поиск по ключу [номер объекта]")
                .ExecuteAction(SearchByKeyByNumber)
                .Build(),
            
            new LabTaskActionBuilder().Name("Поиск по значению [ручной]")
                .ExecuteAction(SearchByValue)
                .Build(),
            
            new LabTaskActionBuilder().Name("Поиск по значению [номер объекта]")
                .ExecuteAction(SearchByValueByNumber)
                .Build(),
            
            new LabTaskActionBuilder().Name("Удалить издание по названию")
                .ExecuteAction(DeletePublication)
                .Build(),
            
            new LabTaskActionBuilder().Name("Очистить списки")
                .ExecuteAction(ClearAll)
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывод списка объектов")
                .ExecuteAction(OutputAll)
                .Build(),
        };
    }
    
    public void RequestBook()
    {
        ConsoleResponseData<Book> book;
        if (!(book = PublicationRequestFactory.MakeBook()).IsOk())
            return;
     
        Target.Output.WriteLine("\n------ Ввод значения ------\n");
        
        ConsoleResponseData<EducationalBook> eduBook;
        if(!(eduBook = PublicationRequestFactory.MakeEduBook()).IsOk())
            return;
        
        books.Add(book.Data());
        strBooks.Add(book.Data().ToString());
        
        dictBooks.Add(book.Data(), eduBook.Data());
        dictStrBooks.Add(book.Data().ToString(), eduBook.Data());
    }

    public void SearchByKey()
    {
        ConsoleResponseData<Book> book;
        if (!(book = PublicationRequestFactory.MakeBook()).IsOk())
            return;
        
        Target.Output.WriteLine();
        
        ContainsKey(book.Data());
    }
    
    public void SearchByValue()
    {
        ConsoleResponseData<EducationalBook> eduBook;
        if(!(eduBook = PublicationRequestFactory.MakeEduBook()).IsOk())
            return;
        
        Target.Output.WriteLine();
        
        ContainsValue(eduBook.Data());
    }

    public void SearchByValueByNumber()
    {
        ConsoleResponseData<int> number;
        
        if(!(number = MakeNumberRequest(1, books.Count)).IsOk())
            return;
        
        ContainsValue(dictBooks.Values.ToArray()[number.Data() - 1]);
    }
    
    public void SearchByKeyByNumber()
    {
        ConsoleResponseData<int> number;
        
        if(!(number = MakeNumberRequest(1, books.Count)).IsOk())
            return;
        
        ContainsKey(dictBooks.Keys.ToArray()[number.Data() - 1]);
    }
    
    public void DeletePublication()
    {
        ConsoleResponseData<string> name = 
            new ConsoleDataRequest<string>("Введите название печатного издания: ")
                .Request(BaseTypeDataConverterFactory.MakeSimpleStringConverter())
                .As<ConsoleResponseData<string>>();
        
        if(!name) return;

        int i = books.FindIndex((x) => x.Name == name.Data());
        
        if(i == -1)
            return;

        Book book = books[i];
        
        books.RemoveAt(i);
        strBooks.RemoveAt(i);

        dictBooks.Remove(book);
        dictStrBooks.Remove(book.ToString());
    }

    public void ClearAll()
    {
        books.Clear();
        strBooks.Clear();
        dictBooks.Clear();
        dictStrBooks.Clear();
    }

    public void OutputAll()
    {
        int i = 1;
        foreach (var keyValuePair in dictBooks)
            Target.Output.WriteLine($"#{i++}: {keyValuePair.Key.Describe()}\n\t\t{keyValuePair.Value.Describe()}\n");
        
        if(books.Count == 0)
            Target.Output.WriteLine("Список пуст");
    }

    void ContainsValue(EducationalBook value)
    {
        EducationalBook book = value;
        
        int i = Array.IndexOf(dictBooks.Values.ToArray(), value);

        if (i != -1)
            book = dictBooks.Values.ToArray()[i];
        
        bool contain;
        
        Stopwatch stopwatch = new Stopwatch();
        
        stopwatch.Start();
        contain = dictBooks.ContainsValue(book);
        stopwatch.Stop();
        
        Target.Output.WriteLine($"Dictionary<Book, EduBook> результат: {contain} => {stopwatch.ElapsedTicks} ticks");
        
        stopwatch.Start();
        contain = dictStrBooks.ContainsValue(book);
        stopwatch.Stop();
        
        Target.Output.WriteLine($"Dictionary<string, EduBook> результат: {contain} => {stopwatch.ElapsedTicks} ticks");
    }

    void ContainsKey(Book value)
    {
        Book book = value;

        int i = Array.IndexOf(books.ToArray(), value);

        if (i != -1)
            book = books[i];

        bool contain;
        
        Stopwatch stopwatch = new Stopwatch();
        
        stopwatch.Start();
        contain = books.Contains(book);
        stopwatch.Stop();
        
        Target.Output.WriteLine($"List<Book> результат: {contain} => {stopwatch.ElapsedTicks} ticks");
        
        stopwatch.Start();
        contain = strBooks.Contains(book.ToString());
        stopwatch.Stop();
        
        Target.Output.WriteLine($"List<string> результат: {contain} => {stopwatch.ElapsedTicks} ticks");

        stopwatch.Start();
        contain = dictBooks.ContainsKey(book);
        stopwatch.Stop();
        
        Target.Output.WriteLine($"Dictionary<Book, EduBook> результат: {contain} => {stopwatch.ElapsedTicks} ticks");
        
        stopwatch.Start();
        contain = dictStrBooks.ContainsKey(book.ToString());
        stopwatch.Stop();
        
        Target.Output.WriteLine($"Dictionary<string, EduBook> результат: {contain} => {stopwatch.ElapsedTicks} ticks");
    }

    void OutputByNumber()
    {
        ConsoleResponseData<int> number =
            MakeNumberRequest(1, books.Count);

        KeyValuePair<Book, EducationalBook> pair =
            dictBooks.ElementAt(number.Data() - 1);

        if(number) Target.Output.WriteLine($"#{number.Data()}: " +
                                           $"{pair.Key.Describe()}" +
                                           $"\n\t\t{pair.Value.Describe()}\n");
    }

    void InitCollections()
    {
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);

        Random random = new Random();
        int count = random.Next(500, 2000);

        books = new List<Book>();
        strBooks = new List<string>();

        dictBooks = new Dictionary<Book, EducationalBook>();
        dictStrBooks = new Dictionary<string, EducationalBook>();

        for (int i = 0; i < count; i++)
        {
            EducationalBook eduBook = new EducationalBook(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                date,
                new List<string> { Guid.NewGuid().ToString() }
            );

            Book book = eduBook.Base;

            if(!dictBooks.TryAdd(book, eduBook))
                continue;

            if (!dictStrBooks.TryAdd(book.ToString(), eduBook))
            {
                dictBooks.Remove(book);
                continue;
            }
            
            books.Add(book);
            strBooks.Add(book.ToString());
        }
    }

    private static ConsoleResponseData<int> MakeNumberRequest(int left, int right) =>
        new ConsoleDataRequest<int>($"Введите номер элемента [{left}; {right}]: ")
            .Request(BaseTypeDataConverterFactory.MakeSimpleIntConverter(),
                BaseComparableValidatorFactory.MakeInRangeNotStrictValidator(
                    left, right, "выход за допустимые границы"))
            .As<ConsoleResponseData<int>>();
}