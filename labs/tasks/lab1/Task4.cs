using IO.requests;
using IO.responses;
using IO.utils;
using labs.builders;
using labs.entities;

namespace labs.lab1;

public sealed class Task4 : LabTask
{
    private static Task4? instance;
    
    private ConsoleResponseData<double> x;

    public static Task4 GetInstance(string name, string description)
    {
        if (instance == null)
            instance = new Task4(name, description);

        return instance;
    }
    
    private Task4(string name, string description) 
        : base(name, description)
    {
        x = new ConsoleResponseData<double>();
        
        Actions = new List<ILabEntity<string>>
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