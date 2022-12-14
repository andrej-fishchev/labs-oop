using labs.interfaces;

namespace labs.abstracts;

// TODO: refactor
public abstract class MenuAction<TK, TV> 
    where TK : notnull
{
    public virtual Action<IMenu<TK, TV>> OnDraw { get; set; }

    public virtual Action<IMenu<TK, TV>> OnDisplay { get; set; }
    
    public virtual Action<IMenu<TK, TV>, TK> OnSelect { get; set; }
    
    public virtual Action<IMenu<TK, TV>> OnClose { get; set; }    
}