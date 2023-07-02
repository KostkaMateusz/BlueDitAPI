using System.ComponentModel.DataAnnotations;

namespace Bluedit.Models.ModelsValidators;

public class ImageFormatValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
       var image= (IFormFile)value;

        if(image is null) 
            return false;
        else
            return image.ContentType.Equals("image/jpeg") 
                || image.ContentType.Equals("image/jpg") 
                || image.ContentType.Equals("image/png");
    }

    public override string FormatErrorMessage(string name)
    {        
        return ErrorMessageString;
    }
}
