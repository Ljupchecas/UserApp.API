using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp.DataAccess.Interfaces;
using UserApp.DTOs.User;

namespace UserApp.DTOs.UserValidators
{
    public class LoginHistoryDtoValidator : AbstractValidator<LoginHistoryDto>
    {
        public LoginHistoryDtoValidator()
        {
            RuleFor(dto => dto.Username)
                .NotEmpty().WithMessage("Username is required.");
        }
    }
}
