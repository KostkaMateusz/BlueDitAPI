using AutoMapper;

namespace Bluedit.Models;

public class PostMappingProfile : Profile
{
    public PostMappingProfile()
    {
        CreateMap<Entities.Post, DataModels.PostDtos.PostInfoDto>();
        CreateMap<Entities.Post, DataModels.PostDtos.PostUpdateDto>().ReverseMap();
    }
}
