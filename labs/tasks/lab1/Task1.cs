using IO.requests;
using IO.responses;
using IO.utils;
using labs.builders;
using labs.entities;

namespace labs.lab1;

// TODO: вынести static методы в класс утилит для текущей лабораторной
public sealed class Task1 : LabTask
{
    private static Task1? instance;
    
    private ConsoleResponseData<double> m;
    private ConsoleResponseData<double> n;

    public static Task1 GetInstance(string name, string description)
    {
        if (instance == null)
            instance = new Task1(name, description);

        return instance;
    }
    
    private Task1(string name = "lab1.task1", string description = "") : 
        base(name, description)
    {
        m = new ConsoleResponseData<double>();
        n = new ConsoleResponseData<double>();
        
        Actions = new List<ILabEntity<string>>
        {
            new LabTaskActionBuilder().Name("Ввод данных")
                .ExecuteAction(InputData)
                .Build(),
            
            new LabTaskActionBuilder().Name("Выполнить задачу")
                .ExecuteAction(() => TaskExpression())
                .Build(),
            
            new LabTaskActionBuilder().Name("Выполнить задачу [с запоминанием]")
                .ExecuteAction(() => TaskExpression(true))
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывод данных")
                .ExecuteAction(() => OutputData(m.Data(), n.Data()))
                .Build()
        };
    }

    public void InputData()
    {
        if (TryReceiveWithNotify(ref m, InputData("Введите M")))
            TryReceiveWithNotify(ref n, InputData("Введите N", false), true);
    }
    
    public void TaskExpression(bool receive = false)
    {
        double left = m.Data();
        double right = n.Data();

        OutputData(left, right);
        
        Target.Output.WriteLine($"m - ++n = {left - ++right}");
        
        OutputData(left, right);

        if (receive)
        {
            m.Data(left);
            n.Data(right);
        }
    }
    
    public static void OutputData(double mValue, double nValue) => 
        Target.Output.WriteLine($"m = {mValue}; n = {nValue}");
    
    public static ConsoleResponseData<double> InputData(string message, bool sendReject = true) => 
        new ConsoleDataRequest<double>(message + $" (от {double.MinValue + 1} до {double.MaxValue}): ")
            .Request(
                BaseTypeDataConverterFactory.MakeDoubleConverterList(), 
                BaseComparableValidatorFactory.MakeInRangeStrictValidator(
                    double.MinValue, 
                    double.MaxValue, 
                    "выход за допустимые границы"
                ), sendReject
            ).As<ConsoleResponseData<double>>();

    // временно полезный трюк
    // TODO: переделать :/
    public static bool TryReceiveWithNotify<T>(
        ref ConsoleResponseData<T> self, 
        ConsoleResponseData<T> value, 
        bool sendOnOk = false
    )
    {
        if(!value.IsOk())
            Target.Output.WriteLine("Ввод прекращен, значения не обновлены");
        
        else if(sendOnOk)
            Target.Output.WriteLine("Ввод завершился успешно");

        if (value.IsOk()) 
            self = value;
        
        return value.IsOk();
    }
}