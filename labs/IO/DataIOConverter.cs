namespace labs.IO
{
    public delegate TO Converter<in TI, out TO>(TI data, out string? error);
}