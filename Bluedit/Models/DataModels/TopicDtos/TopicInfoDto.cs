
namespace Bluedit.Models.DataModels.TopicDtos;

public record TopicInfoDto
{
    public required string TopicName { get; set; }
    public required string TopicDescription { get; set; }
    public int PostCount { get; set; }
}
