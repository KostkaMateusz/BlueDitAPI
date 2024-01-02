using MediatR;

namespace Bluedit.Application.Features.TopicFeatures.Commands.DeleteTopic;

public class DeleteTopicCommand : IRequest<bool>
{
    public string TopicName { get; set; } = string.Empty;

    public DeleteTopicCommand(string TopicName)
    {
        this.TopicName = TopicName;
    }
}
