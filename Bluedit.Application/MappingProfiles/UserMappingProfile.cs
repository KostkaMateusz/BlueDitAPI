using AutoMapper;
using Bluedit.Application.DataModels.UserDtos;

namespace Bluedit.Application.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<Domain.Entities.User, UserInfoDto>();
    }
}
