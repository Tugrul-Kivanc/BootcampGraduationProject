using FluentValidation;
using ShoppingListApp.ViewModels;

namespace ShoppingListApp.Validators
{
    public class UserLoginViewModelValidator : AbstractValidator<UserLoginViewModel>
    {
        public UserLoginViewModelValidator()
        {
            RuleFor(a => a.Email)
                .NotNull().WithMessage("Email is required")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(a => a.Password)
                .NotNull().WithMessage("Enter your Password.");
        }
    }
}
