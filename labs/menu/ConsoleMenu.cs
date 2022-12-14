using labs.abstracts;
using labs.interfaces;

namespace labs.menu;

public sealed class ConsoleMenu<TV> :
    IMenu<string, TV>
{
    private string exitButton;

    public string Title { get; set; }

    public string Exit
    {
        get => exitButton;
        set
        {
            if (value.Length == 0)
                throw new InvalidDataException("длина значения кнопки выхода не может быть 0");

            exitButton = value;
        }
    }

    public IMenuItemDictionary<string, TV> Items { get; set; }

    public ConsoleMenu(string title = "", string exit = " ", ConsoleMenuItemDictionary<TV>? items = default)
    {
        exitButton = "";
        
        Title = title;
        Exit = exit;
        Items = items ?? new ConsoleMenuItemDictionary<TV>();
    }

    public void Display(MenuAction<string, TV> actions)
    {
        string? say;
        
        actions.OnDraw.Invoke(this);
        
        do
        {
            actions.OnDisplay.Invoke(this);
            
            if((say = Console.ReadLine()) == null)
                break;

            if(Items.ContainsKey(say))
                actions.OnSelect.Invoke(this, say);
            
        } while (!say.Trim().Equals(Exit));
        
        actions.OnClose.Invoke(this);
    }
}