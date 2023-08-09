using FluentValidation;
using ShoppingListApp.ViewModels;

namespace ShoppingListApp.Validators
{
    public class ShoppingListViewModelValidator : AbstractValidator<ShoppingListViewModel>
    {
        public ShoppingListViewModelValidator()
        {
            RuleFor(a => a.Name)
                .NotNull().WithMessage("Name is required")
                .Length(2, 30).WithMessage("Name length should be between {0} and {1}.");

            RuleFor(a => a.Quantity)
                .NotNull().WithMessage("Quantity is required")
                .GreaterThan(0).WithMessage("Quantity should be bigger than {0}");

            RuleFor(a => a.Notes)
                .MaximumLength(100).WithMessage("Note length should not exceed {0}.");

            RuleFor(a => a.Image)
                .NotNull().WithMessage("Image Url is required");

            RuleFor(a => a.ShoppingListId)
                .NotNull().WithMessage("Select a Shopping List.")
                .NotEqual(0).WithMessage("Select a Shopping List.");

            RuleFor(a => a.ProductId)
                .NotNull().WithMessage("Select a Product.")
                .NotEqual(0).WithMessage("Select a Product.");
        }
    }
}
