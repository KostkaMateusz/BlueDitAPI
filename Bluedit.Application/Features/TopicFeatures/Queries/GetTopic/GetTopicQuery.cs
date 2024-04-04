using MediatR;

namespace Bluedit.Application.Features.TopicFeatures.Queries.GetTopic;

public class GetTopicQuery : IRequest<GetTopicQueryResponse?>
{
    public string TopicName { get; set; }

    public GetTopicQuery(string topicName)
    {
        this.TopicName = topicName;
    }
}
