namespace labs.utils;

public class Circle
{
    public (double x, double y) Center { get; set; }
    
    public  double Radius { get; set; }

    public Circle(double radius = 5.0, (double x, double y) center = default)
    {
        Center = center;
        Radius = radius;
    }

    public bool Contains((double x, double y) dot)
    {
        return Math.Pow(dot.x - Center.x, 2.0) + Math.Pow(dot.y - Center.y, 2.0)
               <= Math.Pow(Radius, 2.0);
    }
}