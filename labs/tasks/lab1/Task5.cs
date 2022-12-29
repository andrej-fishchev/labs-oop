using IO.requests;
using IO.responses;
using IO.targets;
using IO.utils;
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

    public readonly ConsoleTarget Target = new();
    
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
                .ExecuteAction(() => Target
                    .Write($"f(x): {TaskExpression((x.Data, y.Data), Circle, Triangle)} \n"))
                .Build(),
            
            new LabTaskActionBuilder().Id(3).Name("Вывод данных")
                .ExecuteAction(OutputData)
                .Build()
        };
    }

    public void InputData()
    {
        x = InputData("Введите значение X координаты: ");
        
        if(x.Code != (int) ConsoleResponseDataCode.ConsoleOk)
            return;
        
        y = InputData("Введите значение Y координаты", false);
    }
    
    private ConsoleResponseData<double> InputData(string message, bool sendReject = true)
    {
        return (ConsoleResponseData<double>) new ConsoleDataRequest<double>(message)
            .Request(BaseTypeDataConverterFactory.MakeChainedDoubleConverter(), 
                sendRejectMessage: sendReject);
    }

    public void OutputData()
    {
        Target.Write($"Точка: {(x.Data, y.Data)} \n");
    }

    public bool TaskExpression((double x, double y) dot, Circle circle, Triangle triangle)
    {
        return circle.Contains(dot) || triangle.Contains(dot);
    }
}