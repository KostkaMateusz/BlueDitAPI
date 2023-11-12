using AutoMapper;

namespace Bluedit.Models;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<Bluedit.Domain.Entities.User, DataModels.UserDtos.UserInfoDto>();
    }
}
