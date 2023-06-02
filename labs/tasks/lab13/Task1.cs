using System.Text;
using labs.builders;
using labs.entities;
using labs.shared.utils;
using labs.tasks.lab10.src;
using labs.tasks.lab10.src.requests;
using labs.tasks.lab13.src;
using UserDataRequester.responses;

namespace labs.tasks.lab13;

public sealed class Task1 : LabTask
{
    private static Task1? instance;

    private EventDeque<Publication> Publications { get; }

    public static Task1 GetInstance(string name = "lab13.task1", string description = "var. 24")
        => instance ??= new Task1(name, description);

    private Task1(string name, string description) :
        base(name, description)
    {
        Publications = new EventDeque<Publication>();
        Publications.Subscribers += EventDequeActionListener;

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

            new LabTaskActionBuilder().Name("Опубликовать первую в очередь публикацию")
                .ExecuteAction(ServeFrontPublication)
                .Build(),
            
            new LabTaskActionBuilder().Name("Удалить публикацию по названию")
                .ExecuteAction(DeletePublicationByName)
                .Build(),
            
            new LabTaskActionBuilder().Name("Поиск публикаций по названию")
                .ExecuteAction(SearchPublicationsByName)
                .Build(),

            new LabTaskActionBuilder().Name("Вывести все публикации")
                .ExecuteAction(() => EnumerableItemsPrinter.VerticallyWithNumbers(
                    Console.Out, Publications.GetEnumerator(), "Очередь публикаций пуста")
                ).Build()
        };
    }

    private void ServeFrontPublication()
    {
        if (Publications.Count == 0)
        {
            Console.WriteLine("Очередь публикаций пуста");
            return;
        }
        
        Console.WriteLine("Опубликовано: \n{0}", Publications.Front());
    }
    
    private void SearchPublicationsByName()
    {
        IResponsibleData<object> response;
        if (!(response = PublicationRequester.GetString(terminate: "...")).IsOk() 
            || response.Data() is not string value)
            return;

        EnumerableItemsPrinter.VerticallyWithNumbers(
            Console.Out,
            Publications.Where(x => x.Name == value).GetEnumerator(),
            $"Публикаций с названием '{value}' не найдено"
        );
    }

    private void DeletePublicationByName()
    {
        IResponsibleData<object> response;
        if (!(response = PublicationRequester.GetString(terminate: "...")).IsOk() 
            || response.Data() is not string value)
            return;
        
        try
        {
            Publications.Remove(Publications.Last(x => x.Name == value));
            Console.WriteLine($"Публикация с названием '{response.Data()}' удалена");
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine($"Публикаций с названием '{response.Data()}' не найдено");
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
            Console.WriteLine($"Публикация '{obj.Name}' добавлена.");
    }

    public static void EventDequeActionListener(object source, EventDequeActionArgs args) =>
        Console.WriteLine(new StringBuilder()
            .Append($"\nDeque<Publication>({((EventDeque<Publication>)source).Id}): \n").Append(args)
        );
}