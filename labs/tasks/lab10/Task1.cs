using labs.builders;
using labs.entities;
using labs.lab10.src;

namespace labs.lab10;

public sealed class Task1 : LabTask
{
    public Task1(string name = "lab10.task1", string description = "") :
        base(1, name, description)
    {
        Actions = new List<ILabEntity<int>>
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
        Publication publication = GetBookInstance();

        Book book = GetBookInstance();

        Target.Output
            .WriteLine("Позднее связывание (переменная типа Publication проинициализированна как Book()):");
        
        publication.Describe(Target.Output);
        
        Target.Output
            .WriteLine("\nПозднее связывание (переменная типа Book проинициализированна и содержит ссылку на объект Book):");
        
        book.Describe(Target.Output);
    }

    void CompileTimeLinking()
    {
        Publication publication = GetBookInstance();
        
        Book book = GetBookInstance();
        
        Target.Output
            .WriteLine("Раннее связывание (переменная типа Publication проинициализированна как Book()):");
        
        publication.NoOverridingDescribe(Target.Output);
        
        Target.Output
            .WriteLine("\nРаннее связывание (переменная типа Book проинициализированна и содержит ссылку на объект Book):");
        
        book.NoOverridingDescribe(Target.Output);
    }
    
    public Book GetBookInstance() =>
        new("Сборник сказок А.С. Пушкина", DateOnly.FromDateTime(DateTime.Now), new List<string>
        {
            "А.С. Пушкин"
        });
}