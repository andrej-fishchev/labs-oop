using System.Reflection;
using System.Text;
using System.Text.Json.Nodes;
using IO.targets;
using labs.adapters;
using labs.entities;
using SimpleMenu;
using SimpleMenu.actions;
using SimpleMenu.items;

namespace lab_exec;

public static class Program
{
    public static MenuKeyGenerator<ILabEntity<string>, string> KeyGen = 
        (_, index) => (index + 1).ToString();

    public static string ExitSay = "...";

    public static IList<ILabEntity<string>> labs;

    public static ConsoleMenuAction<ILabEntity<string>> LabEntityMenuAction =
        (ConsoleMenuAction<ILabEntity<string>>)
        new ConsoleMenuActionBuilder<ILabEntity<string>>()
            .OnDisplay(MenuActionDisplay)
            .OnSelect(MenuActionSelect)
            .OnClose(MenuActionClose)
            .Build();

    public static readonly ConsoleTarget ConsoleTarget = new();
    
    public static void Main()
    {
        // TODO: #
        labs = JsonToEntityListAdapter.Adapt(
            ((JsonArray)JsonNode.Parse(File.ReadAllText(
                Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName 
                + "/data/labs.json"))!["labs"]!)!, Test().ToArray());
        
        GetMenu("Лабораторные работы", 
                labs)
            .Display(LabEntityMenuAction);
    }

    public static ConsoleMenu<ILabEntity<string>> GetMenu(string title, IList<ILabEntity<string>> entities)
    {
        return ConsoleMenuFactory.MakeConsoleMenu(title, ExitSay, 
            (ConsoleMenuItemDictionary<ILabEntity<string>>)
            LabEntityToConsoleMenuItemAdapter<ILabEntity<string>>
                .Adapt(entities, KeyGen));
    }

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
        
        IList<ILabEntity<string>> ents =
            (item is LabTask
                ? ((LabTask) item).Actions
                : ((Lab) item).Tasks);

        GetMenu($"{item.Name} \n{item.Description}", ents).Display(LabEntityMenuAction);
    }
    
    public static void MenuActionClose(IMenu<string, ILabEntity<string>> obj)
    {
        ConsoleTarget.Output.WriteLine($"Закрытие: {obj.Title}");
    }

    // TODO: #
    private static IList<Type> Test()
    {
        Assembly assembly = Assembly.LoadFrom("labs.dll");
        Console.Out.WriteLine($"Asm: {assembly.FullName}");

        List<Type> buffer = new List<Type>();
        foreach (var type in assembly.GetTypes())
        {
            if (type.FullName == null || !type.FullName.StartsWith("labs.lab")
                || type.IsAbstract || type.BaseType == null || !type.BaseType.Name.Equals("LabTask"))
                continue;

            Console.Out.WriteLine($"Type: {type.FullName}");
            
            buffer.Add(type);
        }
        
        return buffer;
    }
}