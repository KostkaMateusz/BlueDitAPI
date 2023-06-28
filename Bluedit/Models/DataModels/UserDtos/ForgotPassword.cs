using System.ComponentModel.DataAnnotations;

namespace Bluedit.Models.DataModels.UserDtos;

public class ForgotPasswordDto
{

    [Required]
    [MaxLength(255)]
    [MinLength(3)]
    [EmailAddress]
    public string Email { get; set; }
}
