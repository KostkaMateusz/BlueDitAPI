using AutoMapper;
using Bluedit.Application.DataModels.PostDtos;


namespace Bluedit.Models;

public class PostMappingProfile : Profile
{
    public PostMappingProfile()
    {
        CreateMap<Bluedit.Domain.Entities.Post, PostInfoDto>();
        CreateMap<Bluedit.Domain.Entities.Post, PostUpdateDto>().ReverseMap();
        CreateMap<Bluedit.Domain.Entities.Post, PartialyUpdatePostDto>().ReverseMap();   
    }
}