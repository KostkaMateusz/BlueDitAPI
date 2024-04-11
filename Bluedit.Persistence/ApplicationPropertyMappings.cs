using Bluedit.Application.DataModels.PostDtos;
using Bluedit.Application.DataModels.TopicDtos;
using Bluedit.Domain.Entities;
using Bluedit.Persistence.Helpers.Sorting;

namespace Bluedit.Persistence;

public class ApplicationPropertyMappings
{
    private readonly Dictionary<string, PropertyMappingValue> _authorPropertyMapping =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "TopicName", new PropertyMappingValue(new[] { "TopicName" }) },
            { "TopicDescription", new PropertyMappingValue(new[] { "TopicDescription" }) },
            { "PostCount", new PropertyMappingValue(new[] { "PostCount" }, true) }
        };

    public ApplicationPropertyMappings()
    {
        PropertyMappings.Add(new PropertyMapping<TopicInfoDto, Topic>(_authorPropertyMapping));
        PropertyMappings.Add(new PropertyMapping<PostInfoDto, Post>(AutoMapAll(typeof(Post))));
    }

    public IList<IPropertyMapping> PropertyMappings { get; } = new List<IPropertyMapping>();

    private static Dictionary<string, PropertyMappingValue> AutoMapAll(Type objectType)
    {
        Dictionary<string, PropertyMappingValue> propertyMapping = new(StringComparer.OrdinalIgnoreCase);

        var properties = objectType.GetProperties();

        foreach (var property in properties)
            propertyMapping.Add($"{property.Name}", new PropertyMappingValue(new[] { $"{property.Name}" }, true));
        return propertyMapping;
    }
}