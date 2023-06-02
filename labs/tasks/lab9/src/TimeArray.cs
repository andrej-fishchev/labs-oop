namespace labs.tasks.lab9.src;

public class TimeArray :
    ListT<Time>
{
    public TimeArray(int capacity = 0) :
        base(capacity)
    { }

    public TimeArray(Time[] data) :
        base(data)
    { }
}