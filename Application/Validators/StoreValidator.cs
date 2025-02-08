using Application.Stores.Dtos;
using FluentValidation;

namespace Application.Validators
{
    public class StoreValidator : AbstractValidator<CreateStoreDto>
    {
        public StoreValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}