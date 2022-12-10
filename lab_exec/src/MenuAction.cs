namespace lab_exec;

public abstract class MenuAction<TK, TV> where TK : notnull
{
    public virtual Action<Menu<TK, TV>> OnDraw { get; set; }

    public virtual Action<Menu<TK, TV>> OnDisplay { get; set; }
    
    public virtual Action<Menu<TK, TV>, TK> OnSelect { get; set; }
    
    public virtual Action<Menu<TK, TV>> OnClose { get; set; }    
}