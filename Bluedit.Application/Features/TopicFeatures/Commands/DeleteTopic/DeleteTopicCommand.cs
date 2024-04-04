using MediatR;

namespace Bluedit.Application.Features.TopicFeatures.Commands.DeleteTopic;

public class DeleteTopicCommand : IRequest<bool>
{
    public string TopicName { get; set; }

    public DeleteTopicCommand(string topicName)
    {
        this.TopicName = topicName;
    }
}
