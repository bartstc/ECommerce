using FluentValidation;
using ProductCatalog.Application.Products.Dtos;

namespace Application.Products.Validators;

public class AddProductValidator : AbstractValidator<AddProductDto>
{
    public AddProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Price).NotNull().WithMessage("Price is required.")
            .DependentRules(() =>
            {
                RuleFor(x => x.Price.Amount).GreaterThan(0).WithMessage("Price amount must be greater than 0.");
                RuleFor(x => x.Price.Code).NotEmpty()
                    .Must(Validator.BeValidCurrencyCode).WithMessage("Invalid currency code value");
            });
        RuleFor(x => x.ImageUrl).NotEmpty();
        RuleFor(x => x.Category)
            .NotEmpty()
            .Must(Validator.BeValidCategory).WithMessage("Invalid category value");
    }
}