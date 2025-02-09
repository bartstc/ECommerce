using Application.Core;
using Application.Products.Mappers;
using Application.Products.Dtos;
using Application.Products.Validators;
using FluentValidation;
using MediatR;
using Domain;
using Domain.Errors;

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
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = request.ProductDto.ToDomain();

                _productRepository.AddProduct(product);

                var result = await _productRepository.Complete();

                if (!result) return Result<Unit>.Failure(ProductsError.FailedToCreateProduct);

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}