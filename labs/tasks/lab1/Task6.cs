using labs.builders;
using labs.entities;

namespace labs.lab1;

public sealed class Task6 :
    LabTask
{
    private readonly (float a, float b) floatData;
    private readonly (double a, double b) doubleData;
    
    public Task6(string name = "lab1.task6", string description = "") 
        : base(6, name)
    {
        doubleData = (1000.0, 0.0001);
        floatData = (1000.0f, 0.0001f);
        
        Description = description;
        
        Actions = new List<ILabEntity<int>>
        {
            new LabTaskActionBuilder().Name("Выполнить задачу")
                .ExecuteAction(() => Target.Output
                    .WriteLine($"f(float): {TaskExpression(floatData)}\nf(double): {TaskExpression(doubleData)}"))
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывод данных")
                .ExecuteAction(OutputData)
                .Build()
        };
    }

    public void OutputData() => 
        Target.Output.WriteLine($"Float: {floatData} \nDouble: {doubleData}");

    public double TaskExpression((double a, double b) data)
    {
        double xCube;
        Target.Output.WriteLine($"1. pow(a, 3) = {(xCube = Math.Pow(data.a, 3.0)) }");
        
        double yCube;
        Target.Output.WriteLine($"2. pow(b, 3) = { (yCube = Math.Pow(data.b, 3.0)) }");
        
        double cubeDiffXy;
        Target.Output.WriteLine($"3. pow(a-b, 3) = { (cubeDiffXy = Math.Pow((data.a - data.b), 3.0)) }");
        
        double threeQuadraticYAndX;
        Target.Output.WriteLine($"4. 3 * pow(b,2) * a = { (threeQuadraticYAndX = 3 * Math.Pow(data.b, 2.0) * data.a) }");
        
        double threeQuadraticXAndY;
        Target.Output.WriteLine($"5. 3 * pow(a, 2) * b = { (threeQuadraticXAndY = 3 * Math.Pow(data.a, 2.0) * data.b) } \n");
        
        return (cubeDiffXy - (xCube + threeQuadraticYAndX))
                           /((-threeQuadraticXAndY) - yCube);
    }
    
    public float TaskExpression((float a, float b) data)
    {
        float xCube;
        Target.Output.WriteLine($"1. pow(a, 3) = {(xCube = MathF.Pow(data.a, 3.0f)) }");
        
        float yCube;
        Target.Output.WriteLine($"2. pow(b, 3) = { (yCube = MathF.Pow(data.b, 3.0f)) }");
        
        float cubeDiffXy;
        Target.Output.WriteLine($"3. pow(a-b, 3) = { (cubeDiffXy = MathF.Pow((data.a - data.b), 3.0f)) }");
        
        float threeQuadraticYAndX;
        Target.Output.WriteLine($"4. 3 * pow(b,2) * a = { (threeQuadraticYAndX = 3 * MathF.Pow(data.b, 2.0f) * data.a) }");
        
        float threeQuadraticXAndY;
        Target.Output.WriteLine($"5. 3 * pow(a, 2) * b = { (threeQuadraticXAndY = 3 * MathF.Pow(data.a, 2.0f) * data.b) } \n");
        
        return (cubeDiffXy - (xCube + threeQuadraticYAndX))
               /((-threeQuadraticXAndY) - yCube);
    }
}