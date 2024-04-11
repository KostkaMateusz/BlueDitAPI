using AutoMapper;
using Bluedit.Application.DataModels.UserDtos;
using Bluedit.Domain.Entities;

namespace Bluedit.Application.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserInfoDto>();
    }
}