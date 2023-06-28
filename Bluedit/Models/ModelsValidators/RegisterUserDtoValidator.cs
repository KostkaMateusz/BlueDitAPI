﻿using FluentValidation;
using Bluedit.Entities;
using Microsoft.EntityFrameworkCore;
using Bluedit.Models.DataModels.UserDtos;

namespace Bluedit.Models.ModelsValidators;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
	public RegisterUserDtoValidator(ApplicationDbContext dbContext)
	{

        RuleFor(RegisterUserDto => RegisterUserDto.Name).MinimumLength(5);
        RuleFor(RegisterUserDto => RegisterUserDto.Name).Custom((value, context) =>
        {
            var nameInUse = dbContext.Users.Any(u => u.Name == value);
            if (nameInUse)
            {
                context.AddFailure("User Name", "That User Name is taken");
            }
        }); ;

        RuleFor(RegisterUserDto => RegisterUserDto.Password).MinimumLength(6);

        RuleFor(RegisterUserDto => RegisterUserDto.Email).NotEmpty().EmailAddress();

        RuleFor(RegisterUserDto => RegisterUserDto.Email).Custom((value, context) =>
            {
                var emailInUse = dbContext.Users.Any(u => u.Email == value);
                if (emailInUse)
                {
                    context.AddFailure("Email", "That email is taken");
                }
            });
    }
}
