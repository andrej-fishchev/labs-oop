using System.Text;
using IO.requests;
using IO.responses;
using IO.utils;
using labs.builders;
using labs.entities;

namespace labs.lab1;

public sealed class Task1 : LabTask
{
    private ConsoleResponseData<double> m;
    private ConsoleResponseData<double> n;

    public Task1(string name = "lab1.task1", string description = "") : 
        base(1, name, description)
    {
        m = new ConsoleResponseData<double>();
        n = new ConsoleResponseData<double>();
        
        Actions = new List<ILabEntity<int>>()
        {
            new LabTaskActionBuilder().Name("Ввод данных")
                .ExecuteAction(InputData)
                .Build(),
            
            new LabTaskActionBuilder().Name("Выполнить задачу")
                .ExecuteAction(() => TaskExpression())
                .Build(),
            
            new LabTaskActionBuilder().Name("Выполнить задачу [с запоминанием]")
                .ExecuteAction(() => TaskExpression(true))
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывод данных")
                .ExecuteAction(OutputData)
                .Build()
        };

    }

    public void InputData()
    {
        m = InputData("Введите M: ");
        
        if(!m) return;
        
        n = InputData("Введите N: ", false);
    }

    public void OutputData()
    {
        Target.Output.WriteLine($"M: {m.Data()} \nN: {n.Data()}");
    }

    private ConsoleResponseData<double> InputData(string message, bool sendReject = true)
    {
        return new ConsoleDataRequest<double>(message)
            .Request(BaseTypeDataConverterFactory.MakeDoubleConverterList(), 
                sendRejectMessage: sendReject)
            .As<ConsoleResponseData<double>>();
    }

    public void TaskExpression(bool receive = false)
    {
        double left = m.Data();
        double right = n.Data();

        StringBuilder builder = 
            new StringBuilder($"f(m, n): {left - ++right} \n")
                .Append($"M: {left} \n")
                .Append($"N: {right}");
        
        Target.Output.WriteLine(builder.ToString());

        if (receive)
        {
            m |= left;
            n |= right;
        }
    }
}