using labs.builders;
using labs.entities;
using labs.IO;

namespace labs.lab1;

public sealed class Task1 :
    LabTask<int>
{
    private double m_M;
    private double m_N;

    public Task1(string name = "lab1.task1", string description = "") 
        : base(1, name)
    {
        m_M = m_N = default;
        
        Description = description;
        
        Actions = new List<LabTaskAction<int>>()
        {
            new LabTaskActionBuilder<int>().Id(1).Name("Ввод данных")
                .ExecuteAction(InputData)
                .Build<LabTaskAction<int>>(),
            
            new LabTaskActionBuilder<int>().Id(2).Name("Выполнить задачу")
                .ExecuteAction(() => Console.WriteLine($"f(): {TaskExpression(ref m_M, ref m_N)}"))
                .Build<LabTaskAction<int>>(),
            
            new LabTaskActionBuilder<int>().Id(3).Name("Вывод данных")
                .ExecuteAction(OutputData)
                .Build<LabTaskAction<int>>()
        };
    }

    public void InputData()
    {
        // TODO: simplification and minimization
        ConsoleNumericIOResponse<double> response;
        
        do
        {
            response = (ConsoleNumericIOResponse<double>) 
                new ConsoleNumericIORequest<double>(
                "Введите M: ",
                (string? data, out string? error) =>
                    {
                        error = null;
                        
                        double result;
                        
                        if (!double.TryParse(data, out result))
                            error = $"Ошибка: Ожидалось вещественное значение, но получено '{data}'";

                        return result;
                    })
                    .Request(_ => true);
            
            if(response.Error != null)
                Console.WriteLine(response.Error);

        } while (response.Error != null);
        
        // TODO: m_N :/
        m_M = response.Data;
    }

    public void OutputData()
    {
        Console.WriteLine($"M: {m_M} \nN: {m_N}");
    }

    public double TaskExpression(ref double m, ref double n)
    {
        return m - ++n;
    }
}