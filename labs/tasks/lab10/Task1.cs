using labs.builders;
using labs.entities;
using labs.lab10.src;

namespace labs.lab10;

public sealed class Task1 : LabTask
{
    private static Task1? instance;
    
    private static readonly Book BookInstance = 
        new("Сборник сказок А.С. Пушкина", DateOnly.FromDateTime(DateTime.Now), 
            new List<string> { "А.С. Пушкин" });
    
    public static Task1 GetInstance(string name, string description)
    {
        if (instance == null)
            instance = new Task1(name, description);

        return instance;
    }
    
    private Task1(string name, string description) :
        base(name, description)
    {
        Actions = new List<ILabEntity<string>>
        {
            new LabTaskActionBuilder().Name("Позднее связывание")
                .ExecuteAction(RunTimeLinking)
                .Build(),
            
            new LabTaskActionBuilder().Name("Раннее связывание")
                .ExecuteAction(CompileTimeLinking)
                .Build()
        };
    }
    
    void RunTimeLinking()
    {
        Publication publication = BookInstance;

        Book book = BookInstance;

        Target.Output
            .WriteLine("Позднее связывание (переменная типа Publication проинициализирована и содержит ссылку на объект Book):");
        
        publication.Describe(Target.Output);
        
        Target.Output
            .WriteLine("\nПозднее связывание (переменная типа Book проинициализирована и содержит ссылку на объект Book):");
        
        book.Describe(Target.Output);
    }

    void CompileTimeLinking()
    {
        Publication publication = BookInstance;
        
        Book book = BookInstance;
        
        Target.Output
            .WriteLine("Раннее связывание (переменная типа Publication проинициализирована и содержит ссылку на объект Book):");
        
        publication.NoOverridingDescribe(Target.Output);
        
        Target.Output
            .WriteLine("\nРаннее связывание (переменная типа Book проинициализирована и содержит ссылку на объект Book):");
        
        book.NoOverridingDescribe(Target.Output);
    }

}