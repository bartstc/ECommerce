using FluentValidation;
using ProductCatalog.Application.Products.Dtos;

namespace Application.Products.Validators;

public class UpdatePriceValidator : AbstractValidator<UpdatePriceDto>
{
    public UpdatePriceValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Price amount must be greater than 0.");
        RuleFor(x => x.Code).NotEmpty()
            .Must(Validator.BeValidCurrencyCode).WithMessage("Invalid currency code value");
    }
}