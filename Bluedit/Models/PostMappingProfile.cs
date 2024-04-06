﻿using AutoMapper;
using Bluedit.Application.DataModels.PostDtos;


namespace Bluedit.Models;

public class PostMappingProfile : Profile
{
    public PostMappingProfile()
    {
        CreateMap<Domain.Entities.Post, PostInfoDto>();
        CreateMap<Domain.Entities.Post, PostUpdateDto>().ReverseMap();
        CreateMap<Domain.Entities.Post, PartialyUpdatePostDto>().ReverseMap();   
    }
}