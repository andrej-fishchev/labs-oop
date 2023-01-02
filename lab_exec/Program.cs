using System.Text;
using IO.targets;
using labs.adapters;
using labs.entities;
using SimpleMenu;
using SimpleMenu.actions;
using SimpleMenu.items;

namespace lab_exec;

public static class Program
{
    public static MenuKeyGenerator<ILabEntity<int>, string> KeyGen = 
        (_, index) => (index + 1).ToString();

    public static string ExitSay = "...";
    
    public static ConsoleMenuAction<ILabEntity<int>> LabEntityMenuAction =
        (ConsoleMenuAction<ILabEntity<int>>)
        new ConsoleMenuActionBuilder<ILabEntity<int>>()
            .OnDisplay(MenuActionDisplay)
            .OnSelect(MenuActionSelect)
            .OnClose(MenuActionClose)
            .Build();

    public static readonly ConsoleTarget ConsoleTarget = new();
    
    public static void Main()
    {
        GetMenu("Лабораторные работы", 
                MetaData.LabList)
            .Display(LabEntityMenuAction);
    }

    public static ConsoleMenu<ILabEntity<int>> GetMenu(string title, IList<ILabEntity<int>> entities)
    {
        return ConsoleMenuFactory.MakeConsoleMenu(title, ExitSay, 
            (ConsoleMenuItemDictionary<ILabEntity<int>>)
            LabEntityToConsoleMenuItemAdapter<ILabEntity<int>>
                .Adapt(entities, KeyGen));
    }

    public static void MenuActionDisplay(IMenu<string, ILabEntity<int>> obj)
    {
        StringBuilder builder =
            new StringBuilder($"\n\n{obj.Title}: \n");

        foreach (var keyValuePair in obj.Items)
            builder.Append($"{keyValuePair.Key}. {keyValuePair.Value.Name} \n");

        if (obj.Items.Count == 0)
            builder.Append("Список пуст \n");
        
        ConsoleTarget.Output.Write(
            builder.Append($"\nВведите '{obj.Exit}' для закрытия \n\nОжидается ввод: ")
                .ToString()
        );
    }
    
    public static void MenuActionSelect(IMenu<string, ILabEntity<int>> arg1, string arg2)
    {
        ILabEntity<int> item = arg1.Items[arg2];
        
        if (item is LabTaskAction action)
        {
            action.Execute();
            return;
        }
        
        IList<ILabEntity<int>> ents =
            (item is LabTask
                ? ((LabTask) item).Actions
                : ((Lab) item).Tasks);

        GetMenu($"{item.Name} \n{item.Description}", ents).Display(LabEntityMenuAction);
    }
    
    public static void MenuActionClose(IMenu<string, ILabEntity<int>> obj)
    {
        ConsoleTarget.Output.WriteLine($"Закрытие: {obj.Title}");
    }
}