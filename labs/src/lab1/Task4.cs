using labs.abstracts;
using labs.builders;
using labs.interfaces;
using labs.IO;
using labs.utils;

namespace labs.lab1;

public sealed class Task4 :
    LabTask
{
    private ConsoleDataResponse<double> x;

    private static readonly ConsoleDataRequest<double> UserDataRequest =
        new("Введите X из отрезка [-2.0; 0.0]: ", new ConsoleDataConverter<double>(
            DataConverterUtils.ToDoubleWithInvariant));
    
    public Task4(string name = "lab1.task4", string description = "") 
        : base(4, name, description)
    {
        x = new ConsoleDataResponse<double>();
        
        Description = description;
        
        Actions = new List<ILabEntity<int>>()
        {
            new LabTaskActionBuilder().Id(1).Name("Ввод данных")
                .ExecuteAction(InputData)
                .Build(),
            
            new LabTaskActionBuilder().Id(2).Name("Выполнить задачу")
                .ExecuteAction(() => UserDataRequest.Target
                    .Write($"f(x): {TaskExpression(x.Data)} \n"))
                .Build(),
            
            new LabTaskActionBuilder().Id(3).Name("Вывод данных")
                .ExecuteAction(OutputData)
                .Build()
        };
    }

    public void InputData()
    {
        x = (ConsoleDataResponse<double>)
            UserDataRequest.Request(
            new ConsoleDataValidator<double>(
                (data) => data >= -2.0 && data <= 0));
    }

    public void OutputData()
    {
        UserDataRequest.Target
            .Write($"X = {x} \n");
    }

    public double TaskExpression(double value)
    {
        return Math.Asin(Math.Abs(value + 1));
    }
}