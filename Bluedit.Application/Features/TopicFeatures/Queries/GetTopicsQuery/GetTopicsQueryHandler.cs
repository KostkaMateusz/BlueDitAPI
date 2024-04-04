using AutoMapper;
using Bluedit.Application.Contracts;
using Bluedit.Application.DataModels.TopicDtos;
using Bluedit.Domain.Entities;
using MediatR;

namespace Bluedit.Application.Features.TopicFeatures.Queries.GetTopicsQuery;

public class GetTopicsQueryHandler : IRequestHandler<TopicResourceParameters,IPagedList<Topic>>
{
    private readonly ITopicRepository _topicRepository;

    public GetTopicsQueryHandler(ITopicRepository topicRepository)
    {
        _topicRepository = topicRepository ?? throw new ArgumentNullException(nameof(topicRepository));
    }
    
    public async Task<IPagedList<Topic>> Handle(TopicResourceParameters request, CancellationToken cancellationToken)
    {
        var topicsPagedList=await _topicRepository.GetAllTopicAsync(request);

        return topicsPagedList;
    }
}