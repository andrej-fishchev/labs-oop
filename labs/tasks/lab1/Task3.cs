using System.Text;
using IO.requests;
using IO.responses;
using IO.utils;
using labs.builders;
using labs.entities;

namespace labs.lab1;

public sealed class Task3 :
    LabTask
{
    private ConsoleResponseData<double> m;
    private ConsoleResponseData<double> n;

    public Task3(string name = "lab1.task3", string description = "") : 
        base(3, name, description)
    {
        m = new ConsoleResponseData<double>();
        n = new ConsoleResponseData<double>();
        
        Description = description;
        
        Actions = new List<ILabEntity<int>>()
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
        
        Target.Output.WriteLine($"m-- < ++n = {left-- < ++right}");

        Task1.OutputData(left, right);

        if (receive)
        {
            m.Data(left);
            n.Data(right);
        }
    }
}