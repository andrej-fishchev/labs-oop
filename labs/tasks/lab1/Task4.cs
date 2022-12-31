using IO.requests;
using IO.responses;
using IO.targets;
using IO.utils;
using IO.validators;
using labs.builders;
using labs.entities;

namespace labs.lab1;

public sealed class Task4 :
    LabTask
{
    public readonly ConsoleTarget Target = new();
    
    private ConsoleResponseData<double> x;

    public Task4(string name = "lab1.task4", string description = "") 
        : base(4, name, description)
    {
        x = new ConsoleResponseData<double>();
        
        Description = description;
        
        Actions = new List<ILabEntity<int>>()
        {
            new LabTaskActionBuilder().Id(1).Name("Ввод данных")
                .ExecuteAction(InputData)
                .Build(),
            
            new LabTaskActionBuilder().Id(2).Name("Выполнить задачу")
                .ExecuteAction(() => Target
                    .Write($"f(x): {TaskExpression(x.Data)} \n"))
                .Build(),
            
            new LabTaskActionBuilder().Id(3).Name("Вывод данных")
                .ExecuteAction(OutputData)
                .Build()
        };
    }

    public void InputData()
    {
        x = (ConsoleResponseData<double>) 
            new ConsoleDataRequest<double>("Введите значение X из отрезка [-2.0; 0.0]: ")
            .Request(BaseTypeDataConverterFactory.MakeChainedDoubleConverter(), 
                new ConsoleDataValidator<double>(
                (data) => data >= -2.0 && data <= 0, "значение выходит за допустимые границы"));
    }

    public void OutputData()
    {
        Target.Write($"X = {x.Data} \n");
    }

    public double TaskExpression(double value)
    {
        return Math.Asin(Math.Abs(value + 1));
    }
}