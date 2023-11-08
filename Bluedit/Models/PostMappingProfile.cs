using AutoMapper;

namespace Bluedit.Models;

public class PostMappingProfile : Profile
{
    public PostMappingProfile()
    {
        CreateMap<Bluedit.Domain.Entities.Post, DataModels.PostDtos.PostInfoDto>();
        CreateMap<Bluedit.Domain.Entities.Post, DataModels.PostDtos.PostUpdateDto>().ReverseMap();
    }
}