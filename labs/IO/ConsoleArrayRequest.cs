using labs.interfaces;
using labs.utils;

namespace labs.IO;

public class ConsoleArrayRequest<T> :
    IDataRequest<T[]>,
    ICloneable
{
    public ConsoleDataResponse<int> ArraySize { get; set; } 
        
    public ConsoleDataRequest<T> NestedRequest { get; init; }
    
    public ConsoleDataValidator<T>? NestedRequestValidator { get; set; }

    public ConsoleDataRequest<int> ArraySizeRequest { get; }

    public ConsoleDataValidator<int>? ArraySizeValidator { get; }
    
    public ConsoleArrayRequest(ConsoleDataRequest<T> elements, ConsoleDataValidator<T>? validator = default, int? size = default)
    {
        NestedRequest = elements;
        NestedRequestValidator = validator;
        
        ArraySizeRequest = new(
            "Введите размер массива: ",new ConsoleDataConverter<int>(
                DataConverterUtils.ToInt));
        
        ArraySizeValidator = new(
            (data) => data > 0, "ожидалось значение больше 0");

        ArraySize = new ConsoleDataResponse<int>(size ?? default);
    }

    public IDataResponse<T[]> Request(IDataValidator<T[]>? validator = default, bool sendRejectMessage = true)
    {
        if (validator != null && validator is not ConsoleDataValidator<T[]>)
            throw new InvalidCastException("Ожидался объект типа ConsoleDataValidator<T[]>");
        
        if(ArraySize.Data == 0)
            ArraySize = (ConsoleDataResponse<int>)
                ArraySizeRequest.Request(ArraySizeValidator, sendRejectMessage: sendRejectMessage);

        if (ArraySize.Code != (int)ConsoleDataResponseCode.ConsoleOk)
            return new ConsoleDataResponse<T[]>(code: ArraySize.Code);

        T[] array = new T[ArraySize.Data];

        ConsoleDataResponse<T> elementBuffer;

        for (int i = 0; i < ArraySize.Data; i++)
        {
            NestedRequest.DisplayMessage = $"Введите {i + 1}'е значение: ";
            
            elementBuffer = (ConsoleDataResponse<T>)
                NestedRequest.Request(NestedRequestValidator, false);

            if (elementBuffer.Code != (int)ConsoleDataResponseCode.ConsoleOk)
                return new ConsoleDataResponse<T[]>(code: elementBuffer.Code);

            array[i] = elementBuffer.Data!;
        }

        return new ConsoleDataResponse<T[]>(array, code: (int)ConsoleDataResponseCode.ConsoleOk);
    }

    public object Clone()
    {
        return new ConsoleArrayRequest<T>(NestedRequest, NestedRequestValidator, ArraySize.Data);
    }
}