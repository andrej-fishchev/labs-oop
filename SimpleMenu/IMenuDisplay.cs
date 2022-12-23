using SimpleMenu.actions;

namespace SimpleMenu;

public interface IMenuDisplay<TK, TV> 
    where TK : notnull
{
    public void Display(MenuAction<TK, TV> actions);
}