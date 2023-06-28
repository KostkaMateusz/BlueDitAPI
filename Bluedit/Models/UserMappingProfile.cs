using AutoMapper;

namespace Bluedit.Models;

public class UserMappingProfile : Profile
{
	public UserMappingProfile()
	{
		CreateMap<Entities.User, DataModels.UserDtos.UserInfoDto>();
    }
}
