using System.Numerics;
using labs.lab1;

namespace labsTests.lab1;

public class Task5
{
    [Test]
    public void DotInTriangle()
    {
        Assert.True(labs.lab1.Task5.Triangle.Contains(new Vector<double>(
            new []{ 0.0, -5.0 })));
    }
}