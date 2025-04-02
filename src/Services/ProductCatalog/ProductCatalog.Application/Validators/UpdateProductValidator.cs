using FluentValidation;
using ProductCatalog.Application.Products.Dtos;

namespace Application.Products.Validators;

public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.ImageUrl).NotEmpty();
    }
}
