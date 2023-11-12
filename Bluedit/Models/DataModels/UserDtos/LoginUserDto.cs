using System.ComponentModel.DataAnnotations;

namespace Bluedit.Models.DataModels.UserDtos;

public record LoginUserDto
{
    [EmailAddress]
    public required string Email { get; set; }

    public required string Password { get; set; }
}