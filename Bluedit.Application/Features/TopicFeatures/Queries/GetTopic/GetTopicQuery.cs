using MediatR;

namespace Bluedit.Application.Features.TopicFeatures.Queries.GetTopic;

public class GetTopicQuery : IRequest<GetTopicQueryResponse?>
{
    public GetTopicQuery(string topicName)
    {
        TopicName = topicName;
    }

    public string TopicName { get; }
}