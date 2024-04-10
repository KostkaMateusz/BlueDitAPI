using MediatR;
using AutoMapper;
using Bluedit.Application.Contracts;

namespace Bluedit.Application.Features.TopicFeatures.Queries.GetTopic;

internal class GetTopicQueryHandler : IRequestHandler<GetTopicQuery,GetTopicQueryResponse?>
{
    private readonly ITopicRepository _topicRepository;
    private readonly IMapper _mapper;

    public GetTopicQueryHandler(ITopicRepository topicRepository, IMapper mapper)
    {
        _topicRepository = topicRepository ?? throw new ArgumentNullException(nameof(topicRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<GetTopicQueryResponse?> Handle(GetTopicQuery request, CancellationToken cancellationToken)
    {
        var topic=await _topicRepository.GetTopicWithNameAsync(request.TopicName);

        if (topic is null)        
            return null;       

        var response=_mapper.Map<GetTopicQueryResponse>(topic);

        response.PostCount= await _topicRepository.GetTopicPostsCountAsync(topic.TopicName);

        return response;
    }
}
