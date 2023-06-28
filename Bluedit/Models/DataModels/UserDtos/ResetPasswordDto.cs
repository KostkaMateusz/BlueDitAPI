using System.ComponentModel.DataAnnotations;

namespace Bluedit.Models.DataModels.UserDtos;

public class ResetPasswordDto
{
    [Required]    
    public string resetCode { get; set; }
    
    [Required]
    [EmailAddress]
    [MinLength(6)]
    [MaxLength(60)]
    public string userMail { get; set; }

    [Required]
    [MinLength(6)]
    [MaxLength(60)]
    public string newPassword { get; set; }
}
