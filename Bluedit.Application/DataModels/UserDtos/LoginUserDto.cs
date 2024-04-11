using System.ComponentModel.DataAnnotations;

namespace Bluedit.Application.DataModels.UserDtos;

public class LoginUserDto
{
    [EmailAddress] public required string Email { get; set; }

    public required string Password { get; set; }
}