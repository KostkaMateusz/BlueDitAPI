using AutoMapper;
using Bluedit.Application.DataModels.LikesDto;
using Bluedit.Domain.Entities.LikeEntities;

namespace Bluedit.Application.MappingProfiles;

public class LikeMappingProfile : Profile
{
    public LikeMappingProfile()
    {
        CreateMap<LikesUserInfoDto, LikeBase>().ReverseMap();

        CreateMap<PostLike, LikesDto>().ForMember(dto => dto.UserName, opt => opt.MapFrom(src => src.User!.Name));

        CreateMap<ReplyLike, LikesDto>().ForMember(dto => dto.UserName, opt => opt.MapFrom(src => src.User!.Name));
    }
}