using AutoMapper;
using Bluedit.Application.DataModels.TopicDtos;

namespace Bluedit.Models;

public class TopicMappingProfile : Profile
{
    public TopicMappingProfile()
    {
        CreateMap<Bluedit.Domain.Entities.Topic, TopicCreatedDto>();
        CreateMap<Bluedit.Domain.Entities.Topic, TopicInfoDto>();
        CreateMap<Bluedit.Domain.Entities.Topic, TopicForUpdateDto>().ReverseMap();
    }
}