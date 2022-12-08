using labs.builders;
using labs.entities;

namespace labs.lab1;

public sealed class Task6 :
    LabTask<int>
{
    private (float a, float b) m_FloatData;
    private (double a, double b) m_DoubleData;
    
    public Task6(string name = "lab1.task6", string description = "") 
        : base(6, name)
    {
        m_DoubleData = (1000.0, 0.0001);
        m_FloatData = (1000.0f, 0.0001f);
        
        Description = description;
        
        Actions = new List<LabTaskAction<int>>()
        {
            new LabTaskActionBuilder<int>().Id(1).Name("Выполнить задачу")
                .ExecuteAction(() => Console.WriteLine($"f(float):      {TaskExpression(m_FloatData)} " +
                                                       $"\nf(double):   {TaskExpression(m_DoubleData)}"))
                .Build<LabTaskAction<int>>(),
            
            new LabTaskActionBuilder<int>().Id(2).Name("Вывод данных")
                .ExecuteAction(OutputData)
                .Build<LabTaskAction<int>>()
        };
    }

    public void OutputData()
    {
        Console.WriteLine($"Float: {m_FloatData} \nDouble: {m_DoubleData}");
    }

    public double TaskExpression((double a, double b) data)
    {
        // TODO: asd
        return data.a;
    }
    
    public float TaskExpression((float a, float b) data)
    {
        // TODO: asd
        return data.a;
    }
}