namespace lab_exec;

public sealed class ConsoleMenu<TV> :
    Menu<string, TV>,
    IMenuDisplay<string, TV>
{
    public override string ExitButton { get; set; }

    public ConsoleMenu(string title, string exitButton) :
        base(title)
    {
        ExitButton = (exitButton.Length == 0) 
            ? throw new InvalidDataException("длина значения кнопки выхода не может быть 0")
            : exitButton;
    }

    public override void AddItem(string itemKey, TV itemValue)
    {
        if(itemKey.Equals(ExitButton))
            return;
        
        base.AddItem(itemKey, itemValue);
    }

    public void Display(MenuAction<string, TV> actions)
    {
        string? say;
        
        actions.OnDraw.Invoke(this);
        
        do
        {
            if((say = Console.ReadLine()) == null)
                break;

            if(ContainsItem(say))
                actions.OnSelect.Invoke(this, say);


        } while (!say.Trim().Equals(ExitButton));
        
        actions.OnClose.Invoke(this);
    }
}