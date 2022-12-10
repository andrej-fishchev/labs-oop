using labs.builders;
using labs.entities;

namespace labs.lab1;

public sealed class Task6 :
    LabTask<int>
{
    private (float a, float b) floatData;
    private (double a, double b) doubleData;
    
    public Task6(string name = "lab1.task6", string description = "") 
        : base(6, name)
    {
        doubleData = (1000.0, 0.0001);
        floatData = (1000.0f, 0.0001f);
        
        Description = description;
        
        Actions = new List<LabTaskAction<int>>()
        {
            new LabTaskActionBuilder<int>().Id(1).Name("Выполнить задачу")
                .ExecuteAction(() => Console.WriteLine($"f(float):      {TaskExpression(floatData)} " +
                                                       $"\nf(double):   {TaskExpression(doubleData)}"))
                .Build<LabTaskAction<int>>(),
            
            new LabTaskActionBuilder<int>().Id(2).Name("Вывод данных")
                .ExecuteAction(OutputData)
                .Build<LabTaskAction<int>>()
        };
    }

    public void OutputData()
    {
        Console.WriteLine($"Float: {floatData} " +
                          $"\nDouble: {doubleData}");
    }

    public double TaskExpression((double a, double b) data)
    {
        double xCube;
        Console.WriteLine($"1. pow(a, 3) = {(xCube = Math.Pow(data.a, 3.0)) }");
        
        double yCube;
        Console.WriteLine($"2. pow(y, 3) = { (yCube = Math.Pow(data.b, 3.0)) }");
        
        double cubeDiffXy;
        Console.WriteLine($"3. pow(x-y, 3) = { (cubeDiffXy = Math.Pow((data.a - data.b), 3.0)) }");
        
        double threeQuadraticYAndX;
        Console.WriteLine($"4. 3 * pow(y,2) * x = { (threeQuadraticYAndX = 3 * Math.Pow(data.b, 2.0) * data.a) }");
        
        double threeQuadraticXAndY;
        Console.WriteLine($"5. 3 * pow(x, 2) * y = { (threeQuadraticXAndY = 3 * Math.Pow(data.a, 2.0) * data.b) }");
        
        return (cubeDiffXy - (xCube + threeQuadraticYAndX))
                           /((-threeQuadraticXAndY) - yCube);
    }
    
    public float TaskExpression((float a, float b) data)
    {
        float xCube;
        Console.WriteLine($"1. pow(a, 3) = {(xCube = MathF.Pow(data.a, 3.0f)) }");
        
        float yCube;
        Console.WriteLine($"2. pow(y, 3) = { (yCube = MathF.Pow(data.b, 3.0f)) }");
        
        float cubeDiffXy;
        Console.WriteLine($"3. pow(x-y, 3) = { (cubeDiffXy = MathF.Pow((data.a - data.b), 3.0f)) }");
        
        float threeQuadraticYAndX;
        Console.WriteLine($"4. 3 * pow(y,2) * x = { (threeQuadraticYAndX = 3 * MathF.Pow(data.b, 2.0f) * data.a) }");
        
        float threeQuadraticXAndY;
        Console.WriteLine($"5. 3 * pow(x, 2) * y = { (threeQuadraticXAndY = 3 * MathF.Pow(data.a, 2.0f) * data.b) }");
        
        return (cubeDiffXy - (xCube + threeQuadraticYAndX))
               /((-threeQuadraticXAndY) - yCube);
    }
}