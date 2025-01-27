using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DominandoEFCore.Models.Converters;

public class CustomConverter : ValueConverter<Status, string>
{
    public CustomConverter() : base(t => ToDataBaseConverter(t), a => ToApplicationConverter(a), new ConverterMappingHints(1))
    {

    }

    static string ToDataBaseConverter(Status status)
    {
        return status.ToString()[0..1];
    }

    static Status ToApplicationConverter(string value)
    {
        var status = Enum.GetValues<Status>().FirstOrDefault(x => x.ToString()[0..1].Equals(value));

        return status;
    }
}