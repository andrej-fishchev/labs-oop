using IO.converters;
using IO.requests;
using IO.responses;
using IO.utils;
using IO.validators;
using labs.builders;
using labs.entities;
using labs.utils;
using Random = System.Random;

namespace labs.lab4;

public sealed class Task1 : LabTask
{
    private static Task1? instance;
    
    public ConsoleResponseData<int[]> IntArray
    {
        get; 
        private set;
    }
    
    public static Task1 GetInstance(string name, string description)
    {
        if (instance == null)
            instance = new Task1(name, description);

        return instance;
    }

    private Task1(string name, string description) :
        base(name, description)
    {
        IntArray = new ConsoleResponseData<int[]>(Array.Empty<int>());
        
        Actions = new List<ILabEntity<string>>
        {
            new LabTaskActionBuilder().Name("Создать массив")
                .ExecuteAction(() => InputData(ArrayGenerationType.UserInput))
                .Build(),
            
            new LabTaskActionBuilder().Name("Создать массив [автозаполнение]")
                .ExecuteAction(() => InputData(ArrayGenerationType.Randomizer))
                .Build(),
                
            new LabTaskActionBuilder().Name("Удалить элементы с четными индексами")
                .ExecuteAction(DeleteElements)
                .Build(),

            new LabTaskActionBuilder().Name("Добавить элемент с номером K")
                .ExecuteAction(AddElement)
                .Build(),
            
            new LabTaskActionBuilder().Name("Циклический сдвиг")
                .ExecuteAction(CyclicShift)
                .Build(),
            
            new LabTaskActionBuilder().Name("Найти элемент - среднее арифметическое элементов")
                .ExecuteAction(SearchElement)
                .Build(),
            
            new LabTaskActionBuilder().Name("Сортировка вставкой")
                .ExecuteAction(() =>
                {
                    if(IsValueZeroWithNotify(IntArray.Data().Length, "Ожидается создание массива"))
                        return;

                    IntArray.Data(InsertSort(IntArray.Data()));
                    
                    OutputData();
                })
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывод массива")
                .ExecuteAction(OutputData)
                .Build(),
        };
    }

    public void InputData(ArrayGenerationType type)
    {
        ConsoleArrayDataConverter<int> converter =
            BaseTypeArrayDataConverterFactory.MakeIntArrayConverter(delimiter: ";");

        if (type == ArrayGenerationType.UserInput)
        {
            ConsoleResponseData<int[]> buffer = new ConsoleDataRequest<int[]>(
                    $"Введите множество целых чисел (через '{converter.Delimiter}'):")
                .Request(converter)
                .As<ConsoleResponseData<int[]>>();

            if (lab1.Task1.TryReceiveWithNotify(ref buffer, buffer, true))
                IntArray = buffer;
            
            return;
        }

        ConsoleDataRequest<int> request = 
            new ConsoleDataRequest<int>("Введите резмер массива: ");

        ConsoleResponseData<int> size = request
            .Request(converter.ElementConverter, BaseComparableValidatorFactory
                .MakeComparableGreaterThanValidator(0, "значение должно быть больше 0"))
            .As<ConsoleResponseData<int>>();
        
        if(!lab1.Task1.TryReceiveWithNotify(ref size, size))
            return;

        ConsoleResponseData<int>[] borders = 
            new ConsoleResponseData<int>[2];

        for (int i = 0; i < borders.Length; i++)
        {
            request.DisplayMessage = 
                $"Введите {((i == 0) ? "левую" : "правую")} границу ДСЧ: ";

            var index = i;
            borders[i] = request
                .Request(converter.ElementConverter, new ConsoleDataValidator<int>(
                        data =>
                        {
                            if (index == 0)
                                return true;

                            return data > borders[0].Data();
                        }, "значение правой границы должно быть больше левой"),
                    false)
                .As<ConsoleResponseData<int>>();
            
            if(!lab1.Task1.TryReceiveWithNotify(ref borders[i], borders[i], i != 0)) 
                return;
        }

        IntArray.Data(new int[size.Data()]);
        
        Random random = new Random();

        for (int i = 0; i < IntArray.Data().Length; i++)
            IntArray.Data()[i] = random.Next(borders[0].Data(), borders[1].Data());
    }

    public void DeleteElements()
    {
        if(IsValueZeroWithNotify(IntArray.Data().Length, "Ожидается создаение массива"))
            return;
        
        IntArray.Data(IntArray.Data().Where((_, index) => (index % 2) != 0).ToArray());
        
        OutputData();
    }

    public void AddElement()
    {
        if(IsValueZeroWithNotify(IntArray.Data().Length, "Ожидается создаение массива"))
            return;
        
        OutputData();

        ConsoleResponseData<int> elementToAdd = new ConsoleDataRequest<int>("Введите номер элемента: ")
            .Request(BaseTypeDataConverterFactory.MakeSimpleIntConverter(), BaseComparableValidatorFactory
                .MakeInRangeNotStrictValidator(1, IntArray.Data().Length,
                $"значение не может быть меньше 1 и больше {IntArray.Data().Length}"))
            .As<ConsoleResponseData<int>>();
        
        if(!elementToAdd)
            return;

        int[] buffer = new int[IntArray.Data().Length + 1];

        for (int i = 0; i < IntArray.Data().Length; i++)
            buffer[i] = IntArray.Data()[i];

        buffer[^1] = buffer[elementToAdd.Data()-1];
        
        IntArray.Data(buffer);
        
        OutputData();
    }

    public void SearchElement()
    {
        if(IsValueZeroWithNotify(IntArray.Data().Length, "Ожидается создаение массива"))
            return;

        int sum = (int) IntArray.Data().Average();

        int result = -1;
        for (int i = 0; i < IntArray.Data().Length; i++)
        {
            if (IntArray.Data()[i] == sum)
            {
                result = i;
                break;
            }
        }

        Target.Output.WriteLine("Элемент - среднее арифметическое суммы элементов массива: " +
            $"{((result == -1) ? "не найден" : $"{IntArray.Data()[result].ToString()}")}"
        );
    }

    public int[] InsertSort(int[] array)
    {
        bool f;
        
        for(int i = 1, a, j; i < array.Length; i++)
        {
            a = array[i];
            j = i - 1;
            f = false;
            while(j >= 0 && !f)
            {
                if(!(f = array[j] < a))
                {
                    array[j+1] = array[j];
                    array[j] = a;
                    j--;
                }
            }
        }

        return array;
    }

    public void CyclicShift()
    {
        if(IsValueZeroWithNotify(IntArray.Data().Length, "Ожидается создаение массива"))
            return;
        
        ConsoleResponseData<int> shiftPower = 
            new ConsoleDataRequest<int>("Введите силу сдвига [+/- напрвление]: ")
                .Request(BaseTypeDataConverterFactory.MakeSimpleIntConverter())
                .As<ConsoleResponseData<int>>();
        
        if(!shiftPower) return;

        shiftPower.Data(shiftPower.Data() % IntArray.Data().Length);
        
        if(shiftPower.Data() == 0)
            return;

        IntArray.Data(IntArray.Data()
            .Select((x, index) =>
                new { x, nextIndex = ShiftingExpression(index, shiftPower.Data(), IntArray.Data().Length) })
            .OrderBy(x => x.nextIndex)
            .Select(x => x.x)
            .ToArray());
        
        OutputData();
    }

    public void OutputData()
    {
        for (int i = 0; i < IntArray.Data().Length; i++)
            Target.Output.WriteLine($"{i + 1}: {IntArray.Data()[i]}");
        
        if(IntArray.Data().Length == 0)
            Target.Output.WriteLine("Массив пуст");
    }

    public int ShiftingExpression(int index, int shiftPower, int length) => 
        (length + shiftPower + index) % length;

    public static bool IsValueZeroWithNotify(int value, string msg)
    {
        if(value == 0)
            Target.Output.WriteLine(msg);

        return value == 0;
    }
}