using IO.requests;
using IO.responses;
using IO.utils;
using labs.builders;
using labs.entities;

namespace labs.lab1;

public sealed class Task4 :
    LabTask
{
    private ConsoleResponseData<double> x;

    public Task4(string name = "lab1.task4", string description = "") 
        : base(4, name, description)
    {
        x = new ConsoleResponseData<double>();
        
        Description = description;
        
        Actions = new List<ILabEntity<int>>
        {
            new LabTaskActionBuilder().Name("Ввод данных")
                .ExecuteAction(InputData)
                .Build(),
            
            new LabTaskActionBuilder().Name("Выполнить задачу")
                .ExecuteAction(() => Target.Output
                    .WriteLine($"arcsin(|x+1|) = arcsin(|{x.Data() + 1}|) = {TaskExpression(x.Data())}"))
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывод данных")
                .ExecuteAction(OutputData)
                .Build()
        };
    }

    public void InputData()
    {
        ConsoleResponseData<double> buffer = 
            new ConsoleDataRequest<double>("Введите значение X из отрезка [-2.0; 0.0]: ")
            .Request(BaseTypeDataConverterFactory.MakeDoubleConverterList(), BaseComparableValidatorFactory
                .MakeInRangeNotStrictValidator(-2D, 0D, "выход за допустимые границы"))
            .As<ConsoleResponseData<double>>();

        Task1.TryReceiveWithNotify(ref x, buffer, true);
    }

    public void OutputData() => Target.Output.WriteLine($"X = {x.Data()}");

    public double TaskExpression(double value) => Math.Asin(Math.Abs(value + 1));
}