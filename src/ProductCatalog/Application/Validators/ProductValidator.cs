using FluentValidation;
using ProductCatalog.Application.Products.Dtos;

namespace Application.Products.Validators;

public class ProductValidator : AbstractValidator<CreateProductDto>
{
    public ProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Price).NotNull().WithMessage("Price is required.")
            .DependentRules(() =>
            {
                RuleFor(x => x.Price.Amount).GreaterThan(0).WithMessage("Price amount must be greater than 0.");
                RuleFor(x => x.Price.Code).NotEmpty()
                    .Must(BeValudCurrencyCode).WithMessage("Invalid currency code value");
            });
        RuleFor(x => x.ImageUrl).NotEmpty();
        RuleFor(x => x.Category)
            .NotEmpty()
            .Must(BeAValidCategory).WithMessage("Invalid category value");
    }

    private bool BeAValidCategory(string category)
    {
        return category switch
        {
            "clothing" => true,
            "jewelery" => true,
            "electronics" => true,
            _ => false
        };
    }

    private bool BeValudCurrencyCode(string category)
    {
        return category switch
        {
            "USD" => true,
            "CAD" => true,
            "EUR" => true,
            _ => false
        };
    }
}
