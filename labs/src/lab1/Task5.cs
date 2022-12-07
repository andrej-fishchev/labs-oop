using labs.builders;
using labs.entities;
using labs.lab1.utils;

namespace labs.lab1;

public sealed class Task5 :
    LabTask<int>
{
    public static readonly Circle CIRCLE = 
        new Circle(5.0, (5.0, 0.0));

    public static readonly Triangle TRIANGLE =
        new Triangle();
        
    private (double x, double y) m_X;
    
    public Task5(string name = "lab1.task5", string description = "") 
        : base(5, name)
    {
        m_X = default;
        
        Description = description;
        
        Actions = new List<LabTaskAction<int>>()
        {
            new LabTaskActionBuilder<int>().Id(1).Name("Ввод данных")
                .Delegator(InputData)
                .Build<LabTaskAction<int>>(),
            
            new LabTaskActionBuilder<int>().Id(2).Name("Выполнить задачу")
                .Delegator(() => Console.WriteLine($"f(x): {TaskExpression(m_X, CIRCLE, TRIANGLE)}"))
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
        Console.WriteLine($"Точка: {m_X}");
    }

    public bool TaskExpression((double x, double y) dot, Circle circle, Triangle triangle)
    {
        return circle.Contains(dot) || triangle.Contains(dot);
    }
}