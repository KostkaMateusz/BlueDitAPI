using System.ComponentModel.DataAnnotations;

namespace Bluedit.Models.DataModels.UserDtos;

public class ResetPasswordDto
{
    public required string resetCode { get; set; }

    [EmailAddress]
    [MinLength(6)]
    [MaxLength(60)]
    public required string userMail { get; set; }

    [MinLength(6)]
    [MaxLength(60)]
    public required string newPassword { get; set; }
}