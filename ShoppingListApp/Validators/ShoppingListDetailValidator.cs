using FluentValidation;
using ShoppingListModel.Models;

namespace ShoppingListApp.Validators
{
    public class ShoppingListDetailValidator : AbstractValidator<ShoppingListDetail>
    {
        public ShoppingListDetailValidator()
        {
            RuleFor(a => a.Quantity)
                .NotNull().WithMessage("Quantity is required")
                .GreaterThan(0).WithMessage("Quantity should be bigger than {0}");

            RuleFor(a => a.Note)
                .MaximumLength(100).WithMessage("Note length should not exceed {0}.");
        }
    }
}
