using SimpleMenu.actions;
using SimpleMenu.items;

namespace SimpleMenu;

public sealed class ConsoleMenu<TV> :
    IMenu<string, TV>,
    IMenuDisplay<string, TV>
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
        bool close;
        actions.OnDraw.Invoke(this);
        
        do
        {
            actions.OnDisplay.Invoke(this);
            
            if((say = Console.ReadLine()) == null)
                break;

            say = say.Trim();

            close = say.Equals(Exit);
            
            if(!close && Items.ContainsKey(say))
                actions.OnSelect.Invoke(this, say);
            
            // TODO: избавиться от константных строк
            else if(!close)
                Console.WriteLine($"Идентификатор '{say}' не распознан, повторите ввод");

        } while (!close);
        
        actions.OnClose.Invoke(this);
    }
}