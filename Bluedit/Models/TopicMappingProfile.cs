using AutoMapper;

namespace Bluedit.Models;

public class TopicMappingProfile : Profile
{
    public TopicMappingProfile()
    {
        CreateMap<Entities.Topic, DataModels.TopicDtos.TopicCreateDto>();
        CreateMap<Entities.Topic, DataModels.TopicDtos.TopicInfoDto>();//.ForMember(dto => dto.PostCount, entity => entity.MapFrom(src => src.Posts.Count()));
        CreateMap<Entities.Topic, DataModels.TopicDtos.TopicForUpdateDto>().ReverseMap();
    }
}
