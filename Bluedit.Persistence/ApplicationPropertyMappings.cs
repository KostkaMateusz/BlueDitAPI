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

    public ApplicationPropertyMappings()
    {
        PropertyMappings.Add(new PropertyMapping<TopicInfoDto, Topic>(_authorPropertyMapping));
    }
}