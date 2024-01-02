using MediatR;

namespace Bluedit.Application.Features.TopicFeatures.Queries.GetTopic;

public class GetTopicQuery : IRequest<GetTopicQueryResponse?>
{
    public string TopicName { get; set; }=string.Empty;

    public GetTopicQuery(string TopicName)
    {
        this.TopicName = TopicName;
    }
}
