using labs.abstracts;
using labs.interfaces;
using labs.menu;

namespace labs.builders;

public class ConsoleMenuActionBuilder<TV> :
    MenuActionBuilder<string, TV>
{
    public ConsoleMenuActionBuilder(MenuAction<string, TV>? entity = default) : 
        base(entity ?? new ConsoleMenuAction<TV>())
    { }

    public override ConsoleMenuActionBuilder<TV> OnClose(Action<IMenu<string, TV>> value)
    {
        return (ConsoleMenuActionBuilder<TV>) base.OnClose(value);
    }

    public override ConsoleMenuActionBuilder<TV> OnDisplay(Action<IMenu<string, TV>> value)
    {
        return (ConsoleMenuActionBuilder<TV>)base.OnDisplay(value);
    }

    public override ConsoleMenuActionBuilder<TV> OnDraw(Action<IMenu<string, TV>> value)
    {
        return (ConsoleMenuActionBuilder<TV>)base.OnDraw(value);
    }

    public override ConsoleMenuActionBuilder<TV> OnSelect(Action<IMenu<string, TV>, string> value)
    {
        return (ConsoleMenuActionBuilder<TV>)base.OnSelect(value);
    }
}