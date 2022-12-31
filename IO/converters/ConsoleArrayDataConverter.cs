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
        string delimiter = ";"
    )
    {
        Delimiter = delimiter;
        ElementConverter = nestedConverter;
        ElementValidator = nestedValidator;
    }

    public IResponsibleData<TOut[]> Convert(IResponsibleData<string?> responsibleData)
    {
        ConsoleResponseData<TOut[]> output = new ConsoleResponseData<TOut[]>(
            error: responsibleData.Error, code: responsibleData.Code);
        
        if (responsibleData.Code != (int)ConsoleResponseDataCode.ConsoleOk 
            || responsibleData.Error != null)
            return output;

        string[] buffer = responsibleData.Data!
            .Split(Delimiter, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        if (buffer.Length == 0)
            return new ConsoleResponseDataBuilder<TOut[]>(output)
                .Code((int)ConsoleResponseDataCode.ConsoleInvalidData)
                .Error($"не удалось разбить строку на элементы")
                .Build();

        IResponsibleData<TOut> element;
        IList<TOut> outs = new List<TOut>();
        
        for (int i = 0; i < buffer.Length; i++)
        {
            if ((element = ElementConverter.Convert(new ConsoleResponseData<string>(buffer[i])!)).Code
                != (int)ConsoleResponseDataCode.ConsoleOk)
                continue;
            
            if(ElementValidator != null 
               && (element = ElementValidator.Validate(element)).Error != null)
                continue;
            
            outs.Add(element.Data);
        }

        if (outs.Count == 0)
            return new ConsoleResponseDataBuilder<TOut[]>(output)
                .Code((int)ConsoleResponseDataCode.ConsoleInvalidData)
                .Error($"ни один элемент не является типом '{typeof(TOut).Name}'")
                .Build();

        output.Data = outs.ToArray();
        output.Error = null;
        
        return output;
    }

    public object Clone()
    {
        return new ConsoleArrayDataConverter<TOut>(
            ElementConverter, ElementValidator, Delimiter);
    }
}