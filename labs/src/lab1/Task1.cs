using labs.builders;
using labs.entities;
using labs.IO;
using labs.utils;

namespace labs.lab1;

public sealed class Task1 :
    LabTask<int>
{
    private double m;
    private double n;

    private static readonly ConsoleDataRequest<double> UserDataRequest =
        new("", (string? data, out string? error) =>
        {
            error = null;

            double value;

            if(!DoubleParseUtils.TryWithInvariant(data, out value))
                error = $"ожидалось вещественное число, но получено {data}";

            return value;
        });

    public Task1(string name = "lab1.task1", string description = "") 
        : base(1, name)
    {
        m = n = default;
        
        Description = description;
        
        Actions = new List<LabTaskAction<int>>()
        {
            new LabTaskActionBuilder<int>().Id(1).Name("Ввод данных")
                .ExecuteAction(InputData)
                .Build<LabTaskAction<int>>(),
            
            new LabTaskActionBuilder<int>().Id(2).Name("Выполнить задачу")
                .ExecuteAction(() => UserDataRequest.ConsoleTarget
                    .Write($"f(): {TaskExpression(ref m, ref n)}"))
                .Build<LabTaskAction<int>>(),
            
            new LabTaskActionBuilder<int>().Id(3).Name("Вывод данных")
                .ExecuteAction(OutputData)
                .Build<LabTaskAction<int>>()
        };
    }

    public void InputData()
    {
        UserDataRequest.ConsoleTarget
            .Write($"Ввод может быть прекращен в любое удобное время." +
                   $"Введите '{UserDataRequest.RejectMessage}' для незамедлительного прекращения исполнения задачи");
        
        m = ConsoleIoDataUtils
            .RequestDoubleData(UserDataRequest, $"Введите M: ").Data;
        
        n = ConsoleIoDataUtils
            .RequestDoubleData(UserDataRequest, $"Введите M: ").Data;
    }

    public void OutputData()
    {
        UserDataRequest.ConsoleTarget.Write($"M: {m} \nN: {n}");
    }

    public double TaskExpression(ref double left, ref double right)
    {
        return left - ++right;
    }
}