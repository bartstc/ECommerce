using Application.Products.Dtos;
using FluentValidation;

namespace Application.Products.Validators
{
    public class ProductValidator : AbstractValidator<CreateProductDto>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Price).NotNull().WithMessage("Price is required.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Price.Amount).GreaterThan(0).WithMessage("Price amount must be greater than 0.");
                    RuleFor(x => x.Price.Currency).NotEmpty().WithMessage("Price currency is required.");
                });
            RuleFor(x => x.Rating).NotNull().WithMessage("Rating is required.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Rating.Rate).InclusiveBetween(0, 5).WithMessage("Rating rate must be between 0 and 5.");
                    RuleFor(x => x.Rating.Count).GreaterThanOrEqualTo(0).WithMessage("Rating count must be greater than or equal to 0.");
                });
            RuleFor(x => x.Image).NotEmpty();
            RuleFor(x => x.Category)
                .NotEmpty()
                .Must(BeAValidCategory).WithMessage("Invalid category value");
        }

        private bool BeAValidCategory(string category)
        {
            return category switch
            {
                "men's clothing" => true,
                "women's clothing" => true,
                "jewelery" => true,
                "electronics" => true,
                _ => false
            };
        }
    }
}