
using MediatR;

namespace Bluedit.Application.Features.TopicFeatures.Commands.PutTopic;

public class PutTopicCommand : IRequest<bool>
{
    public string TopicName { get; set; } = string.Empty;
    public string TopicDescription { get; set; } = string.Empty;
    public PutTopicCommand(string TopicName, string TopicDescription)
    {
        this.TopicName = TopicName;
        this.TopicDescription = TopicDescription;
    }
}
