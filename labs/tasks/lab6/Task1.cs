using IO.converters;
using IO.requests;
using IO.responses;
using IO.utils;
using IO.validators;
using labs.builders;
using labs.entities;
using labs.utils;

namespace labs.lab6;

public sealed class Task1 :
    LabTask
{
    public ConsoleResponseData<double[]>[] IntArray
    {
        get; private set;
    }

    private readonly ConsoleDataRequest<double[]> 
        arrayRequest = new("");
    
    private readonly ConsoleArrayDataConverter<double>
        toDoubleArrayConverter;

    public Task1(string name = "lab6.task1", string description = "") : 
        base(1, name, description)
    {
        toDoubleArrayConverter = BaseTypeArrayDataConverterFactory
            .MakeDoubleArrayConverterList();

        IntArray = new ConsoleResponseData<double[]>[]
        {
            new(Array.Empty<double>())
        };

        Actions = new List<ILabEntity<int>>
        {
            new LabTaskActionBuilder().Name("Создать массив")
                .ExecuteAction(() => InputData(ArrayGenerationType.UserInput))
                .Build(),
            
            new LabTaskActionBuilder().Name("Создать массив [автозаполнение]")
                .ExecuteAction(() => InputData(ArrayGenerationType.Randomizer))
                .Build(),
                
            new LabTaskActionBuilder().Name("Сортировка элементов и строк по возрастанию")
                .ExecuteAction(TaskExpression)
                .Build(),

            new LabTaskActionBuilder().Name("Вывод массива")
                .ExecuteAction(OutputData)
                .Build()
        };
    }
    
    public void InputData(ArrayGenerationType type)
    {
        ConsoleResponseData<int> rows = 
            InitIntNumberRequest(GetIntNumberRequest("Введите количество строк: "),
            new ConsoleDataValidator<int>(data => data > 0,
                "ожидается значение больше 0"));

        if(!lab1.Task1.TryReceiveWithNotify(ref rows, rows)) 
            return;

        ConsoleResponseData<double[]>[] buffer = 
            new ConsoleResponseData<double[]>[rows.Data()];
        
        switch (type)
        {
            case ArrayGenerationType.UserInput:
                arrayRequest.DisplayMessage =
                    $"Введите множество вещественных чисел через ({toDoubleArrayConverter.Delimiter}): \n";
                
                for (int i = 0; i < buffer.Length; i++)
                {
                    buffer[i] = arrayRequest
                        .Request(toDoubleArrayConverter, sendRejectMessage: false)
                        .As<ConsoleResponseData<double[]>>();

                    if(!lab1.Task1.TryReceiveWithNotify(ref buffer[i], buffer[i], 
                           i + 1 == buffer.Length)) 
                        return;
                }
                
                break;
            
            case ArrayGenerationType.Randomizer:

                ConsoleResponseData<int> columns;
                for (int i = 0; i < rows.Data(); i++)
                {
                    columns = InitIntNumberRequest(
                        GetIntNumberRequest($"Введите размер для {i + 1}'й строки: "),
                        new ConsoleDataValidator<int>(data => data > 0, 
                            "ожидалось значение больше 0"), false);

                    if(!lab1.Task1.TryReceiveWithNotify(ref columns, columns)) 
                        return;

                    buffer[i] = new ConsoleResponseData<double[]>(new double[columns.Data()]);
                }
                
                ConsoleResponseData<int>[] borders = 
                    new ConsoleResponseData<int>[2];
                
                for (int i = 0; i < borders.Length; i++)
                {
                    borders[i] = InitIntNumberRequest(
                        GetIntNumberRequest($"Введите {((i == 0) ? "левую" : "правую")} границу ДСЧ: "),
                        ((i == 0)
                            ? null
                            : new ConsoleDataValidator<int>(data => data > borders[0].Data(), 
                                "значение правой границы ДСЧ не может быть больше или равно левой")), 
                    false);
                    
                    if(!lab1.Task1.TryReceiveWithNotify(ref borders[i], borders[i], i != 0)) 
                        return;
                }
                
                Random random = new Random();
                
                for (int i = 0; i < buffer.Length; i++)
                    for(int j = 0; j < buffer[i].Data().Length; j++)
                        buffer[i].Data()[j] = random.Next(borders[0].Data(), borders[1].Data());
                
                break;
        }

        IntArray = buffer;
    }
    
    public void TaskExpression()
    {
        if(lab4.Task1.IsValueZeroWithNotify(IntArray[0].Data().Length, 
               "Ожидается создание массива"))
            return;

        IntArray = IntArray.OrderBy(x => x.Data().Length)
            .ToArray();

        for (int i = 0; i < IntArray.Length; i++)
            Array.Sort(IntArray[i].Data());
        
        OutputData();
    }

    public void OutputData()
    {
        if (IntArray[0].Data().Length == 0)
        {
            Target.Output.WriteLine("Массив пуст");
            return;
        }

        for (int i = 0; i < IntArray.Length; i++)
        {
            for (int j = 0; j < IntArray[i].Data().Length; j++)
            {
                Target.Output.Write(
                    $"{IntArray[i].Data()[j]}");
                
                if(j + 1 != IntArray[i].Data().Length)
                    Target.Output.Write("\t");
            }
            
            Target.Output.WriteLine();
        }
    }
    
    public ConsoleResponseData<int> InitIntNumberRequest(ConsoleDataRequest<int> request, 
        ConsoleDataValidator<int>? validator = default, bool send = true)
    {
        return request
            .Request(BaseTypeDataConverterFactory.MakeSimpleIntConverter(), validator, send)
            .As<ConsoleResponseData<int>>();
    }

    public ConsoleDataRequest<int> GetIntNumberRequest(string message)
    {
        return ConsoleDataRequestFactory.MakeConsoleDataRequest<int>(message);
    }
}