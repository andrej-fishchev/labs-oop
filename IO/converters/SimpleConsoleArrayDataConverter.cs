using IO.responses;
using IO.validators;

namespace IO.converters;

public class SimpleConsoleArrayDataConverter<TOut> :
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

    public SimpleConsoleArrayDataConverter(
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
        {
            output.Error = "не удалось выполнить разбиение строки на элементы";
            output.Code = (int) ConsoleResponseDataCode.ConsoleInvalidData;
            
            return output;
        }

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

        output.Data = outs.ToArray();
        output.Error = null;
        
        return output;
    }

    public object Clone()
    {
        return new SimpleConsoleArrayDataConverter<TOut>(
            ElementConverter, ElementValidator, Delimiter);
    }
}