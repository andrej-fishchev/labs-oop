namespace lab_exec;

public interface IMenuDisplay<TK, TV> 
    where TK : notnull
{
    public void Display(MenuAction<TK, TV> actions);
}