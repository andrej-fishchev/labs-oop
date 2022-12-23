using SimpleMenu.items;

namespace SimpleMenu;

public class ConsoleMenuBuilder<TV> :
    MenuBuilder<string, TV>
{
    public ConsoleMenuBuilder(ConsoleMenu<TV>? entity = default) 
        : base(entity ?? new ConsoleMenu<TV>())
    { }

    public override ConsoleMenuBuilder<TV> Items(IMenuItemDictionary<string, TV> value)
    {
        return (ConsoleMenuBuilder<TV>) base.Items(value);
    }

    public override ConsoleMenuBuilder<TV> Title(string value)
    {
        return (ConsoleMenuBuilder<TV>) base.Title(value);
    }

    public override ConsoleMenuBuilder<TV> Exit(string value)
    {
        return (ConsoleMenuBuilder<TV>) base.Exit(value);
    }
}