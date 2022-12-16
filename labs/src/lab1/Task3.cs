using labs.abstracts;
using labs.builders;
using labs.entities;
using labs.interfaces;
using labs.IO;
using labs.utils;

namespace labs.lab1;

public sealed class Task3 :
    LabTask
{
    private ConsoleDataResponse<double> m;
    private ConsoleDataResponse<double> n;

    private static readonly ConsoleDataRequest<double> UserDataRequest =
        new("", new DataIoConverter<string?, double>(
            DataConverterUtils.ToDoubleWithInvariant, new ConsoleDataResponse<double>()));
    
    public Task3(string name = "lab1.task3", 
        string description = "Вычислить значение выражения и его аргументов: (m-- < ++n)") : 
        base(3, name, description)
    {
        m = new ConsoleDataResponse<double>();
        n = new ConsoleDataResponse<double>();
        
        Description = description;
        
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
                }).Build(),
            
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

    public bool TaskExpression()
    {
        return (m.Data-- < ++n.Data);
    }
}