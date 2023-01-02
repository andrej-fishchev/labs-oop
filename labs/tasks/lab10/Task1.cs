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
        Publication book = new Book("Сборник сказок А.С. Пушкина", DateTime.Now.ToShortDateString(), new List<string>
        {
            "А.С. Пушкин"
        });
        
        Book bookCopy = (Book)book;
        
        Target.Output
            .WriteLine("Позднее связывание (переменная типа Publication проинициализированна как Book()):");
        
        book.Describe(Target.Output);
        
        Target.Output
            .WriteLine("\nПозднее связывание (переменная типа Book проинициализированна и содержит ссылку на объект Book):");
        
        bookCopy.Describe(Target.Output);
    }

    void CompileTimeLinking()
    {
        Publication book = new Book("Сборник сказок А.С. Пушкина", DateTime.Now.ToShortDateString(), new List<string>
        {
            "А.С. Пушкин"
        });
        
        Book bookCopy = (Book)book;
        
        Target.Output
            .WriteLine("Раннее связывание (переменная типа Publication проинициализированна как Book()):");
        
        book.NoOverridingDescribe(Target.Output);
        
        Target.Output
            .WriteLine("\nРаннее связывание (переменная типа Book проинициализированна и содержит ссылку на объект Book):");
        
        bookCopy.NoOverridingDescribe(Target.Output);
    }
}