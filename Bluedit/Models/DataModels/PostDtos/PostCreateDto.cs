using Bluedit.Models.ModelsValidators;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Bluedit.Models.DataModels.PostDtos;

public record PostCreateDto
{
    [MinLength(3)]
    public required string Title { get; set; }
    public string? Description { get; set; }

    [NotNull]
    [Required]
    [ImageFormatValidation(ErrorMessage = "Invalid File Type, Allowed file types:jpg,jpeg,png")]
    public required IFormFile image { get; set; }
}