using Application.Core;
using Application.Products.Mappers;
using Application.Products.Dtos;
using Application.Products.Validators;
using FluentValidation;
using MediatR;
using Domain;

namespace Application.Products
{
    public class Create
    {
        public record Command(CreateProductDto ProductDto) : IRequest<Result<Unit>>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.ProductDto).SetValidator(new ProductValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IProductsRepository _productsRepository;

            public Handler(IProductsRepository productsRepository)
            {
                _productsRepository = productsRepository;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = request.ProductDto.ToDomain();

                var result = await _productsRepository.CreateProduct(product);

                if (!result) return Result<Unit>.Failure("Failed to create product");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}