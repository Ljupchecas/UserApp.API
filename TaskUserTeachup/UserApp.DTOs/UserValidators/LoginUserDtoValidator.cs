using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp.DTOs.User;

namespace UserApp.DTOs.UserValidators
{
    public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
    {
        public LoginUserDtoValidator()
        {
            RuleFor(dto => dto.Username)
                .NotEmpty().WithMessage("Username is required.");

            RuleFor(dto => dto.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
