using FluentValidation;
using ShoppingListModel.Models;

namespace ShoppingListApp.Validators
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(a => a.Name)
                .NotNull().WithMessage("{0} is required")
                .Length(2, 30).WithMessage("{0} length should be between {1} and {2}.");
        }
    }
}
