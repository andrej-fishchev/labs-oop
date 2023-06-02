using labs.builders;
using labs.entities;
using labs.shared.data.abstracts;
using labs.shared.utils;
using labs.tasks.lab10.src;
using labs.tasks.lab10.src.requests;
using UserDataRequester.responses;

namespace labs.tasks.lab12;

public sealed class Task1 : LabTask
{
    private static Task1? instance;

    private Deque<Publication> Publications { get; }

    public static Task1 GetInstance(string name = "lab12.task1", string description = "var. 24")
        => instance ??= new Task1(name, description);

    private Task1(string name, string description) :
        base(name, description)
    {
        Publications = new Deque<Publication>();

        Actions = new List<ILabEntity<string>>
        {
            new LabTaskActionBuilder().Name("Добавить книгу")
                .ExecuteAction(() => GetAndSavePublication<Book>(Publications, PublicationRequester.GetBook))
                .Build(),
            
            new LabTaskActionBuilder().Name("Добавить журнал")
                .ExecuteAction(() => GetAndSavePublication<Journal>(
                    Publications, PublicationRequester.GetJournal
                ))
                .Build(),
            
            new LabTaskActionBuilder().Name("Добавить учебник")
                .ExecuteAction(() => GetAndSavePublication<EducationalBook>(
                    Publications, PublicationRequester.GetEduBook
                ))
                .Build(),

            new LabTaskActionBuilder().Name("Удалить публикацию по названию")
                .ExecuteAction(DeletePublicationByName)
                .Build(),

            new LabTaskActionBuilder().Name("Вывести все публикации")
                .ExecuteAction(() => EnumerableItemsPrinter.VerticallyWithNumbers(
                    Console.Out, Publications.GetEnumerator(), "Console| Список пуст")
                ).Build()
        };
    }
    
    public void DeletePublicationByName()
    {
        IResponsibleData<object> response;
        if (!(response = PublicationRequester.GetString(terminate: "...")).IsOk() 
            || response.Data() is not string value)
            return;
        
        try
        {
            Publications.Remove(Publications.Last(x => x.Name == value));
            Console.WriteLine($"CONSOLE| Последняя публикация (в списке) с названием {response.Data()} удалена");
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine($"CONSOLE| Публикаций с названием {response.Data()} не найдено");
        }
    }

    public void GetAndSavePublication<T>(
        ICollection<Publication> collection, Func<string, IResponsibleData<object>> func, string terminate = "..."
    ) where T : Publication
    {
        T? obj;
        if ((obj = PublicationRequester.GetRequest<T>(func, terminate)) != null)
            collection.Add(obj);

        if (obj != null)
            Console.WriteLine($"CONSOLE| Публикация '{obj.Name}' добавлена.");
    }
}