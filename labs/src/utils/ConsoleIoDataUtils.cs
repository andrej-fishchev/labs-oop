using labs.interfaces;
using labs.IO;

namespace labs.utils;

public static class ConsoleIoDataUtils
{
    public static ConsoleDataResponse<double> RequestDoubleData(ConsoleDataRequest<double> request, string message)
    {
        request.Message = message;
        
        ConsoleDataResponse<double> response;
            
        while((response = (ConsoleDataResponse<double>)request
                  .Request(new DataIoConverter<double>(new ConsoleDataResponse<double>())))
              .Error is { } msg 
              && response.Code != (int) ConsoleDataResponseCode.CONSOLE_INPUT_REJECTED)
            request.ConsoleTarget.Write(msg);

        if (response.Code == (int)ConsoleDataResponseCode.CONSOLE_INPUT_REJECTED)
            response.Data = 0;
        
        return response;
    }

    public static ConsoleDataResponse<double> 
        RequestDoubleDataWithValidator(
            ConsoleDataRequest<double> request, 
            string message, 
            DataIoValidator<double> validator)
    {
        ConsoleDataResponse<double> value;

        while (!validator.Validate((value = RequestDoubleData(request, message))) 
               && value.Code != (int)ConsoleDataResponseCode.CONSOLE_INPUT_REJECTED)
            request.ConsoleTarget.Write($"Ошибка: значение '{value}' не прошло валидацию");

        return value;
    }
}