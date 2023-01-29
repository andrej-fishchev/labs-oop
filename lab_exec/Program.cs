using System.Reflection;
using System.Text;
using IO.targets;
using lab_exec.loader;
using labs.adapters;
using labs.entities;
using SimpleMenu;
using SimpleMenu.actions;
using SimpleMenu.items;

namespace lab_exec;

public static class Program
{
    public static readonly ConsoleTarget ConsoleTarget = new();
    
    public static ConsoleMenuAction<ILabEntity<string>> LabEntityMenuAction =
        (ConsoleMenuAction<ILabEntity<string>>)
        new ConsoleMenuActionBuilder<ILabEntity<string>>()
            .OnDisplay(MenuActionDisplay)
            .OnSelect(MenuActionSelect)
            .OnClose(MenuActionClose)
            .Build();
    
    public static void Main(string[] args)
    {
        Console.Out.WriteLine("v = {0}", args[0]);

        GetMenu("Лабораторные работы", new LabNodeLoader(args[0])
                .TryLoadTasksFromMemory(Assembly.LoadFrom("labs.dll")))
            .Display(LabEntityMenuAction);
    }

    public static ConsoleMenu<ILabEntity<string>> GetMenu(string title, IList<ILabEntity<string>> entities) => 
        ConsoleMenuFactory.MakeConsoleMenu(title, "...", 
            LabEntityToConsoleMenuItemAdapter<ILabEntity<string>>
                .Adapt(entities, (_, index) => (index + 1).ToString())
                .As<ConsoleMenuItemDictionary<ILabEntity<string>>>());

    public static void MenuActionDisplay(IMenu<string, ILabEntity<string>> obj)
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
    
    public static void MenuActionSelect(IMenu<string, ILabEntity<string>> arg1, string arg2)
    {
        ILabEntity<string> item = arg1.Items[arg2];
        
        if (item is LabTaskAction action)
        {
            action.Execute();
            return;
        }
        
        IList<ILabEntity<string>> ents = item is LabTask
            ? item.As<LabTask>().Actions
            : item.As<Lab>().Tasks;

        GetMenu($"{item.Name} \n{item.Description}", ents).Display(LabEntityMenuAction);
    }
    
    public static void MenuActionClose(IMenu<string, ILabEntity<string>> obj) => 
        ConsoleTarget.Output.WriteLine($"Закрытие: {obj.Title}");
}