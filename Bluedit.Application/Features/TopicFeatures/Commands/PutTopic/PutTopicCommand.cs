using MediatR;

namespace Bluedit.Application.Features.TopicFeatures.Commands.PutTopic;

public class PutTopicCommand : IRequest<bool>
{
    public PutTopicCommand(string topicName, string topicDescription)
    {
        TopicName = topicName;
        TopicDescription = topicDescription;
    }

    public string TopicName { get; }
    public string TopicDescription { get; }
}