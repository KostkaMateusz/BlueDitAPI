
namespace Bluedit.Models.DataModels.UserDtos;

public record RegisterUserDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}