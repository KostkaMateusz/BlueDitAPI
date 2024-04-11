using MediatR;

namespace Bluedit.Application.Features.TopicFeatures.Queries.TopicExists;

public class TopicExistsQuery : IRequest<bool>
{
    public TopicExistsQuery(string topicName)
    {
        TopicName = topicName;
    }

    public string TopicName { get; }
}