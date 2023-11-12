using System.ComponentModel.DataAnnotations;

namespace Bluedit.Models.DataModels.UserDtos;

public class ForgotPasswordDto
{
    [MaxLength(255)]
    [MinLength(3)]
    [EmailAddress]
    public required string Email { get; set; }
}