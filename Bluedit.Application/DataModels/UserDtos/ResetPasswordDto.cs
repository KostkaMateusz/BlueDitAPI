using System.ComponentModel.DataAnnotations;

namespace Bluedit.Application.DataModels.UserDtos;

public class ResetPasswordDto
{
    public required string ResetCode { get; set; }

    [EmailAddress]
    [MinLength(6)]
    [MaxLength(60)]
    public required string UserMail { get; set; }

    [MinLength(6)]
    [MaxLength(60)]
    public required string NewPassword { get; set; }
}