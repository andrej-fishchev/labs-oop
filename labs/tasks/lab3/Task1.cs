using System.Text;
using labs.builders;
using labs.entities;

namespace labs.lab3;

public sealed class Task1 : LabTask
{
    private static Task1? instance;
    
    private static readonly int n = 10;
    
    private static readonly double a = 0.1D;

    private static readonly double b = 1D;

    private static readonly double k = 10;

    private static readonly double e = 0.0001;
    
    public static Task1 GetInstance(string name, string description)
    {
        if (instance == null)
            instance = new Task1(name, description);

        return instance;
    }
    
    private Task1(string name, string description) : 
        base(name, description)
    {
        Actions = new List<ILabEntity<string>>
        {
             new LabTaskActionBuilder().Name("Выполнить задание")
                .ExecuteAction(TaskExpression)
                .Build(),
             
             new LabTaskActionBuilder().Name("Исходные данные")
             .ExecuteAction(About)
             .Build()
        };
    }

    public void About()
    {
        Target.Output.WriteLine(
            new StringBuilder()
            .Append("f(x): y = 3^x \n")
            .Append("xn = f(xn-1) -> xn-1 + (a - b) / k \n")
            .Append($"a = {a} \n")
            .Append($"b = {b} \n")
            .Append($"k = {k} \n")
            .Append($"e = {e} \n")
            .Append($"n = {n}")
            .ToString()
        );
    }

    public void TaskExpression()
    {
        for (double x = a; x < b; x = GetNextX(x))
            Target.Output.WriteLine($"x = {x}; y = {Math.Pow(3, x)}; SN = {GetSn(x)}; SE = {GetSe(x)};");
    }

    public double GetSn(double x)
    {
        double sn = 1, l = GetNextLn(1);
        
        for (int i = 1, f = 1; i < n; i++, l = GetNextLn(l), f *= i)
            sn += GetResultOfExpression(l , f,  Math.Pow(x, i));

        return sn;
    }
    
    public double GetSe(double x)
    {
        int i = 0, 
            f = 1;
        
        double res, 
            l = 1, 
            se = 0;

        while ((res = GetResultOfExpression(l, f, Math.Pow(x, i))) >= e)
        {
            se += res;
            
            i++;
                
            l = GetNextLn(l);
            
            f *= i;
        }
    
        return se;
    }

    public static double GetResultOfExpression(double ln, double fact, double pow) =>
        ln / fact * pow;
    
    public static double GetNextX(double x) => x + (b - a) / k;

    public static double GetNextLn(double value) => value * Math.Log(3);
}