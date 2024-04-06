using MediatR;

namespace Bluedit.Application.Features.TopicFeatures.Queries.TopicExists;

public class TopicExistsQuery : IRequest<bool>
{
    public string TopicName { get; set; }

    public TopicExistsQuery(string topicName)
    {
        this.TopicName = topicName;
    }
}
