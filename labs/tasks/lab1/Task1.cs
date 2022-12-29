using System.Globalization;
using IO.converters;
using IO.requests;
using IO.responses;
using labs.builders;
using labs.entities;

namespace labs.lab1;

public sealed class Task1 : LabTask
{
    private ConsoleResponseData<double> m;
    private ConsoleResponseData<double> n;

    private static readonly ConsoleDataRequest<double> 
        UserSimpleDataRequest = new("");

    private static readonly FormattedConsoleNumberDataConverter<double>
        ToDoubleConverter = ConsoleDataConverterFactory
            .MakeFormattedNumberDataConverter<double>(
                double.TryParse, 
                NumberStyles.Float | NumberStyles.AllowThousands,
                NumberFormatInfo.InvariantInfo);

    public Task1(
        string name = "lab1.task1", string description = "Вычислить значение выражения и его аргументов: m - ++n") : 
        base(1, name, description)
    {
        m = new ConsoleResponseData<double>();
        n = new ConsoleResponseData<double>();
        
        Actions = new List<ILabEntity<int>>()
        {
            new LabTaskActionBuilder().Id(1).Name("Ввод данных")
                .ExecuteAction(InputData)
                .Build(),
            
            new LabTaskActionBuilder().Id(2).Name("Выполнить задачу")
                .ExecuteAction(() =>
                { 
                    UserSimpleDataRequest.Target
                        .Write($"f(): {TaskExpression()} \n");
                    
                    OutputData();
                })
                .Build(),
            
            new LabTaskActionBuilder().Id(3).Name("Вывод данных")
                .ExecuteAction(OutputData)
                .Build()
        };

    }

    public void InputData()
    {
        UserSimpleDataRequest.DisplayMessage = "Введите M: ";
        m = (ConsoleResponseData<double>) 
            UserSimpleDataRequest.Request(ToDoubleConverter);
        
        if(m.Code != (int) ConsoleResponseDataCode.ConsoleOk)
            return;
        
        UserSimpleDataRequest.DisplayMessage = "Введите N: ";
        n = (ConsoleResponseData<double>) 
            UserSimpleDataRequest.Request(ToDoubleConverter, sendRejectMessage: false);
    }

    public void OutputData()
    {
        UserSimpleDataRequest.Target
            .Write($"M: {m.Data} \nN: {n.Data}\n");
    }

    public double TaskExpression()
    {
        return m.Data - ++n.Data;
    }
}