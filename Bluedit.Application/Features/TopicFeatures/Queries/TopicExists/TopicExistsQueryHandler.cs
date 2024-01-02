using Bluedit.Services.Repositories.TopicRepo;
using MediatR;

namespace Bluedit.Application.Features.TopicFeatures.Queries.TopicExist;

public class TopicExistsQueryHandler : IRequestHandler<TopicExistsQuery, bool>
{
    private readonly ITopicRepository _topicRepository;

    public TopicExistsQueryHandler(ITopicRepository topicRepository)
    {
        _topicRepository = topicRepository ?? throw new ArgumentNullException(nameof(topicRepository));
    }

    public async Task<bool> Handle(TopicExistsQuery request, CancellationToken cancellationToken)
    {
        return await _topicRepository.IsTopicExistAsync(request.TopicName);
    }
}
