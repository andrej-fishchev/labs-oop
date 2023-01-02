using IO.requests;
using IO.responses;
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
            new LabTaskActionBuilder().Name("Ввод данных")
                .ExecuteAction(InputData)
                .Build(),
            
            new LabTaskActionBuilder().Name("Выполнить задачу")
                .ExecuteAction(() => Target.Output
                    .WriteLine($"f(x): {TaskExpression((x.Data(), y.Data()), Circle, Triangle)}"))
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывод данных")
                .ExecuteAction(OutputData)
                .Build()
        };
    }

    public void InputData()
    {
        x = InputData("Введите значение X координаты: ");
        
        if(!x) return;
        
        y = InputData("Введите значение Y координаты", false);
    }
    
    private ConsoleResponseData<double> InputData(string message, bool sendReject = true)
    {
        return new ConsoleDataRequest<double>(message)
            .Request(BaseTypeDataConverterFactory.MakeDoubleConverterList(), 
                sendRejectMessage: sendReject)
            .As<ConsoleResponseData<double>>();
    }

    public void OutputData()
    {
        Target.Output.WriteLine($"Точка: {(x.Data(), y.Data())}");
    }

    public bool TaskExpression((double x, double y) dot, Circle circle, Triangle triangle)
    {
        return circle.Contains(dot) || triangle.Contains(dot);
    }
}