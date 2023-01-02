using System.Text.RegularExpressions;
using IO.requests;
using IO.responses;
using IO.utils;
using labs.builders;
using labs.entities;

namespace labs.lab6;

public sealed class Task2 :
    LabTask
{
    public ConsoleResponseData<string> TaskVariable;

    public Task2(string name = "lab6.task2", string description = "") : 
        base(2, name, description)
    {
        TaskVariable = new ConsoleResponseData<string>("");

        Actions = new List<ILabEntity<int>>
        {
            new LabTaskActionBuilder().Name("Ввод строки")
                .ExecuteAction(InputData)
                .Build(),

            new LabTaskActionBuilder().Name("Выполнить задачу")
                .ExecuteAction(() => Target.Output.
                    WriteLine($"f(): {TaskExpression()}"))
                .Build(),

            new LabTaskActionBuilder().Name("Вывод исходной строки")
                .ExecuteAction(() => Target.Output
                    .WriteLine($"Строка: {TaskVariable.Data()}"))
                .Build()
        };
    }
    
    public void InputData()
    {
        ConsoleResponseData<string> buffer = new ConsoleDataRequest<string>("Введите произвольную строку: ")
            .Request(BaseTypeDataConverterFactory.MakeSimpleStringConverter())
            .As<ConsoleResponseData<string>>();
        
        if(!buffer) return;

        TaskVariable = buffer;
    }
    
    public string TaskExpression()
    {
        if(TaskVariable.Data().Length == 0)
            return TaskVariable.Data();

        Regex? expression;

        if ((expression = TryRegex(@"[A-Za-zА-Яа-я]{1,}")) == null)
        {
            Target.Output.WriteLine("Ошибка: неверное регулярное выражение");
            return TaskVariable.Data();
        }
        
        IList<string> words = 
            new List<string>();

        MatchCollection matches = 
            expression.Matches(TaskVariable.Data());
        
        for(int i = 0; i < matches.Count; i++)
            words.Add(matches[i].Value);

        string buffer = new string(TaskVariable.Data());

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