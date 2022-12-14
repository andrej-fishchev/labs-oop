using labs.abstracts;
using labs.interfaces;

namespace labs.menu;

public sealed class ConsoleMenuAction<TV> :
    MenuAction<string, TV>
{
    public ConsoleMenuAction()
    {
        OnDisplay = OnDraw = OnClose 
            = _ => { };

        OnSelect = (_, _) => { };
    }
    
    public ConsoleMenuAction(
        Action<IMenu<string, TV>> onDisplay, 
        Action<IMenu<string, TV>> onDraw, 
        Action<IMenu<string, TV>, string> onSelect, 
        Action<IMenu<string, TV>> onClose)
    {
        OnDisplay = onDisplay;
        OnDraw = onDraw;
        OnSelect = onSelect;
        OnClose = onClose;
    }

    public override Action<IMenu<string, TV>> OnDisplay { get; set; }
    
    public override Action<IMenu<string, TV>> OnDraw { get; set; }
    
    public override Action<IMenu<string, TV>, string> OnSelect { get; set; }
    
    public override Action<IMenu<string, TV>> OnClose { get; set; }
}