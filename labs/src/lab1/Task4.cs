using labs.builders;
using labs.entities;

namespace labs.lab1;

public sealed class Task4 :
    LabTask<int>
{
    private double m_X;

    public Task4(string name = "lab1.task4", string description = "") 
        : base(4, name)
    {
        m_X = 0;
        
        Description = description;
        
        Actions = new List<LabTaskAction<int>>()
        {
            new LabTaskActionBuilder<int>().Id(1).Name("Ввод данных")
                .Delegator(InputData)
                .Build<LabTaskAction<int>>(),
            
            new LabTaskActionBuilder<int>().Id(2).Name("Выполнить задачу")
                .Delegator(() => Console.WriteLine($"f(x): {TaskExpression(m_X)}"))
                .Build<LabTaskAction<int>>(),
            
            new LabTaskActionBuilder<int>().Id(3).Name("Вывод данных")
                .Delegator(OutputData)
                .Build<LabTaskAction<int>>()
        };
    }

    public void InputData()
    {
        // TODO: asd
    }

    public void OutputData()
    {
        Console.WriteLine($"X = {m_X}");
    }

    public double TaskExpression(double x)
    {
        return Math.Asin(Math.Abs(x + 1));
    }
}