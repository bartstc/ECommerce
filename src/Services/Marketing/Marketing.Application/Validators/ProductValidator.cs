using Marketing.Application.Dtos;

namespace Marketing.Application.Validators;

public class ProductValidator : AbstractValidator<CreateProductDto>
{
    public ProductValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .Must(value => value is Guid)
            .WithMessage("Product Id must be a Guid");
    }
}

