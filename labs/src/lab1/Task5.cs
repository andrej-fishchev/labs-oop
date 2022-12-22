using labs.abstracts;
using labs.builders;
using labs.interfaces;
using labs.IO;
using labs.utils;

namespace labs.lab1;

public sealed class Task5 :
    LabTask
{
    public static readonly Circle Circle = 
        new(5.0, (5.0, 0.0));

    public static readonly Triangle Triangle = 
        new();
    
    private static readonly ConsoleDataRequest<double> UserDataRequest =
        new("", new ConsoleDataConverter<double>(
            DataConverterUtils.ToDoubleWithInvariant));

    private ConsoleDataResponse<double> x;
    private ConsoleDataResponse<double> y;

    public Task5(string name = "lab1.task5", string description = "") 
        : base(5, name, description)
    {
        x = new ConsoleDataResponse<double>(); 
        y = new ConsoleDataResponse<double>();
        
        Description = description;
        
        Actions = new List<ILabEntity<int>>
        {
            new LabTaskActionBuilder().Id(1).Name("Ввод данных")
                .ExecuteAction(InputData)
                .Build(),
            
            new LabTaskActionBuilder().Id(2).Name("Выполнить задачу")
                .ExecuteAction(() => UserDataRequest.Target
                    .Write($"f(x): {TaskExpression((x.Data, y.Data), Circle, Triangle)} \n"))
                .Build(),
            
            new LabTaskActionBuilder().Id(3).Name("Вывод данных")
                .ExecuteAction(OutputData)
                .Build()
        };
    }

    public void InputData()
    {
        UserDataRequest.DisplayMessage = "Введите X координату: ";
        x = (ConsoleDataResponse<double>)UserDataRequest.Request();
        
        if(x.Code != (int) ConsoleDataResponseCode.ConsoleOk)
            return;
        
        UserDataRequest.DisplayMessage = "Введите Y координату: ";
        y = (ConsoleDataResponse<double>)UserDataRequest.Request(sendRejectMessage: false);
    }

    public void OutputData()
    {
        UserDataRequest.Target
            .Write($"Точка: {(x.Data, y.Data)} \n");
    }

    public bool TaskExpression((double x, double y) dot, Circle circle, Triangle triangle)
    {
        return circle.Contains(dot) || triangle.Contains(dot);
    }
}