using AutoMapper;
using Bluedit.Domain.Entities.LikeEntities;
using Bluedit.Models.DataModels.LikesDto;

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