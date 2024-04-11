using AutoMapper;
using Bluedit.Application.DataModels.TopicDtos;
using Bluedit.Application.Features.TopicFeatures.Queries.GetTopic;
using Bluedit.Domain.Entities;

namespace Bluedit.Application.MappingProfiles;

public class TopicMappingProfile : Profile
{
    public TopicMappingProfile()
    {
        CreateMap<Topic, TopicCreatedDto>();
        CreateMap<Topic, TopicInfoDto>();
        CreateMap<Topic, TopicForUpdateDto>().ReverseMap();

        CreateMap<Topic, GetTopicQueryResponse>();
    }
}