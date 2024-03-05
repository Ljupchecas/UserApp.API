using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp.DTOs.User;

namespace UserApp.DTOs.UserValidators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator()
        {
            RuleFor(dto => dto.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(30).WithMessage("First name cannot exceed 30 characters.");

            RuleFor(dto => dto.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(30).WithMessage("Last name cannot exceed 30 characters.");

            RuleFor(dto => dto.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(20).WithMessage("Username cannot exceed 20 characters.");

            RuleFor(dto => dto.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(dto => dto.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(dto => dto.ConfirmPassword)
                .Equal(dto => dto.Password).WithMessage("Passwords must match.");
        }
    }
}
