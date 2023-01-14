namespace labs.lab9.src;

public class Time :
    IComparable,
    ICloneable
{
    public static int Objects
    {
        get; 
        private set;
    }

    
    private int minutes;
    private int hours;
    
    public int Minutes
    {
        get => minutes;
        set
        {
            ThrowableInRage(0, value, 60);

            minutes = value;
        }
    }

    public int Hours
    {
        get => hours;
        set
        {
            ThrowableInRage(0, value, Int32.MaxValue);

            hours = value;
        }
    }

    public Time(int seconds = 0)
    {
        ThrowableInRage(0, seconds, Int32.MaxValue);

        Hours = seconds / 3600;
        Minutes = (seconds % 3600) / 60;

        Objects++;
    }

    public Time(int hours = 0, int minutes = 0)
    {
        Hours = hours;
        Minutes = minutes;

        Objects++;
    }
    
    public Time() :
        this(0, 0)
    {}

    public Time(int[] data) :
        this(data[0], data[1])
    { }

    ~Time()
    {
        if (Objects > 0)
            Objects--;
    }
    
    public static bool TryParse(string? data, out Time obj)
    {
        obj = new Time();

        if (data == null)
            return false;
        
        string[] buff = data.Split(":");

        if (buff.Length != 2)
            return false;

        if (!int.TryParse(buff[0], out var hours))
            return false;

        if (!int.TryParse(buff[1], out var minutes))
            return false;

        try
        {
            Time bufferedTime = 
                new Time(hours, minutes);

            obj = bufferedTime;
        }
        catch (ArgumentException)
        {
            return false;
        }

        return true;
    }

    public int AsSeconds()
    {
        return Hours * 3600 + Minutes * 60;
    }
    
    // left <= x < right 
    private void ThrowableInRage(int left, int value, int right)
    {
        if (value < left || value >= right)
            throw new ArgumentOutOfRangeException();
    }

    public static Time operator -(Time self, int minutes)
    {
        return new Time(self.AsSeconds() - minutes * 60);
    }

    public static Time operator --(Time self)
    {
        return (self - 1);
    }

    public static explicit operator int(Time self)
    {
        return self.Hours;
    }

    public static implicit operator bool(Time self)
    {
        return self.Hours != 0 && self.Minutes != 0;
    }

    public static Time operator +(Time self, int minutes)
    {
        return new Time(self.AsSeconds() + minutes * 60);
    }

    public static Time operator +(int minutes, Time self)
    {
        return self + minutes;
    }

    public static Time operator +(Time left, Time right)
    {
        return new Time(left.AsSeconds() + right.AsSeconds());
    }
    
    public static Time operator -(Time left, Time right)
    {
        return new Time(left.AsSeconds() - right.AsSeconds());
    }
    
    public override string ToString()
    {
        return $"{Hours}:{Minutes}";
    }

    public int CompareTo(object? obj)
    {
        if (obj == null || obj is not Time)
            throw new ArgumentException();

        Time timeObj = (Time)obj;

        if (timeObj.AsSeconds() == AsSeconds())
            return 0;

        return AsSeconds() < timeObj.AsSeconds() ? -1 : 1;
    }

    public object Clone()
    {
        return new Time(AsSeconds());
    }
}