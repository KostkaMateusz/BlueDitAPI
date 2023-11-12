

namespace Bluedit.Application.Features.Users.CreateUser;

public class CreateUserDto
{
    public Guid UserId { get; set; } 
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}