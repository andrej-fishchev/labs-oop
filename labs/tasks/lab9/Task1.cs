using IO.converters;
using IO.requests;
using IO.responses;
using IO.utils;
using IO.validators;
using labs.builders;
using labs.entities;
using labs.lab9.src;
using labs.utils;

namespace labs.lab9;

public sealed class Task1 : LabTask
{
    private ConsoleResponseData<TimeArray> Times { get; set; }

    public Task1(string name = "lab9.task1", string description = "") : 
        base(1, name, description)
    {
        Times = new ConsoleResponseData<TimeArray>(new TimeArray());
        
        Actions = new List<ILabEntity<int>>()
        {
            new LabTaskActionBuilder().Name("Ввод множества объектов Time")
                .ExecuteAction(() => InputData(ArrayGenerationType.UserInput))
                .Build(),
            
            new LabTaskActionBuilder().Name("Ввод множества объектов Time [автозаполнение]")
                .ExecuteAction(() => InputData(ArrayGenerationType.Randomizer))
                .Build(),
            
            new LabTaskActionBuilder().Name("Добавление объекта Time")
                .ExecuteAction(AddItem)
                .Build(),
            
            new LabTaskActionBuilder().Name("Удаление объекта Time")
                .ExecuteAction(RemoveItem)
                .Build(),
            
            new LabTaskActionBuilder().Name("Добавление/Вычитание минут к I-му объекту Time")
                .ExecuteAction(SetMinutesToTimeObject)
                .Build(),

            new LabTaskActionBuilder().Name("Добавление времени к I-му объекту Time")
                .ExecuteAction(AddTimeToTimeObject)
                .Build(),
            
            new LabTaskActionBuilder().Name("Вычитание времени из I-го объекта Time")
                .ExecuteAction(RemoveTimeFromTimeObject)
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывод максимального объекта Time")
                .ExecuteAction(DisplayMaxTimeObject)
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывод количества объектов типа Time")
                .ExecuteAction(() => Target.Output
                    .WriteLine($"Счетчик: {Time.Objects}"))
                .Build(),
            
            new LabTaskActionBuilder().Name("Вывод множества объектов Time")
                .ExecuteAction(OutputData)
                .Build()
        };

    }

    public void InputData(ArrayGenerationType type)
    {
        ConsoleArrayDataConverter<Time> converter = ConsoleDataConverterFactory
            .MakeArrayConverter(ConsoleDataConverterFactory
                .MakeSimpleConverter<Time>(Time.TryParse), delimiter: ";");

        if (type == ArrayGenerationType.UserInput)
        {
            ConsoleResponseData<Time[]> data = (ConsoleResponseData<Time[]>)
                new ConsoleDataRequest<Time[]>(
                    $"Введите множество 'hh:mm' (через '{converter.Delimiter}'): \n")
                .Request(converter);

            if (data) Times = ConsoleResponseDataFactory
                    .MakeResponse(new TimeArray(data.Data()));

            return;
        }

        ConsoleResponseData<int> size = new ConsoleDataRequest<int>("Введите количество элементов массива: ")
            .Request(BaseTypeDataConverterFactory.MakeSimpleIntConverter(), new ConsoleDataValidator<int>(
                data => data > 0, "ожидалось значение больше 0"))
            .As<ConsoleResponseData<int>>();
        
        if(!size) return;

        Random random = new Random();

        Times.Data(new TimeArray(size.Data()));

        for (int i = 0; i < Times.Data().Count; i++)
            Times.Data()[i] = new Time(random.Next(0, Int32.MaxValue % 604800));
    }

    public void AddItem()
    {
        ConsoleResponseData<Time> buffer = new ConsoleDataRequest<Time>("Введите время (формат hh:mm): ")
            .Request(ConsoleDataConverterFactory.MakeSimpleConverter<Time>(Time.TryParse))
            .As<ConsoleResponseData<Time>>();
        
        if(buffer) Times.Data().Add(buffer.Data());
    }

    public void RemoveItem()
    {
        ConsoleResponseData<int> buffer = RequestIntData(
            $"Введите номер элемента [1; {Times.Data().Count}]: ", GetIndexValidator());

        if(buffer) Times.Data().RemoveAt(buffer.Data() - 1);
    }
    
    public void SetMinutesToTimeObject()
    {
        ConsoleResponseData<int> buffer = RequestIntData($"Введите номер элемента [1; {Times.Data().Count}]: ", 
            GetIndexValidator());
        
        if(!buffer) return;

        ConsoleResponseData<int> minutes = 
            RequestIntData("Введите количество минут [+/- - добавить/вычесть]: ", 
                GetMinuteValidator(Times.Data()[buffer.Data() - 1]), false);
        
        if(minutes) Times.Data()[buffer.Data() - 1] += minutes.Data();
    }

    public void DisplayMaxTimeObject()
    {
        Time? max = Times.Data().Max();

        if (max == null)
        {
            Target.Output.WriteLine("Ошибка: Не удалось получить максимальный объект типа Time");
            return;
        }
        
        Target.Output.WriteLine($"Максимальное значение: {max}");
    }
    
    public void RemoveTimeFromTimeObject()
    {
        ConsoleResponseData<int> buffer = RequestIntData(
            $"Введите номер элемента [1; {Times.Data().Count}]: ", 
            GetIndexValidator());
        
        if(!buffer) return;

        ConsoleResponseData<Time> time = new ConsoleDataRequest<Time>(
                "Введите отнимаемое время (формат hh:mm): ")
            .Request(ConsoleDataConverterFactory.MakeSimpleConverter<Time>(Time.TryParse),
                new ConsoleDataValidator<Time>(
                    data => data.AsSeconds() <= Times.Data()[buffer.Data() - 1].AsSeconds(), 
                    $"не может превышать '{Times.Data()[buffer.Data() - 1]}'"), 
                false)
            .As<ConsoleResponseData<Time>>();
        
        if(time) Times.Data()[buffer.Data() - 1] -= time.Data();
    }
    
    public void AddTimeToTimeObject()
    {
        ConsoleResponseData<int> buffer = RequestIntData($"Выберите объект [1; {Times.Data().Count}]: ", 
            GetIndexValidator());
        
        if(!buffer) return;

        ConsoleResponseData<Time> time = new ConsoleDataRequest<Time>(
                "Введите добавочное время (формат hh:mm): ")
            .Request(ConsoleDataConverterFactory
                .MakeSimpleConverter<Time>(Time.TryParse), sendRejectMessage: false)
            .As<ConsoleResponseData<Time>>();
        
        if(time) Times.Data()[buffer.Data() - 1] += time.Data();
    }

    private ConsoleResponseData<int> 
        RequestIntData(string message, IValidatableData<int>? validator = default, bool send = true)
    {
        OutputData();
        
        Target.Output.WriteLine();
        
        return new ConsoleDataRequest<int>(message)
            .Request(BaseTypeDataConverterFactory.MakeSimpleIntConverter(), validator, send)
            .As<ConsoleResponseData<int>>();
    }

    private IValidatableData<int> GetIndexValidator()
    {
        return BaseComparableValidatorFactory.MakeInRangeNotStrictValidator(
            1,Times.Data().Count, "выход за допустимые границы");
    }

    private IValidatableData<int> GetMinuteValidator(Time obj)
    {
        const Int32 left = 0;
        Int32 right = obj.AsSeconds() / 60;
        
        return BaseComparableValidatorFactory.MakeInRangeNotStrictValidator(
            left, right, $"значение не может превышать '{obj.AsSeconds() / 60}' минут");
    }

    public void OutputData()
    {
        for (int i = 0; i < Times.Data().Count; i++)
            Target.Output.WriteLine($"{i + 1}: {Times.Data()[i]}");
        
        if(Times.Data().Count == 0)
            Target.Output.WriteLine("Массив пуст");
    }
}