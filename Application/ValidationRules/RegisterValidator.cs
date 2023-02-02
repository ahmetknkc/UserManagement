using Domain.Models.Authentication.SignUp;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ValidationRules
{
    public class RegisterValidator : AbstractValidator<RegisterUser>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.email)
                .NotEmpty().WithMessage("E-Mail is required.")
                .EmailAddress().WithMessage("Enter a valid email address.");

            RuleFor(x => x.password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Enter a minimum of 6 characters.")
                .MaximumLength(30).WithMessage("Enter a maximum of 30 characters.")
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.");


            RuleFor(x => x.username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(4).WithMessage("Enter a minimum of 4 characters.")
                .MaximumLength(20).WithMessage("Enter a maximum of 30 characters.");


        }
    }
}