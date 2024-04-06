using AutoMapper;
using Bluedit.Application.DataModels.TopicDtos;

namespace Bluedit.Models;

public class TopicMappingProfile : Profile
{
    public TopicMappingProfile()
    {
        CreateMap<Domain.Entities.Topic, TopicCreatedDto>();
        CreateMap<Domain.Entities.Topic, TopicInfoDto>();
        CreateMap<Domain.Entities.Topic, TopicForUpdateDto>().ReverseMap();
    }
}