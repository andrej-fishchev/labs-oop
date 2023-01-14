using IO.responses;
using IO.validators;

namespace IO.converters;

public class ConsoleArrayDataConverter<TOut> :
    IConvertibleData<string?, TOut[]>,
    ICloneable
{
    private string delimiter = ";";

    public string Delimiter
    {
        get => delimiter;
        set
        {
            if (value.Length == 0)
                throw new ArgumentException("Invalid delimiter length, it must be more than zero length");

            delimiter = value;
        }
    }
    
    public IConvertibleData<string?, TOut> ElementConverter { get; set; }
    
    public IValidatableData<TOut>? ElementValidator { get; set; }

    public ConsoleArrayDataConverter(
        IConvertibleData<string?, TOut> nestedConverter,
        IValidatableData<TOut>? nestedValidator = default,
        string delimiter = ";")
    {
        Delimiter = delimiter;
        ElementConverter = nestedConverter;
        ElementValidator = nestedValidator;
    }

    public IResponsibleData<TOut[]> Convert(IResponsibleData<string?> responsibleData)
    {
        if (!responsibleData.IsOk())
            return ConsoleResponseDataFactory.MakeResponse(Array.Empty<TOut>(),
                responsibleData.Error(), (ConsoleResponseDataCode) responsibleData.Code());

        string[] buffer = responsibleData.Data()!
            .Split(Delimiter, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        if (buffer.Length == 0)
            return ConsoleResponseDataFactory.MakeResponse(Array.Empty<TOut>(),
                "не удалось разбить строку на элементы");

        ConsoleResponseData<TOut> element;
        IList<TOut> outs = new List<TOut>();
        
        for (int i = 0; i < buffer.Length; i++)
        {
            element = ElementConverter.Convert(ConsoleResponseDataFactory.MakeResponse(buffer[i])!)
                .As<ConsoleResponseData<TOut>>();

            if (!element.IsOk())
                continue;

            if (ElementValidator != null)
            {
                element = ElementValidator.Validate(element)
                    .As<ConsoleResponseData<TOut>>();

                if (!element.IsOk())
                    continue;
            }
            
            outs.Add(element.Data());
        }

        if (outs.Count == 0)
            return ConsoleResponseDataFactory.MakeResponse(Array.Empty<TOut>(), 
                $"ни один элемент не удалось привести к типу '{typeof(TOut).Name}'");
        
        return ConsoleResponseDataFactory.MakeResponse(outs.ToArray());
    }

    public object Clone()
    {
        return new ConsoleArrayDataConverter<TOut>(
            ElementConverter, ElementValidator, Delimiter);
    }
}