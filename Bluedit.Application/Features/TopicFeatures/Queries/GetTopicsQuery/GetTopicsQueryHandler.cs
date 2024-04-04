using AutoMapper;
using Bluedit.Application.Contracts;
using Bluedit.Application.DataModels.TopicDtos;
using Bluedit.Domain.Entities;
using MediatR;

namespace Bluedit.Application.Features.TopicFeatures.Queries.GetTopicsQuery;

public class GetTopicsQueryHandler : IRequestHandler<TopicResourceParameters,IPagedList<Topic>>
{
    private readonly ITopicRepository _topicRepository;
    private readonly IMapper _mapper;

    public GetTopicsQueryHandler(ITopicRepository topicRepository, IMapper mapper)
    {
        _topicRepository = topicRepository ?? throw new ArgumentNullException(nameof(topicRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    public async Task<IPagedList<Topic>> Handle(TopicResourceParameters request, CancellationToken cancellationToken)
    {
        var topicsPagedList=await _topicRepository.GetAllTopicAsync(request);

        foreach (var topic in (IEnumerable<Topic>)topicsPagedList)
        { 
            topic.PostCount =await _topicRepository.GetTopicPostsCountAsync(topic.TopicName);
        }
        return topicsPagedList;
    }
}