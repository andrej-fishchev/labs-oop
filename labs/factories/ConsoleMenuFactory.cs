using labs.adapters;
using labs.builders;
using labs.menu;

namespace labs.factories;

public static class ConsoleMenuFactory
{
    public static ConsoleMenu<TV> 
        MakeConsoleMenu<TV>(string title, string exitButton, MenuKeyGenerator<TV, string> keyGenerator, List<TV> entities)
    {
        return (ConsoleMenu<TV>)
            new ConsoleMenuBuilder<TV>()
                .Title(title)
                .Exit(exitButton)
                .Items(LabEntityToConsoleMenuItemAdapter<TV>
                    .Adapt(entities, keyGenerator))
            .Build();
    }
}