using FluentValidation;
using ShoppingListApp.ViewModels;

namespace ShoppingListApp.Validators
{
    public class CategoryCreateViewModelValidator : AbstractValidator<CategoryCreateViewModel>
    {
        public CategoryCreateViewModelValidator()
        {
            RuleFor(a => a.Name)
                .NotNull().WithMessage("Name is required")
                .Length(2, 30).WithMessage("Name length should be between {0} and {1}.");
        }
    }
}
