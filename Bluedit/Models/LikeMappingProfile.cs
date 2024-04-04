using AutoMapper;
using Bluedit.Application.DataModels.LikesDto;
using Bluedit.Domain.Entities.LikeEntities;

namespace Bluedit.Models;

public class LikeMappingProfile : Profile
{
    public LikeMappingProfile()
    {
        CreateMap<LikesUserInfoDto,LikeBase>().ReverseMap();

        CreateMap<PostLike, PostLikesDto>().ForMember(dto => dto.UserName, opt => opt.MapFrom(src => src.User.Name));

        CreateMap<ReplyLike, ReplyLikesDto>().ForMember(dto => dto.UserName, opt => opt.MapFrom(src => src.User.Name));
    }
}