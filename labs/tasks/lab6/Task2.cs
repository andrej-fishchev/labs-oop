using System.Text.RegularExpressions;
using IO.requests;
using IO.responses;
using IO.utils;
using labs.builders;
using labs.entities;

namespace labs.lab6;

public sealed class Task2 : LabTask
{
    private static Task2? instance;
    
    public ConsoleResponseData<string> TaskVariable;

    public static Task2 GetInstance(string name, string description)
    {
        if (instance == null)
            instance = new Task2(name, description);

        return instance;
    }
    
    private Task2(string name, string description) : 
        base(name, description)
    {
        TaskVariable = new ConsoleResponseData<string>("");

        Actions = new List<ILabEntity<string>>
        {
            new LabTaskActionBuilder().Name("Ввод строки")
                .ExecuteAction(InputData)
                .Build(),

            new LabTaskActionBuilder().Name("Выполнить задачу")
                .ExecuteAction(() => Target.Output.
                    WriteLine(TaskExpression()))
                .Build(),

            new LabTaskActionBuilder().Name("Вывод исходной строки")
                .ExecuteAction(() =>
                {
                    string output = TaskVariable.Data();
                    
                    if (output.Length == 0)
                        output = "Пустая строка";
                    
                    Target.Output.WriteLine($"Строка: {output}");
                })
                .Build()
        };
    }
    
    public void InputData()
    {
        ConsoleResponseData<string> buffer = new ConsoleDataRequest<string>("Введите произвольную строку: ")
            .Request(BaseTypeDataConverterFactory.MakeSimpleStringConverter())
            .As<ConsoleResponseData<string>>();
        
        if(!lab1.Task1.TryReceiveWithNotify(ref buffer, buffer, true)) 
            return;

        TaskVariable = buffer;
    }
    
    public string TaskExpression()
    {
        if(TaskVariable.Data().Length == 0)
            return "Пустая строка";

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