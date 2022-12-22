namespace labs.delegates
{
    public delegate TO DataConverter<in TI, out TO>(TI data, out string? error);    
}