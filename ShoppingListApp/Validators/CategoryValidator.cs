using FluentValidation;
using ShoppingListModel.Models;

namespace ShoppingListApp.Validators
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(a => a.Name)
                .NotNull().WithMessage("Name is required")
                .Length(2, 30).WithMessage("Name length should be between {0} and {1}.");
        }
    }
}
