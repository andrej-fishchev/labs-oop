using System.Text.RegularExpressions;
using IO.converters;
using IO.requests;
using IO.responses;
using labs.builders;
using labs.entities;

namespace labs.lab6;

public sealed class Task2 :
    LabTask
{
    public ConsoleResponseData<string> TaskVariable;

    public readonly ConsoleDataRequest<string> 
        Request = new("Введите произвольную строку: ");

    public readonly ConsoleSimpleDataConverter<string>
        ToStringConverter = ConsoleDataConverterFactory
            .MakeSimpleConverter((string? x, out string y) =>
            {
                y = x!;
                return true;
            });

    public Task2(string name = "lab6.task2", string description = "") : 
        base(2, name, description)
    {
        TaskVariable = new ConsoleResponseData<string>("");

        Actions = new List<ILabEntity<int>>
        {
            new LabTaskActionBuilder().Id(1).Name("Ввод строки")
                .ExecuteAction(InputData)
                .Build(),

            new LabTaskActionBuilder().Id(2).Name("Выполнить задачу")
                .ExecuteAction(() =>
                {
                    Request.Target.Write($"f(): {TaskExpression()} \n");
                })
                .Build(),

            new LabTaskActionBuilder().Id(3).Name("Вывод исходной строки")
                .ExecuteAction(() => Request.Target.Write($"Строка: {TaskVariable.Data} \n"))
                .Build()
        };
    }
    
    public void InputData()
    {
        ConsoleResponseData<string> buffer = (ConsoleResponseData<string>)
            Request.Request(ToStringConverter);
        
        if(buffer.Code != (int) ConsoleResponseDataCode.ConsoleOk)
            return;

        TaskVariable = buffer;
    }
    
    public string TaskExpression()
    {
        if(TaskVariable.Data.Length == 0)
            return TaskVariable.Data;

        Regex? expression;

        if ((expression = TryRegex(@"[A-Za-zА-Яа-я]{1,}")) == null)
        {
            Request.Target.Write($"Ошибка: неверное регулярное выражение");
            return TaskVariable.Data;
        }
        
        IList<string> words = 
            new List<string>();

        MatchCollection matches = 
            expression.Matches(TaskVariable.Data);
        
        for(int i = 0; i < matches.Count; i++)
            words.Add(matches[i].Value);

        string buffer = new string(TaskVariable.Data);

        for (int i = 0, index; i < words.Count; i++)
        {
            index = buffer.IndexOf(words[i], StringComparison.Ordinal);

            if (index == -1)
            {
                words.RemoveAt(i);
                i--;
                continue;
            }

            buffer = buffer.Substring(0, index) 
                     + $"({i})" 
                     + buffer.Substring(index + words[i].Length);
        }

        words = words.Select(x => new string(x.Reverse().ToArray()))
            .OrderBy(x => x.Length)
            .ToList();

        for (int i = 0; i < words.Count; i++)
            buffer = buffer.Replace($"({i})", words[i]);

        return buffer;
    }

    public Regex? TryRegex(string expression)
    {
        Regex? regex;
        try
        {
            regex = new Regex(expression);
        }
        catch (Exception)
        {
            regex = default;
        }

        return regex;
    }
}