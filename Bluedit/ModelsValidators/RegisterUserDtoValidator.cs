using Bluedit.Application.DataModels.UserDtos;
using Bluedit.Persistence;
using FluentValidation;

namespace Bluedit.ModelsValidators;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator(BlueditDbContext dbContext)
    {

        RuleFor(registerUserDto => registerUserDto.Name).MinimumLength(5);
        RuleFor(registerUserDto => registerUserDto.Name).Custom((value, context) =>
        {
            var nameInUse = dbContext.Users.Any(u => u.Name == value);
            if (nameInUse)
            {
                context.AddFailure("User Name", "That User Name is taken");
            }
        });

        RuleFor(registerUserDto => registerUserDto.Password).MinimumLength(6);

        RuleFor(registerUserDto => registerUserDto.Email).NotEmpty().EmailAddress();

        RuleFor(registerUserDto => registerUserDto.Email).Custom((value, context) =>
            {
                var emailInUse = dbContext.Users.Any(u => u.Email == value);
                if (emailInUse)
                {
                    context.AddFailure("Email", "That email is taken");
                }
            });
    }
}
