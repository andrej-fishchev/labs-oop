namespace labs.lab9.src;

public class TimeArray :
    List_t<Time>
{
    public TimeArray(int capacity = 0) :
        base(capacity)
    { }

    public TimeArray(Time[] data) :
        base(data)
    { }
}