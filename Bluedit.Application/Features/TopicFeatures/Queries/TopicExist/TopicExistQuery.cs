using MediatR;

namespace Bluedit.Application.Features.TopicFeatures.Queries.TopicExist;

public class TopicExistQuery : IRequest<bool>
{
    public string TopicName { get; set; } =string.Empty;

    public TopicExistQuery(string TopicName)
    {
        this.TopicName = TopicName;
    }
}
