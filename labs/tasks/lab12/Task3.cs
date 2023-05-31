using System.Text;
using labs.builders;
using labs.entities;
using labs.lab10.src;
using labs.lab10.src.requests;
using labs.shared.data.abstracts;
using labs.shared.data.algorithms.CustomHashTable.linq;
using UserDataRequester.responses;

namespace labs.tasks.lab12;

public sealed class Task3 : LabTask
{
    private static Task3? instance;

    private CustomHashTable<Publication> Publications
    {
        get;
    }

    public static Task3 GetInstance(string name = "lab12.task3", string description = "var. 24")
    {
        if (instance == null)
            instance = new Task3(name, description);

        return instance;
    }

    private Task3(string name, string description) :
        base(name, description)
    {
        Publications = new CustomHashTable<Publication>();

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

            new LabTaskActionBuilder().Name("Получить публикацию по названию")
                .ExecuteAction(GetPublicationByName)
                .Build(),
            
            new LabTaskActionBuilder().Name("Удалить публикацию по названию")
                .ExecuteAction(DeletePublicationByName)
                .Build(),

            new LabTaskActionBuilder().Name("Вывести все публикации")
                .ExecuteAction(PrintCustomHashTable).Build()
        };
    }

    public void GetAndSaveBook() => 
        GetAndSavePublication<Book>(PublicationRequester.GetBook);
   
    public void GetAndSaveEduBook() => 
        GetAndSavePublication<EducationalBook>(PublicationRequester.GetEduBook);
    
    public void GetAndSaveJournal() => 
        GetAndSavePublication<Journal>(PublicationRequester.GetJournal);

    public void GetPublicationByName()
    {
        IResponsibleData<object> response;
        if (!(response = PublicationRequester.GetString(terminate: "...")).IsOk())
            return;
        try
        {
            Predicate<Publication> condition = 
                obj => obj.Name == (string)response.Data()!;

            var subContainer =
                Publications.ChooseContainer(condition);

            var buffer = subContainer?.FirstOrDefault(condition.Invoke);
        
            Console.WriteLine("CONSOLE| Публикация с названием '{0}' {1}",
                response.Data(),
                (buffer != null
                    ? $"найдена:\n{buffer}"
                    : "не найдена")
            );
        }
        catch (ArgumentNullException)
        {
            Console.WriteLine("CONSOLE| Список публикаций пуст");
        }
        
    }

    public void GetAndSavePublication<T>(Func<string, IResponsibleData<object>> getRequester, string terminate = "...") 
        where T : Publication
    {
        Publication? obj;
        if ((obj = PublicationRequester.GetRequest<T>(getRequester, terminate)) != null)
            Console.WriteLine($"CONSOLE| Публикация '{obj.Name}' {(Publications.Add(obj) ? "" : "не")} добавлена.");
    }
    
    public void DeletePublicationByName()
    {
        IResponsibleData<object> response;
        if (!(response = PublicationRequester.GetString(terminate: "...")).IsOk())
            return;

        try
        {
            Predicate<Publication> condition =
                obj => obj.Name == (string)response.Data()!;

            IEnumerable<Publication>? subContainer =
                Publications.ChooseContainer(condition);

            Publication? buffer = subContainer.First(condition.Invoke);
            
            Publications.Remove(buffer);

            Console.WriteLine($"CONSOLE| Публикация с названием {response.Data()} удалена");
        }
        catch
        {
            Console.WriteLine($"CONSOLE| Публикация с названием {response.Data()} не найдена");
        }
    }

    public void PrintCustomHashTable()
    {
        var pos = 0;
        var output = new StringBuilder();
        
        foreach (var chain in Publications.InnerContainer())
        {
            if(chain == null)
                continue;
            
            output.Append($"#{++pos} :\n");

            foreach (var publication in chain)
                if(publication != null)
                    output.Append($"-- {publication}\n");
        }

        if (output.Length == 0)
            output.Append("CONSOLE| Список элементов пуст");
        
        Console.WriteLine(output);
    }
}