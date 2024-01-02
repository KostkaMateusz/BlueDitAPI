namespace Bluedit.Application.Features.TopicFeatures.Queries.GetTopic;

public class GetTopicQueryResponse
{
    public required string TopicName { get; set; }
    public required string TopicDescription { get; set; }
    public int PostCount { get; set; }
}