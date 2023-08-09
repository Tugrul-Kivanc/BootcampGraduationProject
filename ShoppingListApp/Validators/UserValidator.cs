using FluentValidation;
using ShoppingListModel.Models;

namespace ShoppingListApp.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(a => a.Name)
                .NotNull().WithMessage("{0} is required")
                .Length(2, 30).WithMessage("{0} length should be between {1} and {2}.");

            RuleFor(a => a.Email)
                .NotNull().WithMessage("{0} is required")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(a => a.Password)
                .NotNull().WithMessage("{0} is required")
                .MinimumLength(8).WithMessage("{0} should contain at least {1} characters.")
                .MaximumLength(128).WithMessage("{0} length should not exceed {1}}.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$").WithMessage("Password should contain at least one upper case letter, one lower case letter and a number.");

            RuleFor(a => a.ConfirmPassword)
                .NotNull().WithMessage("{0} is required")
                .Matches(a => a.Password).WithMessage("Passwords do not match");
        }
    }
}
