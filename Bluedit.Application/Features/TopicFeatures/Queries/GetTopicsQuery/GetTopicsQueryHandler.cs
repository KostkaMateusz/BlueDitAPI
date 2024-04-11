using Bluedit.Application.Contracts;
using Bluedit.Application.DataModels.TopicDtos;
using MediatR;

namespace Bluedit.Application.Features.TopicFeatures.Queries.GetTopicsQuery;

internal class GetTopicsQueryHandler : IRequestHandler<TopicResourceParameters, IPagedList>
{
    private readonly ITopicRepository _topicRepository;

    public GetTopicsQueryHandler(ITopicRepository topicRepository)
    {
        _topicRepository = topicRepository ?? throw new ArgumentNullException(nameof(topicRepository));
    }

    public async Task<IPagedList> Handle(TopicResourceParameters request, CancellationToken cancellationToken)
    {
        var topicsPagedList = await _topicRepository.GetAllTopicAsync(request);

        return topicsPagedList;
    }
}