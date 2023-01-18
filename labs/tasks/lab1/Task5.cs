using IO.requests;
using IO.responses;
using IO.utils;
using labs.builders;
using labs.entities;

namespace labs.lab1;

public sealed class Task5 :
    LabTask
{
    private ConsoleResponseData<double> x;
    private ConsoleResponseData<double> y;

    public Task5(string name = "lab1.task5", string description = "") 
        : base(5, name, description)
    {
        x = new ConsoleResponseData<double>(); 
        y = new ConsoleResponseData<double>();
        
        Description = description;
        
        Actions = new List<ILabEntity<int>>
        {
            new LabTaskActionBuilder().Name("Ввод данных")
                .ExecuteAction(InputData)
                .Build(),
            
            new LabTaskActionBuilder().Name("Выполнить задачу")
                .ExecuteAction(() => Target.Output
                    .WriteLine($"Принадлежит{(x.Data(), y.Data())}? { TaskExpression((x.Data(), y.Data())) }"))
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывод данных")
                .ExecuteAction(OutputData)
                .Build()
        };
    }

    public void InputData()
    {
        if(Task1.TryReceiveWithNotify(ref x, InputData("Введите значение X координаты: ")))
            Task1.TryReceiveWithNotify(ref y, 
                InputData("Введите значение Y координаты", false), true);
    }
    
    private static ConsoleResponseData<double> InputData(string message, bool sendReject = true) => 
        new ConsoleDataRequest<double>(message)
            .Request(BaseTypeDataConverterFactory.MakeDoubleConverterList(), sendRejectMessage: sendReject)
            .As<ConsoleResponseData<double>>();

    public void OutputData() => Target.Output.WriteLine($"Точка: {(x.Data(), y.Data())}");

    public bool TaskExpression((double x, double y) point) => 
        CircleContains(point) || TriangleContains(point);

    public static bool CircleContains((double x, double y) point)
    {
        const double radius = 5.0;
        
        (double x, double y) centerPoint = (5.0, 0.0);
        
        return Math.Pow(point.x - centerPoint.x, 2.0) + Math.Pow(point.y - centerPoint.y, 2.0)
               <= Math.Pow(radius, 2.0);
    }

    public static bool TriangleContains((double x, double y) point)
    {
        (double x, double y) a = (0, 5);
        (double x, double y) b = (10, 0);
        (double x, double y) c = (0, -5);

        if (point == a || point == b || point == c)
            return true;
        
        // vec: AB
        (double x, double y) vecAB = (b.x - a.x, b.y - b.x);
        
        // vec: APoint
        (double x, double y) vecAPoint = (point.x - a.x, point.y - a.y);
        
        // vec: CB
        (double x, double y) vecCB = (b.x - c.x, b.y - c.y);
        
        // vec: CPoint
        (double x, double y) vecCPoint = (point.x - c.x, point.y - c.y);

        // product of matrix
        //  AB | x y |
        //  AP | x y |
        double matrixABValue = vecAB.x * vecAPoint.y - vecAPoint.x * vecAB.y;
        
        // product of matrix
        //  CB | x y |
        //  CP | x y |
        double matrixCBValue = vecCB.x * vecCPoint.y - vecCPoint.x * vecCB.y;

        
        // matrixValue < 0 - точка лежит в нижней полуплоскости
        // matrixValue > 0 - точка лежит в верхней полуплоскости
        // matrixValue == 0 - точка принадлежит прямой
        return point.x >= 0 && matrixABValue <= 0 && matrixCBValue >= 0;
    }
}