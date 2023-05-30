using labs.builders;
using labs.entities;
using labs.lab10.src;
using labs.lab10.src.requests;
using labs.shared.data.abstracts;
using labs.shared.data.algorithms.BinaryTree.walks;
using labs.shared.data.algorithms.BinaryTree.walks.linq;
using labs.shared.utils;
using UserDataRequester.responses;

namespace labs.tasks.lab12;

public sealed class Task2 : LabTask
{
    private static Task2? instance;

    private BinaryTree<Publication> Publications { get; }

    public static Task2 GetInstance(string name = "lab12.task2", string description = "var. 24")
        => instance ??= new Task2(name, description);

    private Task2(string name, string description) :
        base(name, description)
    {
        Publications = new BinaryTree<Publication>(new PreOrderTreeWalk<Publication>());

        Actions = new List<ILabEntity<string>>
        {
            new LabTaskActionBuilder().Name("Добавить книгу")
                .ExecuteAction(GetAndSaveBook)
                .Build(),
            
            new LabTaskActionBuilder().Name("Добавить журнал")
                .ExecuteAction(GetAndSaveJournal)
                .Build(),
            
            new LabTaskActionBuilder().Name("Добавить учебник")
                .ExecuteAction(GetAndSaveEduBook)
                .Build(),

            new LabTaskActionBuilder().Name("Получить публикацию с минимальным количеством авторов")
                .ExecuteAction(GetPublicationMinByName)
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

    public void GetAndSaveBook() => 
        GetAndSavePublication<Book>(PublicationRequester.GetBook);
   
    public void GetAndSaveEduBook() => 
        GetAndSavePublication<EducationalBook>(PublicationRequester.GetEduBook);
    
    public void GetAndSaveJournal() => 
        GetAndSavePublication<Journal>(PublicationRequester.GetJournal);

    public void GetPublicationMinByName()
    {
        var buffer = Publications
            .InternalNode()
            .DataWalk(Publications.TreeWalkAlgorithm)
            .MinBy(x => x.Authors.Count);
            
        Console.WriteLine($"CONSOLE| Публикация с минимальным количеством авторов:" +
                          $"\n {(buffer != null ? buffer : "не существует")}");
    }

    public void GetAndSavePublication<T>(Func<string, IResponsibleData<object>> getRequester, string terminate = "...") 
        where T : Publication
    {
        Publication? obj;
        if ((obj = PublicationRequester.GetRequest<T>(getRequester, terminate)) != null)
            Console.WriteLine("CONSOLE| Публикация '{0}' {1}",
                obj.Name,
                Publications.Add(obj)
                    ? "добавлена"
                    : "не добавлена"
            );
    }
    
    public void DeletePublicationByName()
    {
        IResponsibleData<object> response;
        if (!(response = PublicationRequester.GetString(terminate: "...")).IsOk())
            return;

        try
        {
            Publications.Remove(Publications.Last(x => x.Name == (string)response.Data()!));
            Console.WriteLine($"CONSOLE| Последняя публикация (в списке) с названием {response.Data()} удалена");
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine($"CONSOLE| Публикаций с названием {response.Data()} не найдено");
        }
    }
}