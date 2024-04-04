using MediatR;

namespace Bluedit.Application.Features.TopicFeatures.Commands.PutTopic;

public class PutTopicCommand : IRequest<bool>
{
    public string TopicName { get; set; }
    public string TopicDescription { get; set; }
    public PutTopicCommand(string topicName, string topicDescription)
    {
        this.TopicName = topicName;
        this.TopicDescription = topicDescription;
    }
}