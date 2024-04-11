using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Bluedit.Application.DataModels.PostDtos;

public class PostCreateDto
{
    [MinLength(3)] public required string Title { get; set; }

    public string? Description { get; set; }

    [Required]
    [ImageFormatValidation(ErrorMessage = "Invalid File Type, Allowed file types:jpg,jpeg,png")]
    public required IFormFile Image { get; set; }
}