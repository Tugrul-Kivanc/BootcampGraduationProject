using FluentValidation;
using ShoppingListApp.ViewModels;
using ShoppingListModel.Models;

namespace ShoppingListApp.Validators
{
    public class UserRegisterViewModelValidator : AbstractValidator<UserRegisterViewModel>
    {
        public UserRegisterViewModelValidator()
        {
            RuleFor(a => a.Name)
                .NotNull().WithMessage("Name is required")
                .Length(2, 30).WithMessage("Name length should be between {0} and {1}.");

            RuleFor(a => a.Email)
                .NotNull().WithMessage("Email is required")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(a => a.Password)
                .NotNull().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password should contain at least {0} characters.")
                .MaximumLength(128).WithMessage("Password length should not exceed {0}}.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$").WithMessage("Password should contain at least one upper case letter, one lower case letter and a number.");

            RuleFor(a => a.ConfirmPassword)
                .NotNull().WithMessage("Password is required");
        }
    }
}
