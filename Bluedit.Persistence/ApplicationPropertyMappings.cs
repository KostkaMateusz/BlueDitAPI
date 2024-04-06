using System.Reflection;
using Bluedit.Application.DataModels.PostDtos;
using Bluedit.Application.DataModels.TopicDtos;
using Bluedit.Domain.Entities;
using Bluedit.Persistence.Helpers.Sorting;

namespace Bluedit.Persistence;

public class ApplicationPropertyMappings
{
    public IList<IPropertyMapping> PropertyMappings { get; } = new List<IPropertyMapping>();
    
    private readonly Dictionary<string, PropertyMappingValue> _authorPropertyMapping = new(StringComparer.OrdinalIgnoreCase)
    {
        { "TopicName", new(new[] { "TopicName" }) },
        { "TopicDescription", new(new[] { "TopicDescription" }) },
        { "PostCount", new(new[] { "PostCount" }, true) },
    };

    private static Dictionary<string, PropertyMappingValue> AutoMapAll(Type objectType)
    {
        Dictionary<string, PropertyMappingValue> propertyMapping = new(StringComparer.OrdinalIgnoreCase);
        
        PropertyInfo[] properties = objectType.GetProperties();

        foreach (var property in properties)
        {
            propertyMapping.Add( $"{property.Name}", new PropertyMappingValue(new[] { $"{property.Name}"}, true) );
        }
        return propertyMapping;
    }

    public ApplicationPropertyMappings()
    {
        PropertyMappings.Add(new PropertyMapping<TopicInfoDto, Topic>(_authorPropertyMapping));
        PropertyMappings.Add(new PropertyMapping<PostInfoDto, Post>(AutoMapAll(typeof(Post))));
    }
}