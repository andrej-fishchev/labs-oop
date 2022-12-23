using SimpleMenu.items;

namespace SimpleMenu;

public static class ConsoleMenuFactory
{
    public static ConsoleMenu<TV> 
        MakeConsoleMenu<TV>(string title, string exitButton, ConsoleMenuItemDictionary<TV> items)
    {
        return (ConsoleMenu<TV>) 
            new ConsoleMenuBuilder<TV>()
                .Title(title)
                .Exit(exitButton)
                .Items(items)
                .Build();
    }
}