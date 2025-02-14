using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Converter;

internal sealed class UlidToGuidConverter(ConverterMappingHints mappingHints = null!) : ValueConverter<Ulid, Guid>(
        convertToProviderExpression: x => x.ToGuid(),
        convertFromProviderExpression: x => new Ulid(x),
        mappingHints: defaultHints.With(mappingHints))
{
    private static readonly ConverterMappingHints defaultHints = new ConverterMappingHints(size: 16);

    public UlidToGuidConverter() : this(null!)
    {
    }
}
