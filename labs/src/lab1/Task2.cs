using labs.builders;
using labs.entities;

namespace labs.lab1;

public sealed class Task2 :
    LabTask<int>
{
    private double m_M;
    private double m_N;

    public Task2(string name = "lab1.task2", string description = "") 
        : base(2, name)
    {
        m_M = m_N = 0;
        
        Description = description;
        
        Actions = new List<LabTaskAction<int>>()
        {
            new LabTaskActionBuilder<int>().Id(1).Name("Ввод данных")
                .Delegator(InputData)
                .Build<LabTaskAction<int>>(),
            
            new LabTaskActionBuilder<int>().Id(2).Name("Выполнить задачу")
                .Delegator(() => Console.WriteLine($"f(): {TaskExpression(ref m_M, ref m_N)}"))
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
        Console.WriteLine($"M: {m_M} \nN: {m_N}");
    }

    public bool TaskExpression(ref double m, ref double n)
    {
        return (m++ > --n);
    }
}