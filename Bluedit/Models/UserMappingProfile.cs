using AutoMapper;
using Bluedit.Application.DataModels.UserDtos;

namespace Bluedit.Models;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<Bluedit.Domain.Entities.User, UserInfoDto>();
    }
}
