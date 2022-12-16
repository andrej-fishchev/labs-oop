using labs.IO;

namespace labs.utils;

public static class ConsoleIoDataUtils
{
    public static ConsoleDataResponse<T> RequestData<T>(ConsoleDataRequest<T> request, string message)
    {
        request.Message = message;
        
        ConsoleDataResponse<T> response;
            
        while((response = (ConsoleDataResponse<T>)request
                  .Request(new DataIoConverter<T>(new ConsoleDataResponse<T>())))
              .Error is { } msg 
              && response.Code != (int) ConsoleDataResponseCode.ConsoleInputRejected)
            request.ConsoleTarget.Write($"Ошибка: {msg} \n");
        
        if (response.Code == (int)ConsoleDataResponseCode.ConsoleInputRejected)
            response.Data = default;
        
        return response;
    }

    public static ConsoleDataResponse<T> 
        RequestDataWithValidator<T>(
            string requestMessage, 
            ConsoleDataRequest<T> request,
            string validatorMessage,
            DataIoValidator<T> validator)
    {
        ConsoleDataResponse<T> value;

        while (!validator.Validate(value = RequestData(request, requestMessage)) 
               && value.Code != (int)ConsoleDataResponseCode.ConsoleInputRejected)
            request.ConsoleTarget.Write($"Ошибка: {validatorMessage} \n");

        return value;
    }
}