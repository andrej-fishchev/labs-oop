using System.Text;
using labs.builders;
using labs.entities;
using UserDataRequester.requests.console.utils;

namespace labs.tasks.lab17;

public sealed class Task2 : LabTask
{
    private static Task2? instance;

    private string Word { get; set; }
    
    public static Task2 GetInstance(string name = "unknown", string description = "19")
        => instance ??= new Task2(name, description);

    private Task2(string name, string description) :
        base(name, description)
    {
        Word = "";
        
        Actions = new List<ILabEntity<string>>
        {
            new LabTaskActionBuilder().Name("Ввод слова")
                .ExecuteAction(GetWord)
                .Build(),
            
            new LabTaskActionBuilder().Name("Проверка слова без запоминания")
                .ExecuteAction(() => ValidateWord(InputWord()))
                .Build(),
            
            new LabTaskActionBuilder().Name("Проверить принадлежность грамматике")
                .ExecuteAction(() => ValidateWord(Word))
                .Build()
        };
    }

    public static string? InputWord()
    {
        var response = ConsoleBaseRequester.RepeatableGetApprovedData(
            "Введите слово: ",
            terminateString: "..."
        );
        
        if(!response.IsOk() || response.Data() is not string value)
            return null;

        return value;
    }

    public void GetWord() => Word = InputWord() ?? "";

    private static void ValidateWord(string? word) => Console.WriteLine("Слово '{0}' {1} заданной грамматике: \n{2}", 
        word,
        (IsWordCorrect(word, out var path)) ? "принадлежит" : "не принадлежит",
        path
    );
    
    private static bool IsWordCorrect(string? str, out StringBuilder path)
    {
        path = new StringBuilder();

        if (str == null)
            return false;

        var state = "S";            
        var i = 0;

        path.Append(state);
        
        do
        {
            state = state switch
            {
                "S" => str[i] == '_' ? "N" : "E",
                  
                "N" => str[i] switch
                {
                    '-' => "L",
                    '+' => "M",
                    _ => "E"
                },
                  
                "M" => str[i] switch
                {
                    'b' => "KN",
                    '-' => "L",
                    _ => "E"
                },
                  
                "L" => str[i] == 'a' ? "KN" : "E",

                "KN" => str[i] switch
                {
                    '+' => "M",
                    '-' => "L",
                     _ => "E"
                },
                _ => state
            };

            path.Append("->").Append(state);
            
            ++i;
              
        } while(state != "E" && i < str.Length);

        return state == "KN";
    }
}