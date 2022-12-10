using labs.builders;
using labs.entities;
using labs.IO;
using labs.utils;

namespace labs.lab1;

public sealed class Task4 :
    LabTask<int>
{
    private double x;

    private static readonly ConsoleDataRequest<double> UserDataRequest =
        new("", (string? data, out string? error) =>
        {
            error = null;

            double value;

            if(!DoubleParseUtils.TryWithInvariant(data, out value))
                error = $"ожидалось вещественное число, но получено {data}";

            return value;
        });
    
    public Task4(string name = "lab1.task4", string description = "") 
        : base(4, name)
    {
        x = default;
        
        Description = description;
        
        Actions = new List<LabTaskAction<int>>()
        {
            new LabTaskActionBuilder<int>().Id(1).Name("Ввод данных")
                .ExecuteAction(InputData)
                .Build<LabTaskAction<int>>(),
            
            new LabTaskActionBuilder<int>().Id(2).Name("Выполнить задачу")
                .ExecuteAction(() => UserDataRequest.ConsoleTarget
                    .Write($"f(x): {TaskExpression(x)}"))
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
        
        x = ConsoleIoDataUtils.RequestDoubleDataWithValidator(
            UserDataRequest,
            "Введите X из отрезка [-2.0; 0.0]: ",
            new DataIoValidator<double>(
                (value) => value >= -2.0 && value <= 0.0)
        ).Data;
    }

    public void OutputData()
    {
        UserDataRequest
            .ConsoleTarget
            .Write($"X = {x}");
    }

    public double TaskExpression(double x)
    {
        return Math.Asin(Math.Abs(x + 1));
    }
}