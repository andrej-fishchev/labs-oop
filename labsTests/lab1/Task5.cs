namespace labsTests.lab1;

// TODO: #
public class Task5
{
    private labs.lab1.Task5 instance;

    [SetUp]
    public void Init() => 
        instance = new labs.lab1.Task5("test", "test");

    [Test]
    public void DotInTriangle() => 
        Assert.True(labs.lab1.Task5.TriangleContains((0.0, -5.0)));
    
    
    [Test]
    public void DotInCircle() => 
        Assert.True(labs.lab1.Task5.CircleContains((10.0, 0.0)));
}