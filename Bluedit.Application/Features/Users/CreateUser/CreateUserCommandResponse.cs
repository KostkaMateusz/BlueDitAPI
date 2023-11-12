using Bluedit.Application.Responses;

namespace Bluedit.Application.Features.Users.CreateUser;

public class CreateUserCommandResponse : BaseResponse
{
    public CreateUserCommandResponse() : base()
    {

    }

    public CreateUserDto UserDto { get; set; } = default!;

}
