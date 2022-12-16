using labs.interfaces;

namespace labs.IO;

public class ConsoleArrayRequest<T> :
    IDataIoRequest<T[]>
{
    public ConsoleDataRequest<T> NestedRequest { get; init; }
    
    public IDataIoValidator<T>? NestedRequestValidator { get; set; }

    public ConsoleDataRequest<uint> ArraySizeRequest { get; set; }

    public ConsoleArrayRequest(
        ConsoleDataRequest<T> elements, IDataIoValidator<T>? validator , ConsoleDataRequest<uint> size)
    {
        NestedRequest = elements;
        NestedRequestValidator = validator;
        ArraySizeRequest = size;
    }
    
    public IDataIoResponse<T[]> Request(IDataIoValidator<T[]>? validator = default, bool sendRejectMessage = true)
    {
        IDataIoResponse<uint> sizeResponse = 
            ArraySizeRequest.Request(sendRejectMessage: sendRejectMessage);

        if (sizeResponse.Code != (int)ConsoleDataResponseCode.ConsoleOk)
            return new ConsoleDataResponse<T[]>(code: sizeResponse.Code);

        T[] array = new T[sizeResponse.Data];

        IDataIoResponse<T> elementBuffer;

        for (int i = 0; i < sizeResponse.Data; i++)
        {
            NestedRequest.DisplayMessage = $"Введите {i + 1}'е значение: ";
            
            elementBuffer = 
                NestedRequest.Request(NestedRequestValidator, false);

            if (elementBuffer.Code != (int)ConsoleDataResponseCode.ConsoleOk)
                return new ConsoleDataResponse<T[]>(code: elementBuffer.Code);

            array[i] = elementBuffer.Data!;
        }

        return new ConsoleDataResponse<T[]>(array, code: (int)ConsoleDataResponseCode.ConsoleOk);
    }
}