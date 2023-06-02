using System.Text.RegularExpressions;
using labs.builders;
using labs.entities;
using labs.shared.data.abstracts;
using labs.shared.requests;
using labs.shared.utils;
using labs.tasks.lab10.src;
using labs.tasks.lab10.src.requests;
using UserDataRequester.responses;

namespace labs.tasks.lab14;

public sealed class Task1 : LabTask
{
    private static Task1? instance;

    private Deque<IEnumerable<Publication>> PublicationCollections { get; }
    
    public static Task1 GetInstance(string name = "lab13.task1", string description = "var. 24")
        => instance ??= new Task1(name, description);

    private Task1(string name, string description) :
        base(name, description)
    {
        PublicationCollections = new Deque<IEnumerable<Publication>>
        {
            new Stack<Publication>(),
            new List<Publication>()
        };

        Actions = new List<ILabEntity<string>>
        {
            new LabTaskActionBuilder().Name("Добавить книгу")
                .ExecuteAction(() => GetAndSavePublication<Book>(PublicationRequester.GetBook))
                .Build(),
            
            new LabTaskActionBuilder().Name("Добавить журнал")
                .ExecuteAction(() => GetAndSavePublication<Journal>(PublicationRequester.GetJournal))
                .Build(),
            
            new LabTaskActionBuilder().Name("Добавить учебник")
                .ExecuteAction(() => GetAndSavePublication<EducationalBook>(PublicationRequester.GetEduBook))
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывод всех публикаций по типу")
                .ExecuteAction(OutputAllPublicationsByType)
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывод всех публикаций позднее")
                .ExecuteAction(OutputAllPublicationsByDateGreaterThan)
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывод элементов коллекций")
                .ExecuteAction(() => Print(PublicationCollections))
                .Build()
        };
    }

    private void GetAndSavePublication<T>(Func<string, IResponsibleData<object>> func, string terminate = "...")
        where T : Publication
    {
        var collectionsName = CreateCollectionNameList(PublicationCollections).ToList();
        
        EnumerableItemsPrinter.VerticallyWithNumbers(
            Console.Out, collectionsName.GetEnumerator(), ""
        );

        var number = SimpleRequests.GetValueInRangeNotStrict(
            1, PublicationCollections.Count, "Выберите номер коллекции для записи: "
        );
        
        if(number is not { } value)
            return;

        Publication? obj;
        if ((obj = PublicationRequester.GetRequest<T>(func, terminate)) != null)
            GetCollectionAddItemSignature(PublicationCollections[value - 1])?.Invoke(obj);
        
        if(obj != null)
            Console.WriteLine($"Публикация '{obj.Name}' добавлена в '{collectionsName[value - 1]}'.");
    }

    public void OutputAllPublicationsByType()
    {
        EnumerableItemsPrinter.VerticallyWithNumbers(
            Console.Out, PublicationTypes.GetEnumerator(), ""
        );
        
        var number = SimpleRequests.GetValueInRangeNotStrict(
            1, PublicationTypes.Count, "Выберите тип публикации: "
        );
        
        if(number is not { } value)
            return;

        Console.WriteLine();
        
        var type = PublicationTypeNameRelations(PublicationTypes[value - 1]);

        EnumerableItemsPrinter.VerticallyWithNumbers(
            Console.Out, 
            PublicationCollections
                .SelectMany(x => x)
                .Where(x => x.GetType() == type)
                .GetEnumerator(), 
            $"Публикаций типа '{PublicationTypes[value - 1]}' не существует"
        );
    }

    public void OutputAllPublicationsByDateGreaterThan()
    {
        var number = SimpleRequests.GetDate(
            "Введите дату, позднее которой необходимо отобрать публикации:\n> "
        );
        
        if(number is not { } value)
            return;

        Console.WriteLine();

        EnumerableItemsPrinter.VerticallyWithNumbers(
            Console.Out, 
            PublicationCollections
                .SelectMany(x => x)
                .Where(x => x.Date > value)
                .GetEnumerator(), 
            $"Публикаций позднее '{value}' не найдено"
        );
    }

    private static void Print(Deque<IEnumerable<Publication>> deque)
    {
        Console.WriteLine();
        
        var collectionsName = CreateCollectionNameList(deque).ToList();

        for (int j = 0; j < deque.Count; j++)
        {
            Console.WriteLine("Коллекция: {0}", collectionsName[j]);
            
            EnumerableItemsPrinter.VerticallyWithNumbers(
                Console.Out, deque[j].GetEnumerator(), "Коллекция не содержит элементов"
            );
            
            Console.WriteLine();
        }
    }

    private static Action<T>? GetCollectionAddItemSignature<T>(IEnumerable<T> enumerable)
    {
        return enumerable switch
        {
            ICollection<T> collection => collection.Add,
            Stack<T> stack => stack.Push,
            Queue<T> queue => queue.Enqueue,
            _ => null
        };
    }

    private static Type? PublicationTypeNameRelations(string value)
    {
        return value switch
        {
            "Книга" => typeof(Book),
            "Журнал" => typeof(Journal),
            "Учебник" => typeof(EducationalBook),
            
            _ => null
        };
    }

    private static IList<string> PublicationTypes => new List<string>
    {
        "Книга", "Журнал", "Учебник"
    };

    private static IEnumerable<string> CreateCollectionNameList<T>(IEnumerable<IEnumerable<T>> list) =>
        list.Select(y => Regex.Replace(y.GetType().Name, "[0-9'`]", ""));
}