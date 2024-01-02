using MediatR;

namespace Bluedit.Application.Features.TopicFeatures.Queries.TopicExist;

public class TopicExistsQuery : IRequest<bool>
{
    public string TopicName { get; set; } =string.Empty;

    public TopicExistsQuery(string TopicName)
    {
        this.TopicName = TopicName;
    }
}
