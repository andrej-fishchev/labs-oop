using IO.converters;
using IO.requests;
using IO.responses;
using IO.utils;
using IO.validators;
using labs.builders;
using labs.entities;
using labs.utils;

namespace labs.lab5;

public sealed class Task2 :
    LabTask
{
    public ConsoleResponseData<int[]>[] IntArray 
    { 
        get; private set; 
    }
    
    public Task2(string name = "lab5.task2", string description = "") : 
        base(2, name, description)
    {
        IntArray = new ConsoleResponseData<int[]>[]
        {
            new(Array.Empty<int>())
        };

        Actions = new List<ILabEntity<int>>
        {
            new LabTaskActionBuilder().Id(1).Name("Создать двумерный массив")
                .ExecuteAction(() => InputData(ArrayGenerationType.UserInput))
                .Build(),
            
            new LabTaskActionBuilder().Id(2).Name("Создать двумерный массив [автозаполнение]")
                .ExecuteAction(() => InputData(ArrayGenerationType.Randomizer))
                .Build(),
                
            new LabTaskActionBuilder().Id(3).Name("Добавление строки после строки с наиб. элементом")
                .ExecuteAction(TaskExpression)
                .Build(),

            new LabTaskActionBuilder().Id(4).Name("Вывод массива")
                .ExecuteAction(OutputData)
                .Build()
        };
    }
    
    public void InputData(ArrayGenerationType type)
    {
        ConsoleDataValidator<int> sizeValidator =
            new ConsoleDataValidator<int>(data => data > 0, "ожидалось значение больше 0");
        
        ConsoleResponseData<int> rows = new ConsoleDataRequest<int>("Введите количество строк: ")
            .Request(BaseTypeDataConverterFactory.MakeSimpleIntConverter(), sizeValidator)
            .As<ConsoleResponseData<int>>();

        if(!rows) return;

        switch (type)
        {
            case ArrayGenerationType.UserInput:
                ConsoleResponseData<int[]>[] buffer = 
                    new ConsoleResponseData<int[]>[rows.Data()];

                ConsoleArrayDataConverter<int> converter =
                    BaseTypeArrayDataConverterFactory.MakeIntArrayConverter(delimiter: ";");

                for (int i = 0; i < buffer.Length; i++)
                {
                    Target.Output.WriteLine();

                    buffer[i] = new ConsoleDataRequest<int[]>(
                            $"Введите множество целых чисел (через {converter.Delimiter}): \n")
                        .Request(converter, 
                            ((i == 0) 
                                ? null
                                : new ConsoleDataValidator<int[]>(
                                    data => data.Length == buffer[0].Data().Length,
                                    $"ожидалось '{buffer[0].Data().Length}' элементов")), 
                            false)
                        .As<ConsoleResponseData<int[]>>();

                    if(!buffer[i])
                        return;
                }

                IntArray = buffer;
                break;
            
            case ArrayGenerationType.Randomizer:
                
                ConsoleResponseData<int> columns = new ConsoleDataRequest<int>("Введите количество элементов: ")
                    .Request(BaseTypeDataConverterFactory.MakeSimpleIntConverter(), sizeValidator,false)
                    .As<ConsoleResponseData<int>>();
                
                if(!columns) return;
                
                ConsoleResponseData<int>[] borders = 
                    new ConsoleResponseData<int>[2];

                for (int i = 0; i < borders.Length; i++)
                {
                    borders[i] = (ConsoleResponseData<int>)
                        new ConsoleDataRequest<int>($"Введите {((i == 0) ? "левую" : "правую")} границу ДСЧ: ")
                            .Request(BaseTypeDataConverterFactory.MakeSimpleIntConverter(), 
                                new ConsoleDataValidator<int>(
                                (data) =>
                                {
                                    if (i == 0)
                                        return true;

                                    return data > borders[0].Data();
                                }, "значение правой границы должно быть больше левой"),
                            false);
            
                    if(!borders[i]) return;
                }
                
                IntArray = new ConsoleResponseData<int[]>[rows.Data()];

                Random random = new Random();
                
                for (int i = 0; i < IntArray.Length; i++)
                {
                    IntArray[i] = new ConsoleResponseData<int[]>(new int[columns.Data()]);
                    
                    for(int j = 0; j < IntArray[i].Data().Length; j++)
                        IntArray[i].Data()[j] = random.Next(borders[0].Data(), borders[1].Data());
                }
                
                break;
        }

    }
    
    public void TaskExpression()
    {
        if(IntArray.Length == 0)
            return;
        
        int row = 0;

        for (int i = 0, max = 0, buf; i < IntArray.Length; i++)
        {
            if (i == 0)
            {
                max = IntArray[i].Data().Max();
                continue;
            }

            if (max < (buf = IntArray[i].Data().Max()))
            {
                max = buf;
                row = i;
            }
        }

        row++;

        ConsoleResponseData<int[]>[] buffer = 
            new ConsoleResponseData<int[]>[IntArray.Length + 1];

        ConsoleDataValidator<int[]> validator = new ConsoleDataValidator<int[]>(
            data => data.Length == IntArray[0].Data().Length,
            $"ожижалось '{IntArray[0].Data().Length}' элементов"
        );

        ConsoleArrayDataConverter<int> converter = BaseTypeArrayDataConverterFactory
            .MakeIntArrayConverter();

        for (int i = 0, a = 0; i < buffer.Length; i++)
        {
            if (i == row)
            {        
                buffer[i] = new ConsoleDataRequest<int[]>(
                        $"Введите множество целых значений (через {converter.Delimiter}): \n")
                    .Request(converter, validator)
                    .As<ConsoleResponseData<int[]>>();
                
                if(!buffer[i]) return;
                
                continue;
            }

            buffer[i] = IntArray[a++];
        }

        IntArray = buffer;
        
        Target.Output.WriteLine($"\nСтрока добавлена после {row}'й строки");
    }
    
    public void OutputData()
    {
        for (int i = 0; i < IntArray.Length; i++)
        {
            for (int j = 0; j < IntArray[i].Data().Length; j++)
            {
                Target.Output.Write($"{IntArray[i].Data()[j]}");
                
                if(j + 1 != IntArray[i].Data().Length)
                    Target.Output.WriteLine("\t");
            }
            
            Target.Output.WriteLine();
        }
        
        if(IntArray.Length == 0)
            Target.Output.WriteLine("Массив пуст");
    }
}