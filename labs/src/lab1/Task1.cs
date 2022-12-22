using labs.abstracts;
using labs.builders;
using labs.interfaces;
using labs.IO;
using labs.utils;

namespace labs.lab1;

public sealed class Task1 : LabTask
{
    private ConsoleDataResponse<double> m;
    private ConsoleDataResponse<double> n;

    private static readonly ConsoleDataRequest<double> UserDataRequest =
        new("", new ConsoleDataConverter<double>(
            DataConverterUtils.ToDoubleWithInvariant));
    
    public Task1(
        string name = "lab1.task1", string description = "Вычислить значение выражения и его аргументов: m - ++n") : 
        base(1, name, description)
    {
        m = new ConsoleDataResponse<double>();
        n = new ConsoleDataResponse<double>();
        
        Actions = new List<ILabEntity<int>>()
        {
            new LabTaskActionBuilder().Id(1).Name("Ввод данных")
                .ExecuteAction(InputData)
                .Build(),
            
            new LabTaskActionBuilder().Id(2).Name("Выполнить задачу")
                .ExecuteAction(() =>
                { 
                    UserDataRequest.Target
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
        UserDataRequest.DisplayMessage = "Введите M: ";
        m = (ConsoleDataResponse<double>)UserDataRequest.Request();
        
        if(m.Code != (int) ConsoleDataResponseCode.ConsoleOk)
            return;
        
        UserDataRequest.DisplayMessage = "Введите N: ";
        n = (ConsoleDataResponse<double>)UserDataRequest.Request(sendRejectMessage: false);
    }

    public void OutputData()
    {
        UserDataRequest.Target
            .Write($"M: {m.Data} \nN: {n.Data}\n");
    }

    public double TaskExpression()
    {
        return m.Data - ++n.Data;
    }
}