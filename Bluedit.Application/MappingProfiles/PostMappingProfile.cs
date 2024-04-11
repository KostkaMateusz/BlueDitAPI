using AutoMapper;
using Bluedit.Application.DataModels.PostDtos;
using Bluedit.Domain.Entities;

namespace Bluedit.Application.MappingProfiles;

public class PostMappingProfile : Profile
{
    public PostMappingProfile()
    {
        CreateMap<Post, PostInfoDto>();
        CreateMap<Post, PostUpdateDto>().ReverseMap();
        CreateMap<Post, PartialyUpdatePostDto>().ReverseMap();
    }
}