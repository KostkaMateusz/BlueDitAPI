using AutoMapper;
using Bluedit.Application.DataModels.TopicDtos;
using Bluedit.Application.Features.TopicFeatures.Queries.GetTopic;
using Bluedit.Domain.Entities;

namespace Bluedit.Application.MappingProfiles;

public class TopicMappingProfile : Profile
{
    public TopicMappingProfile()
    {
        CreateMap<Domain.Entities.Topic, TopicCreatedDto>();
        CreateMap<Domain.Entities.Topic, TopicInfoDto>();
        CreateMap<Domain.Entities.Topic, TopicForUpdateDto>().ReverseMap();
        
        CreateMap<Topic, GetTopicQueryResponse>();
    }
}