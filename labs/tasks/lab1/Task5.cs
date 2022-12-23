using System.Globalization;
using IO.converters;
using IO.requests;
using IO.responses;
using labs.builders;
using labs.entities;
using labs.utils;

namespace labs.lab1;

public sealed class Task5 :
    LabTask
{
    public static readonly Circle Circle = 
        new(5.0, (5.0, 0.0));

    public static readonly Triangle Triangle = 
        new();
    
    private static readonly ConsoleDataRequest<double> 
        UserSimpleDataRequest = new("");
    
    private static readonly FormattedConsoleNumberDataConverter<double>
        ToDoubleConverter = ConsoleDataConverterFactory
            .MakeFormattedNumberDataConverter<double>(
                double.TryParse, 
                NumberStyles.Float | NumberStyles.AllowThousands,
                NumberFormatInfo.InvariantInfo);
    
    private ConsoleResponseData<double> x;
    private ConsoleResponseData<double> y;

    public Task5(string name = "lab1.task5", string description = "") 
        : base(5, name, description)
    {
        x = new ConsoleResponseData<double>(); 
        y = new ConsoleResponseData<double>();
        
        Description = description;
        
        Actions = new List<ILabEntity<int>>
        {
            new LabTaskActionBuilder().Id(1).Name("Ввод данных")
                .ExecuteAction(InputData)
                .Build(),
            
            new LabTaskActionBuilder().Id(2).Name("Выполнить задачу")
                .ExecuteAction(() => UserSimpleDataRequest.Target
                    .Write($"f(x): {TaskExpression((x.Data, y.Data), Circle, Triangle)} \n"))
                .Build(),
            
            new LabTaskActionBuilder().Id(3).Name("Вывод данных")
                .ExecuteAction(OutputData)
                .Build()
        };
    }

    public void InputData()
    {
        UserSimpleDataRequest.DisplayMessage = "Введите X координату: ";
        x = (ConsoleResponseData<double>)
            UserSimpleDataRequest.Request(ToDoubleConverter);
        
        if(x.Code != (int) ConsoleResponseDataCode.ConsoleOk)
            return;
        
        UserSimpleDataRequest.DisplayMessage = "Введите Y координату: ";
        y = (ConsoleResponseData<double>)
            UserSimpleDataRequest.Request(ToDoubleConverter, sendRejectMessage: false);
    }

    public void OutputData()
    {
        UserSimpleDataRequest.Target
            .Write($"Точка: {(x.Data, y.Data)} \n");
    }

    public bool TaskExpression((double x, double y) dot, Circle circle, Triangle triangle)
    {
        return circle.Contains(dot) || triangle.Contains(dot);
    }
}