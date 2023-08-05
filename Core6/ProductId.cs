using StronglyTypedIds;

namespace Core6;

[StronglyTypedId(
    StronglyTypedIdBackingType.Guid,
    converters: StronglyTypedIdConverter.TypeConverter | StronglyTypedIdConverter.SystemTextJson | StronglyTypedIdConverter.EfCoreValueConverter)]
public partial struct ProductId { }

public record ProductDto (ProductId Id);