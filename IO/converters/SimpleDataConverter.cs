namespace IO.converters
{
    public delegate bool SimpleDataConverter<in TI, TO>(TI data, out TO output);
}