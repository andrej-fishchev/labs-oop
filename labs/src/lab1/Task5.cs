using labs.builders;
using labs.entities;
using labs.IO;
using labs.utils;

namespace labs.lab1;

public sealed class Task5 :
    LabTask<int>
{
    public static readonly Circle Circle = 
        new(5.0, (5.0, 0.0));

    public static readonly Triangle Triangle = 
        new();
    
    private static readonly ConsoleDataRequest<double> UserDataRequest =
        new("", (string? data, out string? error) =>
        {
            error = null;

            double value;

            if(!DoubleParseUtils.TryWithInvariant(data, out value))
                error = $"ожидалось вещественное число, но получено {data}";

            return value;
        });
        
    private (double x, double y) point;
    
    public Task5(string name = "lab1.task5", string description = "") 
        : base(5, name)
    {
        point = default;
        
        Description = description;
        
        Actions = new List<LabTaskAction<int>>()
        {
            new LabTaskActionBuilder<int>().Id(1).Name("Ввод данных")
                .ExecuteAction(InputData)
                .Build<LabTaskAction<int>>(),
            
            new LabTaskActionBuilder<int>().Id(2).Name("Выполнить задачу")
                .ExecuteAction(() => UserDataRequest.ConsoleTarget
                    .Write($"f(x): {TaskExpression(point, Circle, Triangle)}"))
                .Build<LabTaskAction<int>>(),
            
            new LabTaskActionBuilder<int>().Id(3).Name("Вывод данных")
                .ExecuteAction(OutputData)
                .Build<LabTaskAction<int>>()
        };
    }

    public void InputData()
    {
        point.x = ConsoleIoDataUtils.RequestDoubleData(
            UserDataRequest, $"Введите x координату: ")
            .Data;
        
        point.y = ConsoleIoDataUtils.RequestDoubleData(
            UserDataRequest, $"Введите y координату: ")
            .Data;
    }

    public void OutputData()
    {
        UserDataRequest.ConsoleTarget
            .Write($"Точка: {point}");
    }

    public bool TaskExpression((double x, double y) dot, Circle circle, Triangle triangle)
    {
        return circle.Contains(dot) || triangle.Contains(dot);
    }
}