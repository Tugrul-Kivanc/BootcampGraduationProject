using FluentValidation;
using ShoppingListModel.Models;

namespace ShoppingListApp.Validators
{
    public class ShoppingListValidator : AbstractValidator<ShoppingList>
    {
        public ShoppingListValidator()
        {
            RuleFor(a => a.ShoppingListName)
                .NotNull().WithMessage("Name is required")
                .Length(2, 30).WithMessage("Name length should be between {0} and {1}.");
        }
    }
}
