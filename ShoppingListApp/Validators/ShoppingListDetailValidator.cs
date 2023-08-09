using FluentValidation;
using ShoppingListModel.Models;

namespace ShoppingListApp.Validators
{
    public class ShoppingListDetailValidator : AbstractValidator<ShoppingListDetail>
    {
        public ShoppingListDetailValidator()
        {
            RuleFor(a => a.Quantity)
                .NotNull().WithMessage("{0} is required")
                .GreaterThan(0).WithMessage("{0} should be bigger than {1}");

            RuleFor(a => a.Note)
                .MaximumLength(100).WithMessage("{0} length should not exceed {1}.");
        }
    }
}
