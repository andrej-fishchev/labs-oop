using System.Text;
using labs.adapters;
using labs.builders;
using labs.entities;
using labs.factories;
using labs.interfaces;
using labs.menu;

namespace lab_exec;

public static class Program
{
    public static MenuKeyGenerator<ILabEntity<int>, string> KeyGen = 
        (ent, _) => ent.Id.ToString();

    public static string ExitSay = "---";
    
    public static ConsoleMenuAction<ILabEntity<int>> LabEntityMenuAction =
        (ConsoleMenuAction<ILabEntity<int>>)
        new ConsoleMenuActionBuilder<ILabEntity<int>>()
            .OnDisplay(MenuActionDisplay)
            .OnSelect(MenuActionSelect)
            .OnClose(MenuActionClose)
            .Build();


    public static void Main()
    {
        GetMenu("Лабораторные работы", MetaData.LabList)
            .Display(LabEntityMenuAction);
    }

    public static ConsoleMenu<ILabEntity<int>> GetMenu(string name, List<ILabEntity<int>> entities)
    {
        return ConsoleMenuFactory.MakeConsoleMenu(
                name,
                ExitSay,
                KeyGen,
                entities
        );
    }

    public static void MenuActionDisplay(IMenu<string, ILabEntity<int>> obj)
    {
        StringBuilder builder =
            new StringBuilder($"\n\n{obj.Title}: \n");

        foreach (var keyValuePair in obj.Items)
            builder.Append($"{keyValuePair.Key}. {keyValuePair.Value.Name} \n");
        
        Console.WriteLine(builder
            .Append($"\nSay '{obj.Exit}' to exit ... \n\nОжидается ввод: ")
            .ToString());
    }
    
    public static void MenuActionSelect(IMenu<string, ILabEntity<int>> arg1, string arg2)
    {
        ILabEntity<int> item = arg1.Items[arg2];
        
        if (item is LabTaskAction action)
        {
            action.Execute();
            return;
        }
        
        List<ILabEntity<int>> ents = (List<ILabEntity<int>>)
            (item is LabTask
                ? ((LabTask) item).Actions
                : ((Lab) item).Tasks);
        
        string name = (item is Lab)
            ? "Список задач"
            : "Список действий";
        
        GetMenu(name, ents).Display(LabEntityMenuAction);
    }
    
    public static void MenuActionClose(IMenu<string, ILabEntity<int>> obj)
    {
        Console.WriteLine($"Закрытие: {obj.Title}");
    }
}