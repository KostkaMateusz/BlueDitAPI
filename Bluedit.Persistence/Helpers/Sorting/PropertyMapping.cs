﻿namespace Bluedit.Persistence.Helpers.Sorting;

public class PropertyMapping<TSource, TDestination> : IPropertyMapping
{
    public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingDictionary)
    {
        MappingDictionary = mappingDictionary ?? throw new ArgumentNullException(nameof(mappingDictionary));
    }

    public Dictionary<string, PropertyMappingValue> MappingDictionary { get; private set; }
}