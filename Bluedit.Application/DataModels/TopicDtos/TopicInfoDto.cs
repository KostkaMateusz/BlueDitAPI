
namespace Bluedit.Application.DataModels.TopicDtos;

public class TopicInfoDto
{
    public required string TopicName { get; set; }
    public required string TopicDescription { get; set; }
    public int PostCount { get; set; }
}
