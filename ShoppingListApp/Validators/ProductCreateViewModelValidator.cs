using FluentValidation;
using ShoppingListApp.ViewModels;

namespace ShoppingListApp.Validators
{
    public class ProductCreateViewModelValidator : AbstractValidator<ProductCreateViewModel>
    {
        public ProductCreateViewModelValidator()
        {
            RuleFor(a => a.CategoryId)
                .NotNull().WithMessage("Select a category.")
                .NotEqual(0).WithMessage("Select a category.");

            RuleFor(a => a.Name)
                .NotNull().WithMessage("Name is required")
                .Length(2, 30).WithMessage("Name length should be between {0} and {1}.");

            RuleFor(a => a.Image)
                .NotNull().WithMessage("Image Url is required");
        }
    }
}
