using Application.Core;
using Application.Products.Dtos;
using Application.Products.Mappers;
using Application.Products.Validators;
using Domain;
using FluentValidation;
using MediatR;

namespace Application.Products
{
    public class Edit
    {
        public record Command(Guid Id, CreateProductDto ProductDto) : IRequest<Result<Unit>>;

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
                var product = await _productRepository.GetProduct(request.Id);

                if (product == null) return null;

                var updatedProduct = request.ProductDto.ToDomain(product);

                var result = await _productRepository.UpdateProduct(updatedProduct);

                if (!result) return Result<Unit>.Failure("Failed to edit the product");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}