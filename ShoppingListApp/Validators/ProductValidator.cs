using FluentValidation;
using ShoppingListModel.Models;

namespace ShoppingListApp.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(a => a.Name)
                .NotNull().WithMessage("Name is required")
                .Length(2, 30).WithMessage("Name length should be between {0} and {1}.");

            RuleFor(a => a.Image)
                .NotNull().WithMessage("Image Url is required");
        }
    }
}
