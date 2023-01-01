using IO.requests;
using IO.responses;
using IO.targets;
using IO.utils;
using labs.builders;
using labs.entities;

namespace labs.lab1;

public sealed class Task1 : LabTask
{
    private ConsoleResponseData<double> m;
    private ConsoleResponseData<double> n;

    public Task1(string name = "lab1.task1", 
        string description = "Вычислить значение выражения и его аргументов: m - ++n") : 
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
                    Target.Write($"f(): {TaskExpression()} \n");
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
        m = InputData("Введите M: ");
        
        if(m.Code != (int) ConsoleResponseDataCode.ConsoleOk)
            return;
        
        n = InputData("Введите N: ", false);
    }

    public void OutputData()
    {
        Target.Write($"M: {m.Data} \nN: {n.Data}\n");
    }

    private ConsoleResponseData<double> InputData(string message, bool sendReject = true)
    {
        return (ConsoleResponseData<double>) new ConsoleDataRequest<double>(message)
            .Request(BaseTypeDataConverterFactory
                    .MakeDoubleConverterList(), 
                sendRejectMessage: sendReject);
    }

    public double TaskExpression()
    {
        return m.Data - ++n.Data;
    }
}