using Bluedit.Services.Repositories.TopicRepo;
using MediatR;

namespace Bluedit.Application.Features.TopicFeatures.Queries.TopicExist;

public class TopicExistQueryHandler : IRequestHandler<TopicExistQuery, bool>
{
    private readonly ITopicRepository _topicRepository;

    public TopicExistQueryHandler(ITopicRepository topicRepository)
    {
        _topicRepository = topicRepository ?? throw new ArgumentNullException(nameof(topicRepository));
    }

    public async Task<bool> Handle(TopicExistQuery request, CancellationToken cancellationToken)
    {
        return await _topicRepository.IsTopicExistAsync(request.TopicName);
    }
}
