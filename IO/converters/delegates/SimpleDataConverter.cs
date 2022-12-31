namespace IO.converters.delegates;

public delegate bool SimpleDataConverter<in TI, TO>(TI data, out TO output);