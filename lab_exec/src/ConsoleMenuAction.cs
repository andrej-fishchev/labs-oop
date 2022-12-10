namespace lab_exec;

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
        Action<Menu<string, TV>> onDisplay, 
        Action<Menu<string, TV>> onDraw, 
        Action<Menu<string, TV>, string> onSelect, 
        Action<Menu<string, TV>> onClose)
    {
        OnDisplay = onDisplay;
        OnDraw = onDraw;
        OnSelect = onSelect;
        OnClose = onClose;
    }

    public override Action<Menu<string, TV>> OnDisplay { get; set; }
    
    public override Action<Menu<string, TV>> OnDraw { get; set; }
    
    public override Action<Menu<string, TV>, string> OnSelect { get; set; }
    
    public override Action<Menu<string, TV>> OnClose { get; set; }
}