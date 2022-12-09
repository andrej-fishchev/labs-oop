using labs.builders;
using labs.entities;
using labs.IO;

namespace labs.lab1;

public sealed class Task1 :
    LabTask<int>
{
    private double m_M;
    private double m_N;

    private static readonly ConsoleDataRequest<double> m_UserDataRequest =
        new("", (string? data, out string? error) =>
        {
            error = null;

            double value;

            if (!double.TryParse(data, out value))
                error = $"ожидалось вещественное число, но получено {data}";

            return value;
        });

    public Task1(string name = "lab1.task1", string description = "") 
        : base(1, name)
    {
        m_M = m_N = default;
        
        Description = description;
        
        Actions = new List<LabTaskAction<int>>()
        {
            new LabTaskActionBuilder<int>().Id(1).Name("Ввод данных")
                .ExecuteAction(InputData)
                .Build<LabTaskAction<int>>(),
            
            new LabTaskActionBuilder<int>().Id(2).Name("Выполнить задачу")
                .ExecuteAction(() => Console.WriteLine($"f(): {TaskExpression(ref m_M, ref m_N)}"))
                .Build<LabTaskAction<int>>(),
            
            new LabTaskActionBuilder<int>().Id(3).Name("Вывод данных")
                .ExecuteAction(OutputData)
                .Build<LabTaskAction<int>>()
        };
    }

    public void InputData()
    {
        m_UserDataRequest.ConsoleTarget
            .Write($"Ввод может быть прекращен в любое удобное время." +
                   $"Введите '{m_UserDataRequest.RejectInputMessage}' для незамедлительного прекращения исполнения задачи");
        
        m_M = RequestData($"Введите M: ");
        m_N = RequestData("Введите N: ");
    }

    public double RequestData(string message)
    {
        m_UserDataRequest.Message = message;
        
        ConsoleDataResponse<double> response;
            
        while((response = (ConsoleDataResponse<double>)m_UserDataRequest
                  .Request(new DataIoConverter<double>(new ConsoleDataResponse<double>())))
                  .Error is { } msg 
                  && response.Code != (int) ConsoleDataResponseCode.CONSOLE_INPUT_REJECTED)
            m_UserDataRequest.ConsoleTarget.Write(msg);

        if (response.Code == (int)ConsoleDataResponseCode.CONSOLE_INPUT_REJECTED)
            response.Data = 0;
        
        return response.Data;
    }

    public void OutputData()
    {
        Console.WriteLine($"M: {m_M} \nN: {m_N}");
    }

    public double TaskExpression(ref double m, ref double n)
    {
        return m - ++n;
    }
}