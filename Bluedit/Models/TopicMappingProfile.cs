using AutoMapper;

namespace Bluedit.Models;

public class TopicMappingProfile : Profile
{
    public TopicMappingProfile()
    {
        CreateMap<Bluedit.Domain.Entities.Topic, DataModels.TopicDtos.TopicCreateDto>();
        CreateMap<Bluedit.Domain.Entities.Topic, DataModels.TopicDtos.TopicInfoDto>();
        CreateMap<Bluedit.Domain.Entities.Topic, DataModels.TopicDtos.TopicForUpdateDto>().ReverseMap();
    }
}