using MediatR;

namespace Bluedit.Application.Features.TopicFeatures.Commands.DeleteTopic;

public class DeleteTopicCommand : IRequest<bool>
{
    public DeleteTopicCommand(string topicName)
    {
        TopicName = topicName;
    }

    public string TopicName { get; set; }
}