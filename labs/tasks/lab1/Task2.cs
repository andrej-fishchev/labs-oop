using IO.responses;
using labs.builders;
using labs.entities;

namespace labs.lab1;

public sealed class Task2 : LabTask
{
    private static Task2? instance;
    
    private ConsoleResponseData<double> m;
    private ConsoleResponseData<double> n;

    public static Task2 GetInstance(string name, string description)
    {
        if (instance == null)
            instance = new Task2(name, description);

        return instance;
    }
    
    private Task2(string name, string description) : 
        base(name, description)
    {
        m = new ConsoleResponseData<double>(); 
        n = new ConsoleResponseData<double>();
        
        Actions = new List<ILabEntity<string>>
        {
            new LabTaskActionBuilder().Name("Ввод данных")
                .ExecuteAction(InputData)
                .Build(),
            
            new LabTaskActionBuilder().Name("Выполнить задачу")
                .ExecuteAction(() => TaskExpression())
                .Build(),
            
            new LabTaskActionBuilder().Name("Выполнить задачу [с запоминанием]")
                .ExecuteAction(() => TaskExpression(true))
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывод данных")
                .ExecuteAction(() => Task1.OutputData(m.Data(), n.Data()))
                .Build()
        };
    }
    
    public void InputData()
    {
        if (Task1.TryReceiveWithNotify(ref m, Task1.InputData("Введите M")))
            Task1.TryReceiveWithNotify(ref n, Task1.InputData("Введите N", false), 
                true);
    }
    
    public void TaskExpression(bool receive = false)
    {
        double left = m.Data();
        double right = n.Data();

        Task1.OutputData(left, right);
        
        Target.Output.WriteLine($"m++ > --n = {left++ > --right}");

        Task1.OutputData(left, right);
        
        if (receive)
        {
            m.Data(left);
            n.Data(right);
        }
    }
    
}