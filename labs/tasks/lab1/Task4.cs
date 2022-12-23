using System.Globalization;
using IO.converters;
using IO.requests;
using IO.responses;
using IO.validators;
using labs.builders;
using labs.entities;

namespace labs.lab1;

public sealed class Task4 :
    LabTask
{
    private ConsoleResponseData<double> x;

    private static readonly ConsoleDataRequest<double> 
        UserSimpleDataRequest = new("Введите X из отрезка [-2.0; 0.0]");

    private static readonly FormattedConsoleNumberDataConverter<double>
        ToDoubleConverter = ConsoleDataConverterFactory
            .MakeFormattedNumberDataConverter<double>(
                double.TryParse, 
                NumberStyles.Float | NumberStyles.AllowThousands,
                NumberFormatInfo.InvariantInfo);
    
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
                .ExecuteAction(() => UserSimpleDataRequest.Target
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
            UserSimpleDataRequest.Request(
                ToDoubleConverter, new ConsoleDataValidator<double>(
                (data) => data >= -2.0 && data <= 0));
    }

    public void OutputData()
    {
        UserSimpleDataRequest.Target
            .Write($"X = {x} \n");
    }

    public double TaskExpression(double value)
    {
        return Math.Asin(Math.Abs(value + 1));
    }
}