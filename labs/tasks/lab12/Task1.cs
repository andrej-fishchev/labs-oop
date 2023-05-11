using labs.builders;
using labs.entities;
using labs.lab10.src;
using labs.lab10.src.requests;
using labs.shared.data.abstracts;
using UserDataRequester.responses;

namespace labs.lab12;

public sealed class Task1 : LabTask
{
    private static Task1? instance;
    
    public Deque<Publication> Publications
    {
        get;
    }

    public static Task1 GetInstance(string name = "lab12.task1", string description = "var. 24")
    {
        if (instance == null)
            instance = new Task1(name, description);

        return instance;
    }

    private Task1(string name, string description) :
        base(name, description)
    {
        Publications = new Deque<Publication>();

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

            new LabTaskActionBuilder().Name("Удалить публикацию по названию")
                .ExecuteAction(DeletePublicationByName)
                .Build(),

            new LabTaskActionBuilder().Name("Вывести все публикации")
                .ExecuteAction(() => OutputEnumerator(Console.Out, Publications.GetEnumerator()))
                .Build()
        };
    }

    public void GetAndSaveBook() => 
        GetAndSavePublication<Book>(PublicationRequester.GetBook);
   
    public void GetAndSaveEduBook() => 
        GetAndSavePublication<EducationalBook>(PublicationRequester.GetEduBook);
    
    public void GetAndSaveJournal() => 
        GetAndSavePublication<Journal>(PublicationRequester.GetJournal);

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
    
    public static void OutputEnumerator<T>(TextWriter writer, IEnumerator<T> items)
    {
        int pos = 0;
        while (items.MoveNext())
            writer.WriteLine($"{++pos}. {items.Current} \n");
        
        if(pos == 0)
            writer.WriteLine("CONSOLE| Список публикаций пуст");
    }

    public void GetAndSavePublication<T>(Func<string, IResponsibleData<object>> func, string terminate = "...") 
        where T : Publication
    {
        Publication? obj;
        if ((obj = PublicationRequester.GetRequest<T>(func, terminate)) != null)
            Publications.Back(obj);

        if (obj != null)
            Console.WriteLine($"CONSOLE| Публикация '{obj.Name}' добавлена.");
    }
}