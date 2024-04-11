using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Bluedit.Application.DataModels;

public class ImageFormatValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null)
            return false;

        if (value is not IFormFile image)
            return false;
        return image.ContentType.Equals("image/jpeg")
               || image.ContentType.Equals("image/jpg")
               || image.ContentType.Equals("image/png");
    }

    public override string FormatErrorMessage(string name)
    {
        return ErrorMessageString;
    }
}