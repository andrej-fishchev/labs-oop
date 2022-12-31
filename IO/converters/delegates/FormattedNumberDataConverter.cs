using System.Globalization;

namespace IO.converters.delegates;

public delegate bool FormattedNumberDataConverter<in TI, TO>(
    TI data, 
    NumberStyles styles,
    IFormatProvider? provider, 
    out TO output);